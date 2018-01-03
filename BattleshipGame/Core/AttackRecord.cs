namespace BattleshipGame.Core
{
    public class AttackRecord : Location
    {
        public AttackRecord(string locationCode, bool isHit) : base(locationCode)
        {
            IsHit = isHit;
        }

        public bool IsHit { get; set; }
    }
}
