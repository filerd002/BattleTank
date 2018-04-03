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
        private const int MAX_AXIS_VALUE = 5000;

        public static GenericGamepadTankActionProvider DefaultPlayerOneGamepadProvider { get; } = new GenericGamepadTankActionProvider(2, 0, 7);
        public static GenericGamepadTankActionProvider DefaultPlayerTwoGamepadProvider { get; } = new GenericGamepadTankActionProvider(2, 0, 7);
        public int SpeedBoostButtonNumber { get; set; }
        public int PlantMineButtonNumber { get; set; }
        public int FireButtonNumber { get; set; }

        private Joystick _joystick;

        public GenericGamepadTankActionProvider(int speedBoostButtonNumber, int plantMineButtonNumber, int fireButtonNumber)
        {
            SpeedBoostButtonNumber = speedBoostButtonNumber;
            PlantMineButtonNumber = plantMineButtonNumber;
            FireButtonNumber = fireButtonNumber;

            Initialize(1);
            Initialize(2);
        }

        private bool Initialize(int pad)
        {
            DirectInput dinput = new DirectInput();
            if (pad == 1)
            {
                DeviceInstance device1 = dinput.GetDevices(DeviceClass.GameController, DeviceEnumerationFlags.AttachedOnly).FirstOrDefault();
                if (device1 is null) return false;

                _joystick = new Joystick(dinput, device1.InstanceGuid);

            }
            if (pad == 2)
            {
                DeviceInstance device2 = dinput.GetDevices(DeviceClass.GameController, DeviceEnumerationFlags.AttachedOnly).LastOrDefault();
                if (device2 is null || dinput.GetDevices(DeviceClass.GameController, DeviceEnumerationFlags.AttachedOnly).Count==1 ) return false;

                _joystick = new Joystick(dinput, device2.InstanceGuid);
            }
          

            foreach (DeviceObjectInstance doi in _joystick.GetObjects(ObjectDeviceType.Axis))
            {
                _joystick.GetObjectPropertiesById((int) doi.ObjectType).SetRange(-MAX_AXIS_VALUE, MAX_AXIS_VALUE);
            }
            _joystick.Properties.AxisMode = DeviceAxisMode.Absolute;

            _joystick.Acquire();
            return true;
        }

        /// <inheritdoc />
        public TankControllerState GetTankControllerState()
        {
            JoystickState state = _joystick.GetCurrentState();

            if (_joystick.Poll().IsFailure)
            {
                throw new Exception("Nie udało się połączyć z joystickiem");
            }
            if (_joystick.GetCurrentState(ref state).IsFailure)
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

        public static bool IsAnyAvailbableGamePad1()
            => DefaultPlayerOneGamepadProvider.Initialize(1);

        public static bool IsAnyAvailbableGamePad2()
          => DefaultPlayerTwoGamepadProvider.Initialize(2);

        #region Overrides
        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (base.Equals(obj)) return true;
            if (!(obj is GenericGamepadTankActionProvider provider)) return false;

            return provider.FireButtonNumber == this.FireButtonNumber
                   && provider.PlantMineButtonNumber == this.PlantMineButtonNumber
                   && provider.SpeedBoostButtonNumber == this.SpeedBoostButtonNumber;
        }
        #endregion
    }
}
