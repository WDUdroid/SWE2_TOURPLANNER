namespace SWE2_TOURPLANNER.Model
{
    class ConfigHelper
    {
        public string DatabaseSource;
        public string ImageSource;

        public ConfigHelper(string databaseSource, string imageSource)
        {
            DatabaseSource = databaseSource;
            ImageSource = imageSource;
        }
    }
}
