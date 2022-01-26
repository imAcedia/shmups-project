using Action = System.Action;

namespace Shmup
{
    public interface IShipInput
    {
        public event Action OnShootDown;
        public event Action OnBombDown;

        ShipInputState GetInputState(Ship ship);
    }
}
