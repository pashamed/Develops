namespace Develops.Config
{
    internal class AppSettings
    {
        public required string DatabaseConnectionString { get; set; }
        public required string CsvFilePath { get; set; }
        public required string DuplicatesFilePath { get; set; }
        public required string TimeZone { get; set; }
        public required string SqlScriptFilePath { get; set; }
    }
}