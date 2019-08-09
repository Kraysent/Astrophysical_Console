using AstrophysicalEngine.Model;

namespace AstrophysicalEngine.ViewModel
{
    public interface IImportManager
    {
        Radioobject ProcessObject(string line);
    }
}
