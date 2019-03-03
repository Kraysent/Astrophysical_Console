using AstrophysicalEngine.Model;
using System.IO;

namespace AstrophysicalEngine.ViewModel
{
    public class Session
    {
        public RadioobjectEnumerator radioobjects;
        public string OutputPath;

        public Session(RadioobjectEnumerator radioobjects, string outputPath)
        {
            this.radioobjects = radioobjects;
            OutputPath = outputPath;
        }

        public Session(RadioobjectEnumerator radioobjects) : this(radioobjects, Directory.GetCurrentDirectory() + "\\")
        {
            
        }

        public Session()
        {
            radioobjects = new RadioobjectEnumerator();
        }
    }
}
