namespace MemoryCore.JsonModels
{
    public class Language
    {
        public string IETFTag { get; set; }
        public string UnlocalizedName { get; set; }
        public string UnlocalizedCountryName { get; set; }
        public string NativeName { get; set; }
        public string NativeCountryName { get; set; }
    }

    public class LanguagePair
    {
        public Language From { get; set; }
        public Language To { get; set; }
    }
}