namespace Shootball.Model.Robot
{
    public class RobotStatistics
    {
        public float MaxLife { get; }
        public float LifeLeft { get; private set; }
        public int MaxShots { get; }
        public int ShotsLeft { get; private set; }

        public RobotStatistics(float maxLife, int maxShots)
        {
            MaxLife = maxLife;
            LifeLeft = MaxLife;
            MaxShots = maxShots;
            ShotsLeft = MaxShots;            
        }

        public bool GetDamaged(float amount)
        {
            var newLife = LifeLeft - amount;
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