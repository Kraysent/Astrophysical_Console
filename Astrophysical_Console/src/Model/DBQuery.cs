using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Astrophysical_Console.Model
{
    static class DBQuery
    {
        public delegate void ProgressHandler(string process, int curr, int length);
        //public delegate void ProcessEndedHandler(string process);
        //public static event ProcessEndedHandler ProcessEnded;
        public static event ProgressHandler Progress;

        /// <summary>
        /// Gets the HTML code of page from www.sao.ru with specific coordinates
        /// </summary>
        /// <param name="coords"></param>
        /// <param name="frequency"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public static async Task<string[]> Query(Coordinates coords, int frequency, int radius)
        {
            const string URL = "https://www.sao.ru/cats/cq";
            string postData = "";

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
            
            return await GetHTMLCode(URL, postData);
        }

        /// <summary>
        /// Returns the list of radioobjects from the link parsed from HTML code
        /// </summary>
        /// <param name="source"></param>
        /// <param name="coords"></param>
        /// <returns></returns>
        public static async Task<List<string>> HTMLParseLinkToObjects(string[] source)
        {
            string link = "";
            string[] output;
            int i;
            List<string> result = new List<string>();

            foreach (string line in source)
            {
                if (line.Contains("<A HREF="))
                {
                    link = "https://www.sao.ru/cats/" + line.Split('"')[1];
                    break;
                }
            }
            
            output = Encoding.ASCII.GetString(await (new WebClient()).DownloadDataTaskAsync(link)).Split('\n');

            for (i = 24; i < output.Length; i++)
            {
                if (output[i].IndexOf("-----") == -1)
                    result.Add(output[i].Replace('.', ','));
                else
                    break;
            }

            return result;
        }

        /// <summary>
        /// Returns the list of Radioobjects from two lists
        /// </summary>
        /// <param name="objList325"></param>
        /// <param name="objList1400"></param>
        /// <returns></returns>
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

                    Progress("Parsing radioobjects", i, objList325.Length);

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
        
        /// <summary>
        /// Downloads pictures for all objects in the list
        /// </summary>
        public static async Task GetPicture(List<Radioobject> objects, string outputPath)
        {
            const string ProcessName = "Downloading pictures";

            Bitmap currPicture;
            string currPath;
            int i;
            
            for (i = 0; i < objects.Count; i++)
            {
                currPath = $"{outputPath}\\{objects[i].Coords.ToString()}.jpg";

                currPicture = await GetPicture(objects[i].Coords);
                if (currPicture == null)
                    continue;
                currPicture = await MinimizePicture(currPicture);
                currPicture.Save(currPath);
                Progress(ProcessName, i, objects.Count);
            }

            //ProcessEnded(ProcessName);
        }
        
        /// <summary>
        /// Returns the list of radioobjects with densityRatio parameter
        /// </summary>
        /// <param name="objects"></param>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public static async Task<List<Radioobject>> GetDensityRatio(List<Radioobject> objects, Coordinates centerCoords, int radius)
        {
            double areaDensity = await GetAverageAreaDensity(centerCoords, radius);
            int i;

            for (i = 0; i < objects.Count; i++)
            {
                Progress("Counting density ratio", i, objects.Count);
                objects[i].DensityRatio = (await GetObjectsDensity(objects[i].Coords, 15 * 60)) / areaDensity;
                objects[i].Redshift = await GetObjectsRedshift(objects[i].Coords);
            }

            return objects;
        }

        //-------------------------------------------------//

        /// <summary>
        /// Downloads single picture to the specific directory 
        /// </summary>
        /// <param name="coords"></param>
        /// <param name="outputPath"></param>
        private static async Task<Bitmap> GetPicture(Coordinates coords)
        {
            string url = "https://skyview.gsfc.nasa.gov/current/cgi/runquery.pl?Position=" + coords.RAToString() + "%2C+" + coords.DecToString() + "&survey=VLA+FIRST+(1.4+GHz)&coordinates=J2000&coordinates=" +
                "&projection=Tan&pixels=300&size=0.03&float=on&scaling=Log&resolver=SIMBAD-NED&Sampler=_skip_&Deedger=_skip_&rotation=&Smooth=" +
                "&lut=colortables%2Fb-w-linear.bin&PlotColor=&grid=_skip_&gridlabels=1&catalogurl=&CatalogIDs=on&survey=_skip_&survey=_skip_&survey=_skip_" +
                "&IOSmooth=&contour=&contourSmooth=&ebins=null";
            string[] source = await GetHTMLCode(url);
            string imgUrl;
            int i;
            Bitmap image = new Bitmap(10, 10);

            for (i = 0; i < source.Length; i++)
            {
                if (source[i].IndexOf("<img id=img1") != -1)
                {
                    if (source[i + 1].IndexOf("src") != -1)
                    {
                        imgUrl = "https://skyview.gsfc.nasa.gov/" + source[i + 1].Substring(source[i + 1].IndexOf("src") + 5, source[i + 1].Length -
                           source[i + 1].IndexOf("src") - 7);

                        try
                        {
                            using (WebClient client = new WebClient())
                            {
                                image = new Bitmap(new MemoryStream(await client.DownloadDataTaskAsync(imgUrl)));
                            }
                        }
                        catch (WebException) { }
                    }
                }
            }

            if (image.Size != new Size(10, 10))
                return image;
            else
                return null;
        }

        /// <summary>
        /// Minimizes single picture to 24x24 format
        /// </summary>
        /// <param name="picture"></param>
        /// <returns></returns>
        private static async Task<Bitmap> MinimizePicture(Bitmap img)
        {
            const int squareSide = 5;
            int i, j, currX, currY, squareNumber = img.Width / squareSide;
            Bitmap newImg = new Bitmap(squareNumber, squareNumber);

            await Task.Run(() =>
            {
                for (i = 0; i < squareNumber; i++)
                {
                    for (j = 0; j < squareNumber; j++)
                    {
                        currX = i * squareSide + squareSide / 2;
                        currY = j * squareSide + squareSide / 2;
                        Color currPixel = img.GetPixel(currX, currY);
                        newImg.SetPixel(i, j, currPixel);
                    }
                }
            });

            return newImg;
        }

        /// <summary>
        /// Returns average density of area near object
        /// </summary>
        /// <param name="coords"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        private static async Task<double> GetObjectsDensity(Coordinates coords, int radius)
        {
            string url = "https://ned.ipac.caltech.edu/cgi-bin/objsearch?in_csys=Equatorial&in_equinox=J2000.0&lon=" + coords.RAToString() + 
                "&lat=" + coords.DecToString() + "&radius=" + (radius / 60) + "&hconst=73&omegam=0.27&omegav=0.73&corr_z=1&z_constraint=Unconstrained" +
                "&z_value1=&z_value2=&z_unit=z&ot_include=ANY&nmp_op=ANY&out_csys=Equatorial&out_equinox=J2000.0&obj_sort=Distance+to+search+center" +
                "&of=ascii_tab&zv_breaker=30000.0&list_limit=5&img_stamp=YES&search_type=Near+Position+Search";
            string[] source = await GetHTMLCode(url);

            return source.Length / (Math.PI * radius * radius);
        }

        /// <summary>
        /// Returns redshift of object, if it is measured
        /// </summary>
        /// <param name="coords"></param>
        /// <returns></returns>
        private static async Task<double> GetObjectsRedshift(Coordinates coords)
        {
            const int radius = 1;
            string url = "https://ned.ipac.caltech.edu/cgi-bin/objsearch?search_type=Near+Position+Search&in_csys=Equatorial&in_equinox=J2000.0" +
                $"&lon={coords.RAToString()}&lat={coords.DecToString()}&radius={radius}.0&hconst=73&omegam=0.27&omegav=0.73&corr_z=1&" +
                $"z_constraint=Unconstrained&z_value1=&z_value2=&z_unit=z&ot_include=ANY&nmp_op=ANY&out_csys=Equatorial&out_equinox=J2000.0" +
                $"&obj_sort=Distance+to+search+center&of=ascii_bar&zv_breaker=30000.0&list_limit=5&img_stamp=YES";
            string[] source = await GetHTMLCode(url);
            int i;
            string[] currLine;

            try
            {
                for (i = 27; i < source.Length; i++)
                {
                    currLine = source[i].Split('|');

                    if (currLine[4] == "RadioS")
                        if (currLine[6].Trim(' ') != "")
                            return double.Parse(currLine[6].Trim(' ').Replace('.', ','));
                        else return 0;
                }

                return 0;
            }
            catch { return 0; }
        }

        /// <summary>
        /// Returns the average density of specific area
        /// </summary>
        /// <param name="coords"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        private static async Task<double> GetAverageAreaDensity(Coordinates coords, int radius)
        {
            const int NUMBER_OF_ITERATIONS = 50;
            Random rnd = new Random();
            int i;
            double averageDensity = 0;

            for (i = 0; i < NUMBER_OF_ITERATIONS; i++)
            {
                Progress("Counting area density", i, NUMBER_OF_ITERATIONS);
                averageDensity += await GetObjectsDensity(coords + new Coordinates(rnd.Next(radius), rnd.Next(radius)), 2 * 60);
            }

            averageDensity = averageDensity / NUMBER_OF_ITERATIONS;

            return averageDensity;
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
