using Radarr.Http.REST;

namespace Radarr.Api.V3.Profiles.Languages
{
    public class LanguageResource : RestResource
    {
        public new int Id { get; set; }
        public string Name { get; set; }
        public string NameLower => Name.ToLowerInvariant();
    }
}
