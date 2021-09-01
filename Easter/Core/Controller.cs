using Easter.Core.Contracts;
using Easter.Models.Bunnies;
using Easter.Models.Bunnies.Contracts;
using Easter.Models.Dyes;
using Easter.Models.Dyes.Contracts;
using Easter.Models.Eggs;
using Easter.Models.Eggs.Contracts;
using Easter.Models.Workshops;
using Easter.Repositories;
using Easter.Utilities.Messages;

using System;
using System.Linq;
using System.Text;

namespace Easter.Core
{
    public class Controller : IController
    {
        private BunnyRepository bunnies;
        private EggRepository eggs;

        private const int BUNNY_MIN_COLOR_ENERGY = 50;

        public Controller()
        {
            this.bunnies = new BunnyRepository();
            this.eggs = new EggRepository();
        }

        public string AddBunny(string bunnyType, string bunnyName)
        {
            IBunny bunny = null;

            if (bunnyType == "HappyBunny")
            {
                bunny = new HappyBunny(bunnyName);
            }
            else if (bunnyType == "SleepyBunny")
            {
                bunny = new SleepyBunny(bunnyName);
            }

            if (bunny == null)
            {
                throw new InvalidOperationException(ExceptionMessages.InvalidBunnyType);
            }

            this.bunnies.Add(bunny);

            return String.Format(OutputMessages.BunnyAdded, bunnyType, bunnyName);
        }

        public string AddDyeToBunny(string bunnyName, int power)
        {
            IBunny bunny = this.bunnies.FindByName(bunnyName);

            if (bunny == null)
            {
                throw new InvalidOperationException(ExceptionMessages.InexistentBunny);
            }

            IDye dye = new Dye(power);
            bunny.AddDye(dye);

            return String.Format(OutputMessages.DyeAdded, power, bunnyName);
        }

        public string AddEgg(string eggName, int energyRequired)
        {
            IEgg egg = new Egg(eggName, energyRequired);
            this.eggs.Add(egg);

            return String.Format(OutputMessages.EggAdded, eggName);
        }

        public string ColorEgg(string eggName)
        {
            Workshop workshop = new Workshop();

            IEgg egg = this.eggs.FindByName(eggName);

            var bunnies = this.bunnies.Models
                .Where(x => x.Energy >= BUNNY_MIN_COLOR_ENERGY)
                .OrderByDescending(x => x.Energy)
                .ToList();

            if (!bunnies.Any())
            {
                throw new InvalidOperationException(ExceptionMessages.BunniesNotReady);
            }

            while (bunnies.Any())
            {
                IBunny currentBunny = bunnies.First();

                workshop.Color(egg, currentBunny);

                if (!currentBunny.Dyes.Any())
                {
                    bunnies.Remove(currentBunny);
                }

                if (currentBunny.Energy == 0)
                {
                    bunnies.Remove(currentBunny);
                    this.bunnies.Remove(currentBunny);
                }

                if (egg.IsDone())
                {
                    break;
                }
            }

            return String.Format(egg.IsDone() ?
                OutputMessages.EggIsDone :
                OutputMessages.EggIsNotDone, eggName);
        }

        public string Report()
        {
            int eggsColored = this.eggs
                .Models
                .Count(p => p.IsDone());

            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"{eggsColored} eggs are done!");
            sb.AppendLine($"Bunnies info:");

            foreach (var bunny in this.bunnies.Models)
            {
                int countDyes = bunny.Dyes.Count(x => !x.IsFinished());

                sb
                    .AppendLine($"Name: {bunny.Name}")
                    .AppendLine($"Energy: {bunny.Energy}")
                    .AppendLine($"Dyes: {countDyes} not finished");
            }

            return sb.ToString().TrimEnd();
        }
    }
}
