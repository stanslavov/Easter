namespace Easter.Models.Bunnies
{
    public class SleepyBunny : Bunny
    {
        private const int INIT_ENERGY = 50;
        private const int WORK_ENERGY_DECR = 5;

        public SleepyBunny(string name) 
            : base(name, INIT_ENERGY)
        {

        }

        public override void Work()
        {
            base.Work();
            this.Energy -= WORK_ENERGY_DECR;
        }
    }
}
