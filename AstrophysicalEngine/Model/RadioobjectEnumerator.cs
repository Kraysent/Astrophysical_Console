using System.Collections;
using System.Collections.Generic;

namespace AstrophysicalEngine.Model
{
    class RadioobjectEnumerator : IEnumerable<Radioobject>
    {
        public List<Radioobject> Objects { get; set; }

        public Radioobject this[int n]
        {
            get => Objects[n];
            set => Objects[n] = value;
        }

        public IEnumerator<Radioobject> GetEnumerator()
        {
            return Objects.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Objects.GetEnumerator();
        }
    }
}
