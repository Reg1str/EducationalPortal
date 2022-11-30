namespace EducationPortal.Infrastructure.JsonDAL.Extensions
{
    using Newtonsoft.Json;

    public class JsonSerializer : ISerializer
    {
        public T Deserialze<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}
