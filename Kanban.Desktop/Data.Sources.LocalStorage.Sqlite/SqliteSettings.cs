namespace Data.Sources.LocalStorage.Sqlite
{
    public class SqliteSettings : IDataBaseSettings
    {
        public string BasePath { get; set; }

        public string GetConnectionString()
        {
            return $"Data Source = {BasePath}";
        }
    }
}
