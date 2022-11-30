namespace EducationPortal.Infrastructure.DAL.Extensions
{
    public interface ISerializer
    {
        string Serialize(object obj);

        T Deserialze<T>(string json);
    }
}
