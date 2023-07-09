namespace NoteTakingApp.Models
{
    // Factory Pattern, um IoC für NoteModel aufrecht zu halten.
    // Nicht ideal, dass Konstruktorparameter hier fest definiert sind, aber besser für Testbarkeit der Modelle.
    public interface INoteModelFactory
    {
        public INoteModel Create(int id, string content);
    }

    public class NoteModelFactory : INoteModelFactory
    {
        public INoteModel Create(int id, string content)
        {
            return new NoteModel(id, content);
        }
    }
}
