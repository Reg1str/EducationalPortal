namespace EducationPortal.Infrastructure.JsonDAL.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using EducationPortal.Domain.Services.Interfaces;
    using Newtonsoft.Json.Linq;

    public class JsonFormatter
    {
        private ISerializer serializer;

        public JsonFormatter(ISerializer serializer)
        {
            this.serializer = serializer;
        }

        public JObject CreateJsonMarkup(List<Type> types)
        {
            return new JObject(types.Select(x => new JProperty(x.Name + "s", new JArray())));
        }

        public JObject Format(Dictionary<Type, Dictionary<object, EntityState>> collections)
        {
            var resultingJObject = new JObject();
            foreach (var collection in collections)
            {
                var type = collection.Key;
                var entities = collection.Value;
                var jArray = new JArray();
                foreach (var entity in entities.Keys)
                {
                    jArray.Add(JObject.Parse(this.serializer.Serialize(entity)));
                }

                resultingJObject.Add(new JProperty(type.Name + "s", jArray));
            }

            return resultingJObject;
        }
    }
}
