namespace Shootball.Model.Robot
{
    public class RobotStatistics
    {
        public readonly float MaxLife;
        public readonly int MaxShots;

        public float LifeLeft { get; private set; }
        public int ShotsLeft { get; private set; }
        public int Points { get; private set; }

        public RobotStatistics(float maxLife, int maxShots)
        {
            MaxLife = maxLife;
            MaxShots = maxShots;
            LifeLeft = MaxLife;
            ShotsLeft = MaxShots;
            Points = 0;
        }

        public bool GetDamaged()
        {
            var newLife = LifeLeft - 10;
            if (newLife > 0)
            {
                LifeLeft = newLife;
                return true;
            }
            else
            {
                LifeLeft = 0;
                return false;
            }
        }

        public void IncreasePoints()
        {
            Points += 50;
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