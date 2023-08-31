using Odin.Auth.Infra.Messaging.Extensions;
using System.Text.Json;

namespace Odin.Auth.Infra.Messaging.Policies
{
    public class JsonSnakeCasePolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name)
            => name.ToSnakeCase();
    }
}
