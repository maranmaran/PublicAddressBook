using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Domain.DeserializatorPOCOs;
using Backend.Domain.Entities.Auditing;
using Backend.Domain.Entities.Contacts;
using Backend.Infrastructure.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Backend.Infrastructure.Services
{
    public class ActivityService : IActivityService
    {
        private readonly JsonSerializerSettings _serializerSettings;

        public ActivityService()
        {
            _serializerSettings = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = new List<JsonConverter>()
                {
                    new StringEnumConverter()
                }
            };
        }

        public async Task<string> GetEntityAsJson(AuditRecord audit)
        {
            return audit.EntityType switch
            {
                nameof(Contact) => JsonConvert.SerializeObject(await DeserializeContact(audit), _serializerSettings),
                _ => throw new ArgumentException($"Entity type is not recognized. {audit.EntityType}"),
            };
        }


        private async Task<ContactDeserializeResult> DeserializeContact(AuditRecord audit)
        {
            return await Task.FromResult(audit.GetData<ContactDeserializeResult>().Entity);
        }

    }
}
