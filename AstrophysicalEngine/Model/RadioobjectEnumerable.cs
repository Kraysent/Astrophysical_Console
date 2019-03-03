using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AstrophysicalEngine.Model
{
    public class RadioobjectEnumerable : IEnumerable<Radioobject>
    {
        private List<Radioobject> _objects { get; set; } = new List<Radioobject>();

        public int Count => _objects.Count;

        public event EventHandler<string> OnProcessBegin;
        public event EventHandler<string> OnProcessEnd;
        public event EventHandler<ProgressEventArgs> OnProcessProgressed;

        //-----------------------------------------------------------------------------//

        public void Add(Radioobject obj) => _objects.Add(obj);

        public void AddRange(IEnumerable<Radioobject> objs) => _objects.AddRange(objs);

        public void Clear() => _objects.Clear();

        public async Task DownloadObjectsListAsync(Coordinates coords, int radius)
        {
            ProcessBegin("Query to CATS on frequency 1400");
            string query1400 = await QueryAsync(coords, 1400, radius);
            ProcessEnd("Query to CATS on frequency 1400");
            ProcessBegin("Query to CATS on frequency 325");
            string query325 = await QueryAsync(coords, 325, radius);
            ProcessEnd("Query to CATS on frequency 325");
            ProcessBegin("Parsing link to file with objects, frequency 1400");
            string[] obj1400 = (await HTMLParseLinkToObjectsAsync(query1400)).ToArray();
            ProcessEnd("Parsing link to file with objects, frequency 1400");
            ProcessBegin("Parsing link to file with objects, frequency 325");
            string[] obj325 = (await HTMLParseLinkToObjectsAsync(query325)).ToArray();
            ProcessEnd("Parsing link to file with objects, frequency 325");
            ProcessBegin("Parsing radioobjects");
            _objects = await ParseRadioobjectsAsync(obj325, obj1400);
            ProcessEnd("Parsing radioobjects");
        }

        public async Task DownloadPicturesAsync(string outputPath)
        {
            Bitmap currPicture;
            string currPath;
            int i;

            outputPath += "\\Pictures\\";
            Directory.CreateDirectory(outputPath);

            for (i = 0; i < _objects.Count; i++)
            {
                ProcessProgressed(i, _objects.Count);

                currPath = $"{outputPath}\\{_objects[i].Coords.ToString()}.jpg";

                currPicture = await GetPictureAsync(_objects[i].Coords);
                if (currPicture == null)
                    continue;
                currPicture = await MinimizePictureAsync(currPicture);
                currPicture.Save(currPath);
            }
        }

        public async Task GetDensityRatioAsync(Coordinates centerCoords, int areaRadius)
        {
            ProcessBegin("Counting area density");
            double areaDensity = await GetAverageAreaDensityAsync(centerCoords, areaRadius);
            ProcessEnd("Counting area density");

            int i, radius = 15;

            for (i = 0; i < _objects.Count; i++)
            {
                ProcessProgressed(i, _objects.Count);

                string url = "https://ned.ipac.caltech.edu/cgi-bin/objsearch?search_type=Near+Position+Search&in_csys=Equatorial&in_equinox=J2000.0" +
                $"&lon={_objects[i].Coords.RAToString()}&lat={_objects[i].Coords.DecToString()}&radius={radius}&hconst=73&omegam=0.27&omegav=0.73&corr_z=1" +
                "&z_constraint=Unconstrained&z_value1=&z_value2=&z_unit=z&ot_include=ANY&nmp_op=ANY&out_csys=Equatorial&out_equinox=J2000.0" +
                "&obj_sort=Distance+to+search+center&of=ascii_bar&zv_breaker=30000.0&list_limit=5&img_stamp=YES";
                string[] source = await GetHTMLCodeAsync(url);

                _objects[i].DensityRatio = (source.Length - 27) / (Math.PI * radius * radius) / areaDensity;
                _objects[i].Redshift = GetObjectsRedshift(source);
            }
        }
        
        public Radioobject this[int n]
        {
            get => _objects[n];
            set => _objects[n] = value;
        }

        public IEnumerator<Radioobject> GetEnumerator() => _objects.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _objects.GetEnumerator();

        //-----------------------------------------------------------------------------//

        private async Task<string> QueryAsync(Coordinates coords, int frequency, int radius)
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

            source = await GetHTMLCodeAsync(URL, postData);

            foreach (string line in source)
            {
                if (line.Contains("<A HREF="))
                {
                    return "https://www.sao.ru/cats/" + line.Split('"')[1];
                }
            }

            throw new Exception("No link in the source.");
        }

        private async Task<string[]> HTMLParseLinkToObjectsAsync(string link)
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

        private async Task<List<Radioobject>> ParseRadioobjectsAsync(string[] objList325, string[] objList1400)
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
                    ProcessProgressed(i, objList325.Length);

                    obj1 = objList325[i];
                    obj1Params = obj1.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

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

        private async Task<Bitmap> GetPictureAsync(Coordinates coords)
        {
            string url = "https://skyview.gsfc.nasa.gov/current/cgi/runquery.pl?Position=" + coords.RAToString() + "%2C+" + coords.DecToString() + "&survey=VLA+FIRST+(1.4+GHz)&coordinates=J2000&coordinates=" +
                "&projection=Tan&pixels=300&size=0.03&float=on&scaling=Log&resolver=SIMBAD-NED&Sampler=_skip_&Deedger=_skip_&rotation=&Smooth=" +
                "&lut=colortables%2Fb-w-linear.bin&PlotColor=&grid=_skip_&gridlabels=1&catalogurl=&CatalogIDs=on&survey=_skip_&survey=_skip_&survey=_skip_" +
                "&IOSmooth=&contour=&contourSmooth=&ebins=null";
            string[] source = await GetHTMLCodeAsync(url);
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

        private async Task<Bitmap> MinimizePictureAsync(Bitmap img)
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

        private async Task<double> GetAverageAreaDensityAsync(Coordinates coords, int radius)
        {
            const int NUMBER_OF_ITERATIONS = 50;
            Random rnd = new Random();
            int i;
            double averageDensity = 0;

            for (i = 0; i < NUMBER_OF_ITERATIONS; i++)
            {
                ProcessProgressed(i, NUMBER_OF_ITERATIONS);

                averageDensity += await GetObjectsDensityAsync(coords + new Coordinates(rnd.Next(radius), rnd.Next(radius)), 2);
            }

            averageDensity = averageDensity / NUMBER_OF_ITERATIONS;

            return averageDensity;
        }

        private double GetObjectsRedshift(string[] source)
        {
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

        private async Task<double> GetObjectsDensityAsync(Coordinates coords, int radius)
        {
            string url = "https://ned.ipac.caltech.edu/cgi-bin/objsearch?search_type=Near+Position+Search&in_csys=Equatorial&in_equinox=J2000.0" +
                $"&lon={coords.RAToString()}&lat={coords.DecToString()}&radius={radius}&hconst=73&omegam=0.27&omegav=0.73&corr_z=1" +
                "&z_constraint=Unconstrained&z_value1=&z_value2=&z_unit=z&ot_include=ANY&nmp_op=ANY&out_csys=Equatorial&out_equinox=J2000.0" +
                "&obj_sort=Distance+to+search+center&of=ascii_bar&zv_breaker=30000.0&list_limit=5&img_stamp=YES";
            string[] source = await GetHTMLCodeAsync(url);

            return (source.Length - 27) / (Math.PI * radius * radius);
        }
        
        private async Task<string[]> GetHTMLCodeAsync(string url)
        {
            HttpWebRequest request = WebRequest.CreateHttp(url);
            HttpWebResponse response = (HttpWebResponse)(await request.GetResponseAsync());
            return (await (new StreamReader(response.GetResponseStream())).ReadToEndAsync()).Split('\n');
        }

        private async Task<string[]> GetHTMLCodeAsync(string url, string postData)
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

        private void ProcessBegin(string processName) => OnProcessBegin?.Invoke(this, processName);

        private void ProcessEnd(string processName) => OnProcessEnd?.Invoke(this, processName);

        private void ProcessProgressed(int current, int maximum) => OnProcessProgressed?.Invoke(this, new ProgressEventArgs(current, maximum));
    }
}
