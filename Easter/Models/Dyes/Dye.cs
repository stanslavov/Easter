using Easter.Models.Dyes.Contracts;

namespace Easter.Models.Dyes
{
    public class Dye : IDye
    {
        private int power;

        private const int USE_POWER_DECR = 10;

        public Dye(int power)
        {
            this.Power = power;
        }

        public int Power
        {
            get
            {
                return this.power;
            }
            protected set
            {
                this.power = value > 0 ? value : 0;
            }
        }

        public void Use()
        {
            this.Power -= USE_POWER_DECR;
        }

        public bool IsFinished()
        {
            return this.Power == 0;
        }
    }
}
