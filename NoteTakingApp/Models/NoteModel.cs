namespace NoteTakingApp.Models
{
    public interface INoteModel
    {
        public int Id { get; }
        public string Content { get; set; }
    }

    public class NoteModel : INoteModel
    {
        public int Id { get; private set; }
        public string Content { get; set; }

        public NoteModel(int id, string content)
        {
            Id = id;
            Content = content;
        }
    }
}
