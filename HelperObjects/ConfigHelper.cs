namespace SWE2_TOURPLANNER.HelperObjects
{
    class ConfigHelper
    {
        public string DatabaseSource;
        public string ImageSource;

        public ConfigHelper(string _databaseSource, string _imageSource)
        {
            DatabaseSource = _databaseSource;
            ImageSource = _imageSource;
        }
    }
}
