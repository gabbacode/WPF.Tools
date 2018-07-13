namespace Data.Sources.LocalStorage.Sqlite
{
    public interface IDataBaseSettings
    {
        string BasePath { get; set; }
        string GetConnectionString();
    }
}
