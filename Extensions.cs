using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MooresCloudDemos
{
    public static class Extensions
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
        public static Boolean MakeRequestToLight(Uri command)
        {
            var req = ((HttpWebRequest)WebRequest.Create(command));
            var resp = (HttpWebResponse)req.GetResponse();

            // Not sure if this is the correct response that the light will serve?
            return resp.StatusCode == HttpStatusCode.OK;
        }
    }
}
