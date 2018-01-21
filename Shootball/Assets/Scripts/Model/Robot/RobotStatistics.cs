namespace Shootball.Model.Robot
{
    public class RobotStatistics
    {
        public readonly float MaxLife;
        public readonly int MaxShots;

        public float LifeLeft { get; private set; }
        public int ShotsLeft { get; private set; }
        public int Points { get; private set; }
        public bool IsAlive => LifeLeft > 0;

        public RobotStatistics(float maxLife, int maxShots)
        {
            MaxLife = maxLife;
            MaxShots = maxShots;
            LifeLeft = MaxLife;
            ShotsLeft = MaxShots;
            Points = 0;
        }

        public bool GetDamaged(float effectiveness)
        {
            if (IsAlive)
            {
                if (effectiveness < 0) effectiveness = 0;
                else if (effectiveness > 1) effectiveness = 1;
                LifeLeft = LifeLeft - 10 * effectiveness;
            }

            return IsAlive;
        }

        public void IncreasePoints(float effectiveness)
        {
            if (effectiveness < 0) effectiveness = 0;
            else if (effectiveness > 1) effectiveness = 1;
            Points += (int)(50 * effectiveness);
        }

        public bool Shoot()
        {
            if (ShotsLeft <= 0)
            {
                return false;
            }

            ShotsLeft--;
            return true;
        }

        public void RechargeShot()
        {
            if (ShotsLeft < MaxShots) { ShotsLeft++; }
        }
    }
}