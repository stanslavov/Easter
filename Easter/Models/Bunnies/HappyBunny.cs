namespace Easter.Models.Bunnies
{
    public class HappyBunny : Bunny
    {
        private const int INIT_ENERGY = 100;

        public HappyBunny(string name) 
            : base(name, INIT_ENERGY)
        {

        }
    }
}
