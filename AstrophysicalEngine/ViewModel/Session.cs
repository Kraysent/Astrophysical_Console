using AstrophysicalEngine.Model;

namespace AstrophysicalEngine.ViewModel
{
    class Session
    {
        public RadioobjectEnumerator radioobjects;

        public Session(RadioobjectEnumerator radioobjects)
        {
            this.radioobjects = radioobjects;
        }
    }
}
