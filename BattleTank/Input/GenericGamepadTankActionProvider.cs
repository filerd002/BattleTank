﻿using System;
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

        public static GenericGamepadTankActionProvider DefaultGamepadProvider { get; } = new GenericGamepadTankActionProvider(2, 0, 7);

        public int SpeedBoostButtonNumber { get; set; }
        public int PlantMineButtonNumber { get; set; }
        public int FireButtonNumber { get; set; }

        private Joystick _joystick;

        public GenericGamepadTankActionProvider(int speedBoostButtonNumber, int plantMineButtonNumber, int fireButtonNumber)
        {
            SpeedBoostButtonNumber = speedBoostButtonNumber;
            PlantMineButtonNumber = plantMineButtonNumber;
            FireButtonNumber = fireButtonNumber;

            Initialize();
        }

        private bool Initialize()
        {
            DirectInput dinput = new DirectInput();
            DeviceInstance devices = dinput.GetDevices(DeviceClass.GameController, DeviceEnumerationFlags.AttachedOnly).FirstOrDefault();

            if (devices is null) return false;

            _joystick = new Joystick(dinput, devices.InstanceGuid);

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

        public static bool IsAnyAvailbableGamePad()
            => DefaultGamepadProvider.Initialize(); 

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