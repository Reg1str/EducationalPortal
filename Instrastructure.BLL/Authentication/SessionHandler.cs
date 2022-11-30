namespace EducationPortal.Infrastructure.Authentication
{
    using System.IO;
    using EducationPortal.Domain.Core.Entities;
    using EducationPortal.Infrastructure.JsonDAL.Extensions;

    public class SessionHandler : ISessionHandler
    {
        private JsonReaderAndWriter jsonReaderAndWriter;
        private JsonSerializer jsonSerializer;

        public SessionHandler(JsonReaderAndWriter jsonReaderAndWriter, JsonSerializer serializer)
        {
            this.jsonReaderAndWriter = jsonReaderAndWriter;
            this.jsonSerializer = serializer;
        }

        public void CloseSession()
        {
            File.Delete("CurrentSession.json");
        }

        public void CreateUserSession(UserSession userAccount)
        {
            this.jsonReaderAndWriter.WriteJsonToFile("CurrentSession.json", this.jsonSerializer.Serialize(userAccount));
        }

        public UserSession GetCurrentSession()
        {
            return this.jsonSerializer.Deserialze<UserSession>(this.jsonReaderAndWriter.ReadJsonFromFile("CurrentSession.json"));
        }
    }
}
