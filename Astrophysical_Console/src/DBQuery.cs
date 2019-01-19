using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Astrophysical_Console
{
    static class DBQuery
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
        
        /// <summary>
        /// Returns the list of radioobjects from the link parsed from HTML code
        /// </summary>
        /// <param name="source"></param>
        /// <param name="coords"></param>
        /// <returns></returns>
        public static IEnumerable<string> HTMLParseLinkToObjects(string[] source)
        {
            string link = "";
            string[] output;
            int i;

            foreach (string line in source)
            {
                if (line.Contains("<a href =\""))
                    link = "https://www.sao.ru/cats/" + line.Substring(13, 20);
            }

            output = Encoding.ASCII.GetString((new WebClient()).DownloadData(link)).Split('\n');

            for (i = 24; i < output.Length; i++)
            {
                if (output[i].IndexOf("-----") == -1)
                    yield return output[i];
                else
                    break;
            }
        }

        /// <summary>
        /// Returns the list of Radioobjects from two lists
        /// </summary>
        /// <param name="objList325"></param>
        /// <param name="objList1400"></param>
        /// <returns></returns>
        public static IEnumerable<Radioobject> ParseRadioobjects(string[] objList325, string[] objList1400)
        {
            foreach (string obj1 in objList325)
            {
                string[] obj1Params = obj1.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                Coordinates coords1 = new Coordinates(obj1Params[2] + "+" + obj1Params[3] + "+" + obj1Params[4].Split('.')[0], 
                    obj1Params[6] + "+" + obj1Params[7] + "+" + obj1Params[8].Split('.')[0]);

                foreach (string obj2 in objList1400)
                {
                    string[] obj2Params = obj2.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    Coordinates coords2 = new Coordinates(obj2Params[2] + "+" + obj2Params[3] + "+" + obj2Params[4].Split('.')[0],
                        obj2Params[6] + "+" + obj2Params[7] + "+" + obj2Params[8].Split('.')[0]);

                    if (Coordinates.Distance(coords1, coords2) < 15)
                    {
                        yield return new Radioobject(obj1Params[0], obj1Params[1], coords1, double.Parse(obj1Params[11]), double.Parse(obj2Params[11]));
                    }

                    break;
                }
            }
        }

        /// <summary>
        /// Downloads single picture to the specific directory
        /// </summary>
        /// <param name="coords"></param>
        /// <param name="outputPath"></param>
        public static void GetPicture(Coordinates coords, string outputPath)
        {
            string url = "https://skyview.gsfc.nasa.gov/current/cgi/runquery.pl?Position=" + coords.ToString() + "&survey=NVSS&coordinates=J2000&coordinates=" +
                "&projection=Tan&pixels=300&size=0.1&float=on&scaling=Log&resolver=SIMBAD-NED&Sampler=_skip_&Deedger=_skip_&rotation=&Smooth=" +
                "&lut=colortables%2Fb-w-linear.bin&PlotColor=&grid=_skip_&gridlabels=1&catalogurl=&CatalogIDs=on&survey=_skip_&survey=_skip_&survey=_skip_" +
                "&IOSmooth=&contour=&contourSmooth=&ebins=null";
            HttpWebRequest request = WebRequest.CreateHttp(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string[] source = (new StreamReader(response.GetResponseStream())).ReadToEnd().Split('\n');
            string imgUrl;
            int i;

            for (i = 0; i < source.Length; i++)
            {
                if (source[i].IndexOf("img id") != -1)
                {
                    if (source[i + 1].IndexOf("src") != -1)
                    {
                        imgUrl = "https://skyview.gsfc.nasa.gov/" + source[i + 1].Substring(source[i + 1].IndexOf("src") + 5, source[i + 1].Length -
                           source[i + 1].IndexOf("src") - 7);

                        try
                        {
                            using (WebClient client = new WebClient())
                            {
                                Directory.CreateDirectory(outputPath + @"\Pictures\");

                                client.DownloadFile(imgUrl, outputPath + @"\Pictures\" + coords.ToString());
                            }
                        }
                        catch (WebException)
                        {
                            Console.WriteLine("Caught WebException on coordinates {0}", coords.ToString());
                        }
                    }
                }
            }

        }

        /// <summary>
        /// Downloads pictures for all objects in the list
        /// </summary>
        public static void GetPicture(IEnumerable<Radioobject> objects, string outputPath)
        {
            foreach (Radioobject obj in objects)
            {
                GetPicture(obj.Coords, outputPath)
            }
        }
    }
}
