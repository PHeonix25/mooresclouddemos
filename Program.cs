using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MooresCloudDemos
{
    class Program
    {
        static void Main(string[] args)
        {
            var debug = args.Contains("-debug");

            // First show the UV index
            new DisplayUvIndex().ConvertUvIndexToLight(debug);

            // Then show the Air Pollution rating
            new DisplayAirPollution().ConvertAirPollutionToLight(debug);

            if (Environment.UserInteractive)
            {
                Console.WriteLine("All operations complete. Please press [ENTER] to exit.");
                Console.ReadLine();
            }
        }
    }
}
