using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BattleTank.Tanks
{
    public interface ITankActionProvider
    {
        TankControllerState GetTankControllerState();
    }

    public class KeyboardTankActionProvider : ITankActionProvider
    {
        public Keys GoUp { get; }
        public Keys GoLeft { get; }
        public Keys GoDown { get; }
        public Keys GoRight { get; }
        public Keys SpeedBoost { get; }
        public Keys PlantMine { get; }
        public Keys Fire { get; }

        public KeyboardTankActionProvider(Keys goUp, Keys goLeft, Keys goDown, Keys goRight, Keys speedBoost, Keys plantMine, Keys fire)
        {
            GoUp = goUp;
            GoLeft = goLeft;
            GoDown = goDown;
            GoRight = goRight;
            SpeedBoost = speedBoost;
            PlantMine = plantMine;
            Fire = fire;
        }
        /// <inheritdoc />
        public TankControllerState GetTankControllerState()
        {
            // TODO: tego 5000 nie może byc tutaj na sztywno!
            KeyboardState keyboardState = Keyboard.GetState();

            return new TankControllerState(
                moveY: (keyboardState.IsKeyDown(GoUp) ? 5000 : 0) - (keyboardState.IsKeyDown(GoDown) ? 5000 : 0),
                moveX: (keyboardState.IsKeyDown(GoRight) ? 5000 : 0) -  (keyboardState.IsKeyDown(GoLeft) ? 5000 : 0),
                speedBoost: keyboardState.IsKeyDown(SpeedBoost),
                plantMine: keyboardState.IsKeyDown(PlantMine),
                fire: keyboardState.IsKeyDown(Fire));
        }
    }
}
