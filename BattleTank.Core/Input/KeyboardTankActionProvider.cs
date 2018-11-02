using Microsoft.Xna.Framework.Input;

namespace BattleTank.Core.Input
{
    public class KeyboardTankActionProvider : ITankActionProvider
    {
        public static KeyboardTankActionProvider DefaultPlayerOneKeybordLayout { get; } =  new KeyboardTankActionProvider(Keys.W, Keys.A, Keys.S, Keys.D, Keys.B, Keys.N, Keys.Space);
        public static KeyboardTankActionProvider DefaultPlayerTwoKeybordLayout { get; }= new KeyboardTankActionProvider(Keys.Up, Keys.Left, Keys.Down, Keys.Right, Keys.Decimal, Keys.NumPad1, Keys.NumPad0);
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
            KeyboardState keyboardState = Keyboard.GetState();

            return new TankControllerState(
                moveY: (float)((keyboardState.IsKeyDown(GoUp) ? 1 : 0) - (keyboardState.IsKeyDown(GoDown) ? 1 : 0)),
                moveX: (float)((keyboardState.IsKeyDown(GoRight) ? 1: 0) -  (keyboardState.IsKeyDown(GoLeft) ? 1 : 0)),
                speedBoost: keyboardState.IsKeyDown(SpeedBoost),
                plantMine: keyboardState.IsKeyDown(PlantMine),
                fire: keyboardState.IsKeyDown(Fire));
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (base.Equals(obj)) return true;
            if (!(obj is KeyboardTankActionProvider provider)) return false;

            return     provider.Fire == this.Fire
                    && provider.GoUp == this.GoUp
                    && provider.GoDown == this.GoDown 
                    && provider.GoLeft == this.GoLeft
                    && provider.GoRight == this.GoRight 
                    && provider.PlantMine == this.PlantMine
                    && provider.SpeedBoost == this.SpeedBoost;
        }

        public bool IsConnectedTankController()
        {
            return true;
        }
    }
}