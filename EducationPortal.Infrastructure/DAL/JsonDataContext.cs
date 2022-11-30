namespace EducationPortal.Infrastructure.DAL
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using EducationPortal.Domain.Core.Entities;
    using EducationPortal.Domain.Services.Interfaces;
    using EducationPortal.Infrastructure.DAL.Extensions;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json.Linq;

    public class JsonDataContext
    {
        private string connectionString;
        private ILogger logger;
        private ISerializer serializer;
        private Dictionary<Type, Dictionary<object, EntityState>> entities;
        private JsonReaderAndWriter jsonReaderAndWriter;
        private JsonFormatter jsonFormatter;

        public JsonDataContext(string connection, ILogger<JsonDataContext> logger, ISerializer serializer,
            JsonReaderAndWriter jsonReaderAndWriter, JsonFormatter jsonFormatter)
        {
            this.connectionString = connection;
            this.logger = logger;
            this.serializer = serializer;
            this.jsonReaderAndWriter = jsonReaderAndWriter;
            this.jsonFormatter = jsonFormatter;
            this.entities = new Dictionary<Type, Dictionary<object, EntityState>>();
        }

        public T Find<T>(int id)
            where T : BaseEntity
        {
            return this.FindInContext<T>(id) ?? this.FindInDatabase<T>(id);
        }

        public void Add<T>(T entity)
            where T : BaseEntity
        {
            this.SetState(entity, EntityState.Added);
        }

        public void Remove<T>(T entity)
            where T : BaseEntity
        {
            this.SetState(entity, EntityState.Removed);
        }

        public void Update<T>(T entity)
            where T : BaseEntity
        {
            this.SetState(entity, EntityState.Modified);
        }

        public IEnumerable<T> FindBySpecification<T>(Func<T, bool> specification)
            where T : BaseEntity
        {
            this.CheckCategoryCreated<T>();
            var contextEntitiesSearchResult = this.entities[typeof(T)].Keys.Select(key => (T)key).Where(specification);
            if (contextEntitiesSearchResult.Any())
            {
                return contextEntitiesSearchResult;
            }

            var rawJsonData = this.jsonReaderAndWriter.ReadJsonFromFile(this.connectionString);
            var jObject = JObject.Parse(rawJsonData);
            var children = jObject[typeof(T).Name + "s"];
            return jObject[typeof(T).Name + "s"].Children()
                                                .Select(x => (T)x.ToObject(typeof(T)))
                                                .Where(specification);
        }

        public void SaveChanges()
        {
            try
            {
                var rawJsonData = this.jsonReaderAndWriter.ReadJsonFromFile(this.connectionString);
                var jObjectData = new JObject();
                var jObjectContext = this.jsonFormatter.Format(this.entities);
                if (!string.IsNullOrEmpty(rawJsonData))
                {
                    jObjectData = JObject.Parse(rawJsonData);
                }
                else
                {
                    jObjectData = this.jsonFormatter.CreateJsonMarkup(this.entities.Keys.ToList());
                }

                foreach (var collection in this.entities)
                {
                    var type = collection.Key;
                    var array = (JArray)jObjectData[type.Name + "s"];
                    var dbSet = collection.Value;
                    foreach (var pair in dbSet)
                    {
                        var entityId = pair.Key.GetType().GetProperty("Id").GetValue(pair.Key) as int?;
                        if (entityId.HasValue)
                        {
                            switch (pair.Value)
                            {
                                case EntityState.Added:
                                    if (this.FindInDatabase(entityId.Value, type) == null)
                                    {
                                        array.Add(JObject.Parse(this.serializer.Serialize(pair.Key)));
                                    }

                                    break;
                                case EntityState.Modified:
                                    var toModify = array.FirstOrDefault(obj => obj["Id"].Value<int>() == entityId.Value);
                                    if (toModify != null)
                                    {
                                        array.Remove(toModify);
                                        array.Add(JObject.Parse(this.serializer.Serialize(pair.Key)));
                                    }

                                    break;
                                case EntityState.Removed:
                                    var toDelete = array.FirstOrDefault(obj => obj["Id"].Value<int>() == entityId.Value);
                                    if (toDelete != null)
                                    {
                                        array.Remove(toDelete);
                                    }

                                    break;
                                default:
                                    break;
                            }

                            jObjectData[type.Name + "s"] = array;
                        }
                    }
                }

                this.jsonReaderAndWriter.WriteJsonToFile(this.connectionString, this.serializer.Serialize(jObjectData));
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
            }
        }

        private void SetState<T>(T entity, EntityState state)
            where T : BaseEntity
        {
            this.CheckCategoryCreated<T>();
            if (this.Find<T>(entity.Id) == null)
            {
                this.entities[typeof(T)].Add(entity, state);
            }
            else
            {
                this.entities[typeof(T)][entity] = state;
            }
        }

        private T FindInContext<T>(int id)
            where T : BaseEntity
        {
            if (this.entities.ContainsKey(typeof(T)))
            {
                return this.entities[typeof(T)].Cast<T>()
                                        .Where(entity => entity.Id == id)
                                        .FirstOrDefault();
            }

            return default;
        }

        private T FindInDatabase<T>(int id)
            where T : BaseEntity
        {
            var rawJsonData = this.jsonReaderAndWriter.ReadJsonFromFile(this.connectionString);
            if (string.IsNullOrEmpty(rawJsonData))
            {
                return default;
            }

            var jObject = JObject.Parse(rawJsonData);
            return jObject[typeof(T).Name + "s"].Children()
                                                .Select(x => (T)x.ToObject(typeof(T)))
                                                .Where(x => x.Id == id)
                                                .FirstOrDefault();
        }

        private object FindInDatabase(int id, Type type)
        {
            var rawJsonData = this.jsonReaderAndWriter.ReadJsonFromFile(this.connectionString);
            if (string.IsNullOrEmpty(rawJsonData))
            {
                return default;
            }

            var jObject = JObject.Parse(rawJsonData);
            return jObject[type.Name + "s"].Children()
                                           .Select(x => x.ToObject(type))
                                           .Where(x => (x.GetType().GetProperty("Id").GetValue(x) as int?).Value == id)
                                           .FirstOrDefault();
        }

        private void CheckCategoryCreated<T>()
        {
            if (!this.entities.ContainsKey(typeof(T)))
            {
                this.entities.Add(typeof(T), new Dictionary<object, EntityState>());
            }
        }
    }
}
