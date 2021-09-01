using Easter.Models.Eggs.Contracts;
using Easter.Utilities.Messages;

using System;

namespace Easter.Models.Eggs
{
    public class Egg : IEgg
    {
        private const int COLOR_ENERGY_DECR = 10;

        private string name;
        private int energyRequired;

        public Egg(string name, int energyRequired)
        {
            this.Name = name;
            this.EnergyRequired = energyRequired;
        }

        public string Name 
        {
            get
            {
                return this.name;
            }
            private set
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException(ExceptionMessages.InvalidEggName);
                }

                this.name = value;
            }
        }

        public int EnergyRequired
        {
            get
            {
                return this.energyRequired;
            }
            private set
            {
                this.energyRequired = value > 0 ? value : 0;
            }
        }

        public void GetColored()
        {
            this.EnergyRequired -= COLOR_ENERGY_DECR;
        }

        public bool IsDone()
        {
            return this.EnergyRequired == 0;
        }
    }
}
