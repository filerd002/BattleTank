using System;
using System.Collections.Generic;
using System.Linq;
using SlimDX.DirectInput; 
namespace BattleTank.Core.Input
{

#if WINDOWS

    class GenericGamepadTankActionProvider : ITankActionProvider
    {

        private const int MAX_AXIS_VALUE = 5000;

        public int SpeedBoostButtonNumber { get; set; }
        public int PlantMineButtonNumber { get; set; }
        public int FireButtonNumber { get; set; }

        public Joystick _joystick { get; set; }

        public GenericGamepadTankActionProvider(int gamePadNumber, int speedBoostButtonNumber = 2, int plantMineButtonNumber = 0, int fireButtonNumber = 7)
        {
            SpeedBoostButtonNumber = speedBoostButtonNumber;
            PlantMineButtonNumber = plantMineButtonNumber;
            FireButtonNumber = fireButtonNumber;

            Initialize(gamePadNumber);
        }

        private bool Initialize(int pad)
        {

            DirectInput dinput = new DirectInput();
            List<DeviceInstance> devices = GetAllAvailableDeviceInstances();
            
            DeviceInstance device = devices.ElementAtOrDefault(pad);
            if (device is null) return false;

            _joystick = new SlimDX.DirectInput.Joystick(dinput, device.InstanceGuid);

            foreach (DeviceObjectInstance doi in _joystick.GetObjects(ObjectDeviceType.Axis))
            {
                _joystick.GetObjectPropertiesById((int)doi.ObjectType).SetRange(-MAX_AXIS_VALUE, MAX_AXIS_VALUE);
            }
            _joystick.Properties.AxisMode = DeviceAxisMode.Absolute;

            _joystick.Acquire();
            return true;
        }

           public bool IsConnectedTankController(){
            if (_joystick.Poll().IsSuccess)
                return true;
            else
                return false;
            }


        /// <inheritdoc />
        public TankControllerState GetTankControllerState()
        {
           JoystickState state = _joystick.GetCurrentState();

            //if (_joystick.Poll().IsFailure)
            //{                           
            //   throw new Exception("Nie udało się połączyć z joystickiem");
            //}
            //if (_joystick.GetCurrentState(ref state).IsFailure)
            //{
            //    throw new Exception("Nie udało się pobrać danych z joystika");
            //}

            bool[] buttons = state.GetButtons();
            return new TankControllerState(
                moveX: ((float)(Math.Abs(state.X) < 50 ? 0 : state.X) / MAX_AXIS_VALUE),
                moveY: ((float)-(Math.Abs(state.Y) < 50 ? 0 : state.Y) / MAX_AXIS_VALUE),
                speedBoost: buttons[SpeedBoostButtonNumber],
                plantMine: buttons[PlantMineButtonNumber],
                fire: buttons[FireButtonNumber]);
        }

        public static List<ITankActionProvider> GetAllAvailable()
        {
            var retVal = new List<ITankActionProvider>();
            for (int i = 0; i < HowManyAvailable(); i++)
            {
                retVal.Add(new GenericGamepadTankActionProvider(i));
            }
            return retVal;
        }

        public static int HowManyAvailable()
        {
            List<DeviceInstance> devices = GetAllAvailableDeviceInstances();
            return devices.Count;
        }

        private static List<DeviceInstance> GetAllAvailableDeviceInstances()
        {
            DirectInput dinput = new DirectInput();
            List<DeviceInstance> devices = dinput.GetDevices(DeviceClass.GameController, DeviceEnumerationFlags.AttachedOnly)
                .Where(d => !d.ProductName.Contains("XBOX")).ToList();
            return devices;
        }

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

#endif

}

