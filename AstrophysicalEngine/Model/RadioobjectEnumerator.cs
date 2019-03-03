using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AstrophysicalEngine.Model
{
    public class RadioobjectEnumerator : IEnumerable<Radioobject>
    {
        private List<Radioobject> _objects { get; set; } = new List<Radioobject>();

        public async void DownloadObjectsList(Coordinates coords, int radius)
        {
            string query1400 = await Query(coords, 1400, radius);
            string query325 = await Query(coords, 325, radius);
            string[] obj1400 = (await HTMLParseLinkToObjects(query1400)).ToArray();
            string[] obj325 = (await HTMLParseLinkToObjects(query325)).ToArray();
            _objects = await ParseRadioobjects(obj325, obj1400);
        }

        public Radioobject this[int n]
        {
            get => _objects[n];
            set => _objects[n] = value;
        }

        public IEnumerator<Radioobject> GetEnumerator() => _objects.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _objects.GetEnumerator();

        private static async Task<string> Query(Coordinates coords, int frequency, int radius)
        {
            const string URL = "https://www.sao.ru/cats/cq";
            string postData = "";
            string[] source;

            postData = "ALPHA_MAX=" + (coords + new Coordinates(radius / 15, 0)).RAToString();
            postData += "&ALPHA_MIN=" + (coords - new Coordinates(radius / 15, 0)).RAToString();
            postData += ((frequency == 1400) ? "&CATALOGS=&0=r&1=NVSS" : "&CATALOGS=r");
            postData += "&CATS_SORT=on";
            postData += "&CATS_ZIP=of";
            postData += "&DELTA_MAX=" + (coords + new Coordinates(0, radius)).DecToString();
            postData += "&DELTA_MIN=" + (coords - new Coordinates(0, radius)).DecToString();
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

            source = await GetHTMLCode(URL, postData);

            foreach (string line in source)
            {
                if (line.Contains("<A HREF="))
                {
                    return "https://www.sao.ru/cats/" + line.Split('"')[1];
                }
            }

            throw new Exception("No link in the source.");
        }

        private static async Task<string[]> HTMLParseLinkToObjects(string link)
        {
            string[] output;
            int i;
            List<string> result = new List<string>();

            output = Encoding.ASCII.GetString(await (new WebClient()).DownloadDataTaskAsync(link)).Split('\n');

            for (i = 24; i < output.Length; i++)
            {
                if (output[i].IndexOf("-----") == -1)
                    result.Add(output[i].Replace('.', ','));
                else
                    break;
            }

            return result.ToArray();
        }

        public static async Task<List<Radioobject>> ParseRadioobjects(string[] objList325, string[] objList1400)
        {
            string[] obj1Params, obj2Params;
            string obj1, obj2;
            double currFlux325, currFlux1400;
            int i, j;
            Coordinates coords1, coords2;
            List<Radioobject> output = new List<Radioobject>();

            await Task.Run(() =>
            {
                for (i = 0; i < objList325.Length; i++)
                {
                    obj1 = objList325[i];
                    obj1Params = obj1.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    //Progress("Parsing radioobjects", i, objList325.Length);

                    try
                    {
                        coords1 = new Coordinates(obj1Params[2] + "+" + obj1Params[3] + "+" + obj1Params[4].Split(',')[0],
                            obj1Params[6] + "+" + obj1Params[7] + "+" + obj1Params[8].Split(',')[0]);
                    }
                    catch (FormatException) { continue; }

                    for (j = 0; j < objList1400.Length; j++)
                    {
                        obj2 = objList1400[j];
                        obj2Params = obj2.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                        try
                        {
                            coords2 = new Coordinates(obj2Params[2] + "+" + obj2Params[3] + "+" + obj2Params[4].Split(',')[0],
                                obj2Params[6] + "+" + obj2Params[7] + "+" + obj2Params[8].Split(',')[0]);
                        }
                        catch (FormatException) { continue; }

                        if (Coordinates.Distance(coords1, coords2) <= 15)
                        {
                            try
                            {
                                currFlux325 = double.Parse(obj1Params[11]);
                                currFlux1400 = double.Parse(obj2Params[11]);
                            }
                            catch (FormatException) { continue; }

                            output.Add(new Radioobject(coords1, currFlux325, currFlux1400));

                            break;
                        }
                    }
                }
            });

            return output;
        }
        
        private static async Task<string[]> GetHTMLCode(string url)
        {
            HttpWebRequest request = WebRequest.CreateHttp(url);
            HttpWebResponse response = (HttpWebResponse)(await request.GetResponseAsync());
            return (await (new StreamReader(response.GetResponseStream())).ReadToEndAsync()).Split('\n');
        }

        private static async Task<string[]> GetHTMLCode(string url, string postData)
        {
            byte[] data;
            string source;
            WebRequest request = WebRequest.Create(url);
            HttpWebResponse response;

            data = Encoding.ASCII.GetBytes(postData);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            using (Stream stream = request.GetRequestStream())
                stream.Write(data, 0, data.Length);
            response = (HttpWebResponse)(await request.GetResponseAsync());
            source = new StreamReader(response.GetResponseStream()).ReadToEnd();

            return source.Split('\n');
        }
    }
}
