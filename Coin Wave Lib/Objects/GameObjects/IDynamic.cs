namespace Coin_Wave_Lib.Objects.GameObjects
{
    public interface IDynamic
    {
        void MoveInOneFrame(GameObject gameObject);
        void SetSpeed(int frameTime);
    }
}
