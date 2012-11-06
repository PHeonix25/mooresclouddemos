using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MooresCloudDemos
{
    public class DisplayUvIndex
    {
        public String TranslateToColour(Double value)
        {
            if (value < 2.9) return "0xFF8080";
            if (value < 6.0) return "0xFFFF80";
            if (value < 8.0) return "0xBFFF80";
            if (value < 11.0) return "0x80FF80";
            return "0x80FFFF";
        }

        public void ConvertUvIndexToLight(Boolean debugging = false)
        {
            var feedUrl = new Uri("http://www.arpansa.gov.au/uvindex/realtime/xml/uvvalues.xml");
            var location = "SYD";
            var xPath = String.Format("//location[name='{0}']/index", location.ToLower());
            var colour = this.TranslateToColour(Extensions.GetValueFromXml<Double>(feedUrl, xPath));
            var result = String.Format("http://localhost/cgi-bin/ajaxcolor?color={0}", colour);

            if (Environment.UserInteractive)
                Console.WriteLine("Final result: " + result);

            if (debugging || Extensions.MakeRequestToLight(new Uri(result)) && Environment.UserInteractive)
                Console.WriteLine("Command submitted successfully.");

        }
    }
}
