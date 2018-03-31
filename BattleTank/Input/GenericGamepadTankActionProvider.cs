using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using SlimDX.DirectInput;

namespace BattleTank.Input
{
    class GenericGamepadTankActionProvider : ITankActionProvider
    {
        private Joystick Joystick;
        private const int MAX_AXIS_VALUE = 5000;
        public static GenericGamepadTankActionProvider DefaultGamepadProvider { get; } = new GenericGamepadTankActionProvider(2, 0, 7);
        public int SpeedBoostButtonNumber { get; set; }
        public int PlantMineButtonNumber { get; set; }
        public int FireButtonNumber { get; set; }
        public GenericGamepadTankActionProvider(int speedBoostButtonNumber, int plantMineButtonNumber, int fireButtonNumber)
        {
            SpeedBoostButtonNumber = speedBoostButtonNumber;
            PlantMineButtonNumber = plantMineButtonNumber;
            FireButtonNumber = fireButtonNumber;

            Initialize();
        }

        private void Initialize()
        {
            DirectInput dinput = new DirectInput();
            DeviceInstance devices = dinput.GetDevices(DeviceClass.GameController, DeviceEnumerationFlags.AttachedOnly).First();
            Joystick = new SlimDX.DirectInput.Joystick(dinput, devices.InstanceGuid);

            foreach (DeviceObjectInstance doi in Joystick.GetObjects(ObjectDeviceType.Axis))
            {
                Joystick.GetObjectPropertiesById((int) doi.ObjectType).SetRange(-MAX_AXIS_VALUE, MAX_AXIS_VALUE);
            }
            Joystick.Properties.AxisMode = DeviceAxisMode.Absolute;

            Joystick.Acquire();
        }

        /// <inheritdoc />
        public TankControllerState GetTankControllerState()
        {
            JoystickState state = Joystick.GetCurrentState();

            if (Joystick.Poll().IsFailure)
            {
                throw new Exception("Nie udało się połączyć z joystickiem");
            }
            if (Joystick.GetCurrentState(ref state).IsFailure)
            {
                throw new Exception("Nie udało się pobrać danych z joystika");
            }

            bool[] buttons = state.GetButtons();
            return new TankControllerState(
                moveX: (float)state.X / MAX_AXIS_VALUE,
                moveY: (float)-state.Y / MAX_AXIS_VALUE,
                speedBoost: buttons[SpeedBoostButtonNumber],
                plantMine: buttons[PlantMineButtonNumber],
                fire: buttons[FireButtonNumber]);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (base.Equals(obj)) return true;
            if (!(obj is GenericGamepadTankActionProvider provider)) return false;

            return provider.FireButtonNumber == this.FireButtonNumber
                   && provider.PlantMineButtonNumber == this.PlantMineButtonNumber
                   && provider.SpeedBoostButtonNumber == this.SpeedBoostButtonNumber;
        }
    }
}
