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
        public GenericGamepadTankActionProvider()
        {
            DirectInput dinput = new DirectInput();
            var devices = dinput.GetDevices(DeviceClass.GameController, DeviceEnumerationFlags.AttachedOnly).First();
            Joystick = new SlimDX.DirectInput.Joystick(dinput, devices.InstanceGuid);

            foreach (DeviceObjectInstance doi in Joystick.GetObjects(ObjectDeviceType.Axis))
            {
                Joystick.GetObjectPropertiesById((int)doi.ObjectType).SetRange(Int16.MinValue, Int16.MaxValue);
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
            var buttons = state.GetButtons();
            return new TankControllerState(
                moveY: -state.Y,
                moveX: state.X,
                speedBoost: buttons[2],
                plantMine: buttons[0],
                fire: buttons[7]);
        }
    }
}
