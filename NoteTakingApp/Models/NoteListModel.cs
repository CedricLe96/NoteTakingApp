using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace NoteTakingApp.Models
{
    // Interface für Model um Testbarkeit durch IoC zu vereinfachen.

    public interface INoteListModel
    {
        public int NoteCount { get; }
        public ICollection<INoteModel> Notes { get; }

        public void Add(int id, INoteModel note);
        public INoteModel Get(int id);
        public bool ContainsId(int id);
        public IDictionaryEnumerator GetEnumerator();
        public bool Remove(int id);
    }

    public class NoteListModel : INoteListModel
    {
        private readonly Dictionary<int, INoteModel> _noteListCollection;

        public int NoteCount => _noteListCollection.Count;

        public ICollection<INoteModel> Notes => _noteListCollection.Values;

        public NoteListModel()
        {
            _noteListCollection = new Dictionary<int, INoteModel>();
        }

        public INoteModel Get(int id)
        {
            return _noteListCollection[id];
        }

        public void Add(int id, INoteModel note)
        {
            _noteListCollection.Add(id, note);
        }

        public bool ContainsId(int id)
        {
            return _noteListCollection.ContainsKey(id);
        }

        public IDictionaryEnumerator GetEnumerator()
        {
            return _noteListCollection.GetEnumerator();
        }

        public bool Remove(int id)
        {
            return _noteListCollection.Remove(id);
        }
    }
}
