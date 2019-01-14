using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Astrophysical_Console
{
    static class CATSQuery
    {
        private const string URL = "https://www.sao.ru/cats/cq";

        /// <summary>
        /// Gets the HTML code of page from www.sao.ru with specific coordinates
        /// </summary>
        /// <param name="coords"></param>
        /// <param name="frequency"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public static string[] Query(Coordinates coords, int frequency, int radius)
        {
            WebRequest request = WebRequest.Create(URL);
            WebResponse response;
            string postData = "", source;
            byte[] data;

            postData = "ALPHA_MAX=" + (coords + new Coordinates(radius / 15, 0));
            postData += "&ALPHA_MIN=" + (coords - new Coordinates(radius / 15, 0));
            postData += ((frequency == 1400) ? "&CATALOGS=&0=r&1=NVSS" : "&CATALOGS=r");
            postData += "&CATS_SORT=on";
            postData += "&CATS_ZIP=of";
            postData += "&DELTA_MAX=" + (coords + new Coordinates(0, radius));
            postData += "&DELTA_MIN=" + (coords - new Coordinates(0, radius));
            postData += "&EPOCH=2000";
            postData += "&FLUX_MAX=";
            postData += "&FLUX_MIN=";
            postData += "&FREQ_MAX=" + frequency;
            postData += "&FREQ_MIN=" + frequency;
            postData += "&GLAT_MAX=";
            postData += "&GLAT_MIN=";
            postData += "&GLON_MAX=";
            postData += "&GLON_MIN=";
            postData += "&OUT_FORMAT=table";

            data = Encoding.ASCII.GetBytes(postData);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            using (Stream stream = request.GetRequestStream())
                stream.Write(data, 0, data.Length);
            response = (HttpWebResponse)request.GetResponse();
            source = new StreamReader(response.GetResponseStream()).ReadToEnd();

            return source.Split('\n');
        }
        
        /*public static string[] HTMLParseLinks(string[] source)
        {
            string link, fileName;

            foreach (string line in source)
            {
                if (line.Contains("<a href =\""))
                {
                    link = "https://www.sao.ru/cats/" + line.Substring(13, 20);
                }
            }


        }*/
    }
}
