namespace BattleTank.Core.Input
{
    public interface ITankActionProvider
    {
        TankControllerState GetTankControllerState();

        bool IsConnectedTankController();

    }
}
