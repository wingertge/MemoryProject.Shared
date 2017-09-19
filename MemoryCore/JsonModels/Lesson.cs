namespace MemoryCore.JsonModels
{
    public class Lesson
    {
        public string Id { get; set; }
        public string From { get; set; }
        public string Reading { get; set; }
        public string To { get; set; }
        public LanguagePair Languages { get; set; }
    }
}