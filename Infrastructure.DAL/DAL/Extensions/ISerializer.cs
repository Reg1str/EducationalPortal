namespace EducationPortal.Infrastructure.JsonDAL.Extensions
{
    public interface ISerializer
    {
        string Serialize(object obj);

        T Deserialze<T>(string json);
    }
}
