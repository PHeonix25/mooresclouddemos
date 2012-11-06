using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MooresCloudDemos
{
    class DisplayAirPollution
    {
        public String TranslateToColour(String value)
        {
            if (String.Equals(value, "High", StringComparison.CurrentCultureIgnoreCase))
                return "0xFFFF80";
            if (String.Equals(value, "Very High", StringComparison.CurrentCultureIgnoreCase))
                return "0x80FF80";
            if (String.Equals(value, "Low", StringComparison.CurrentCultureIgnoreCase))
                return "0xFF8080";

            return "0x808080"; // Should set unknown colour
        }

        /// <remarks>
        /// Needs a lot more debugging & error handling
        /// </remarks>
        public void ConvertAirPollutionToLight(Boolean debugging = false)
        {
            var feedUrl = new Uri("http://data.one.gov.hk/dataset/2/en");
            var location = "Central"; // Case sensitive
            var xPath = String.Format("//item[starts-with(title, '{0}')]/title", location);
            var title = Extensions.GetValueFromXml<String>(feedUrl, xPath);
            var code = title.Split(':').Skip(2).SingleOrDefault().Trim();
            var colour = this.TranslateToColour(code);
            var result = String.Format("http://localhost/cgi-bin/ajaxcolor?color={0}", colour);

            if (Environment.UserInteractive)
                Console.WriteLine("Final result: " + result);

            if (debugging || Extensions.MakeRequestToLight(new Uri(result)) && Environment.UserInteractive)
                Console.WriteLine("Command submitted successfully.");

        }
    }
}
