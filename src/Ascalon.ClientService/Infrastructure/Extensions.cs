using Newtonsoft.Json;

namespace Ascalon.ClientService.Infrastructure
{
    public static class Extensions
    {
        public static T FromJson<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static string ToJson<T>(this T json)
        {
            return JsonConvert.SerializeObject(json);
        }
    }
}
