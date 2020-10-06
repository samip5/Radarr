using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Nancy;
using Nancy.Responses.Negotiation;
using NLog;
using NzbDrone.Common.Instrumentation;
using NzbDrone.Common.Serializer;
using NzbDrone.Core.Messaging.Commands;

namespace Radarr.Http.Extensions
{
    public class NancyJsonSerializer : ISerializer
    {
        protected readonly JsonSerializerOptions _serializerSettings;
        private readonly Logger _logger;

        public NancyJsonSerializer()
        {
            _logger = NzbDroneLogger.GetLogger(typeof(NancyJsonSerializer));

            _serializerSettings = STJson.GetSerializerSettings();
            _serializerSettings.Converters.Add(new PolymorphicWriteOnlyJsonConverter<Command>());
        }

        public bool CanSerialize(MediaRange contentType)
        {
            return contentType == "application/json";
        }

        public void Serialize<TModel>(MediaRange contentType, TModel model, Stream outputStream)
        {
            STJson.Serialize(model, outputStream, _serializerSettings);
        }

        public IEnumerable<string> Extensions { get; private set; }
    }
}
