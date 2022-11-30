namespace EducationPortal.Infrastructure.DAL.Extensions
{
    using System.IO;
    using Microsoft.Extensions.Logging;

    public class JsonReaderAndWriter
    {
        private readonly ILogger logger;

        public JsonReaderAndWriter(ILogger<JsonReaderAndWriter> logger)
        {
            this.logger = logger;
        }

        public string ReadJsonFromFile(string connectionString)
        {
            var json = string.Empty;
            try
            {
                json = File.ReadAllText(connectionString);
            }
            catch (FileNotFoundException ex)
            {
                this.logger.LogError(ex.Message);
            }

            return json;
        }

        public void WriteJsonToFile(string connectionString, string json)
        {
            try
            {
                File.WriteAllText(connectionString, json);
            }
            catch (FileNotFoundException ex)
            {
                this.logger.LogError(ex.Message);
            }
        }
    }
}
