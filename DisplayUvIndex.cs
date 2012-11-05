using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MooresCloudDemos
{
    public static class DisplayUvIndex
    {
        public static Boolean CanChangeType(String value, Type to)
        {
            try { return Convert.ChangeType(value, to) != null; }
            catch { return false; }
        }
        public static T GetValueFromXml<T>(Uri feed, String xPath) where T : IConvertible
        {
            using (var xml = XmlReader.Create(feed.ToString()))
            {
                var doc = new XmlDocument();
                doc.Load(xml);
                var loc = doc.SelectSingleNode(xPath);
                if (loc == null && Environment.UserInteractive)
                { Console.WriteLine("ERROR - That xPath value returned no results"); return default(T); }
                else if (!CanChangeType(loc.InnerText, typeof(T)) && Environment.UserInteractive)
                    Console.WriteLine("ERROR - The xPath returned a value, but failed at casting to the requested type.");

                return (T)Convert.ChangeType(loc.InnerText, typeof(T));
            }
        }

        public static String TranslateToColour(this Double value)
        {
            if (value < 2.9) return "0xFF8080";
            if (value < 6.0) return "0xFFFF80";
            if (value < 8.0) return "0xBFFF80";
            if (value < 11.0) return "0x80FF80";
            return "0x80FFFF";
        }

        public static void ConvertUvIndexToLight(string[] args)
        {
            var feedUrl = new Uri("http://www.arpansa.gov.au/uvindex/realtime/xml/uvvalues.xml");
            var location = "SYD";
            var xPath = String.Format("//location[name='{0}']/index", location.ToLower());
            var colour = GetValueFromXml<Double>(feedUrl, xPath).TranslateToColour();
            var result = String.Format("http://localhost/cgi-bin/ajaxcolor?color={0}", colour);

            if (Environment.UserInteractive)
                Console.WriteLine("Final result: " + result);

            ((HttpWebRequest)WebRequest.Create(result)).GetResponse(); // Make the request and change the colour
        }
    }
}
