using Newtonsoft.Json;

namespace SellerCloud.Net.Http.Extensions
{
    public static class JsonHelper
    {
        private static bool IsJsonString(string input) => input.TrimStart()?.StartsWith(@"""") ?? false;
        private static bool IsJsonObject(string input) => input.TrimStart()?.StartsWith("{") ?? false;

        public static T Deserialize<T>(string input) => JsonConvert.DeserializeObject<T>(input);

        public static T TryDeserialize<T>(string input)
        {
            T result;

            bool isStringExpected = typeof(T) == typeof(string);
            bool isStringProvided = IsJsonString(input);

            // Special cases to avoid exception throwing
            if (isStringExpected && !isStringProvided)
            {
                result = default(T);
            }
            else if (isStringProvided && !isStringExpected)
            {
                result = default(T);
            }
            else
            {
                try
                {
                    result = Deserialize<T>(input);
                }
                catch (JsonReaderException)
                {
                    result = default(T);
                }
                catch (JsonSerializationException)
                {
                    result = default(T);
                }
            }

            return result;
        }
    }
}
