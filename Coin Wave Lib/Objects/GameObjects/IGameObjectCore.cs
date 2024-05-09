namespace Coin_Wave_Lib.Objects.GameObjects
{
    internal interface IGameCore
    {
        (int x, int y) Index { get; set; }
        bool IsSolid { get; set; }
    }
}
