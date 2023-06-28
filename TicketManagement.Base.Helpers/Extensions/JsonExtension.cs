using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Dynamic;

namespace TicketManagement.Base.Helpers.Extensions;

public static class JsonExtension
{
    public static string StringifyJson(this Dictionary<string, string> json)
    {
        return JsonConvert.SerializeObject(json);
    }

    public static string StringifyJson(this List<Dictionary<string, string>> json)
    {
        return JsonConvert.SerializeObject(json);
    }

    public static string StringifyJson(this List<string> json)
    {
        return JsonConvert.SerializeObject(json);
    }

    public static dynamic ConvertToJson(this string jsonStr)
    {
        if (jsonStr is null) return null;

        var obj = JsonConvert.DeserializeObject(jsonStr);

        if (obj is string str) return str;

        return ((JToken)obj!).ConvertTokenToJson();
    }

    private static dynamic ConvertTokenToJson(this JToken token)
    {
        switch (token)
        {
            // FROM : http://blog.petegoo.com/archive/2009/10/27/using-json.net-to-eval-json-into-a-dynamic-variable-in.aspx
            // Ideally in the future Json.Net will support dynamic and this can be eliminated.
            case JValue value:
                return value.Value!;
            case JObject jObject:
                {
                    var expando = new ExpandoObject();
                    var list = jObject.OfType<JProperty>().ToList();

                    list
                        .ForEach(property =>
                        {
                            ((IDictionary<string, object>)expando!).Add(property.Name,
                                ConvertTokenToJson(property.Value));
                        });
                    return expando;
                }
            case JArray array:
                {
                    return array.Select(arrayItem => ConvertTokenToJson(arrayItem)).Select(dummy => (ExpandoObject)dummy)
                        .ToList();
                }
            default:
                throw new ArgumentException($"Unknown token type '{token.GetType()}'", nameof(token));
        }
    }
}