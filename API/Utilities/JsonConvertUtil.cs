using Newtonsoft.Json;
using System.Diagnostics;

namespace WebApplication2.Utilities
{
    public static class JsonConvertUtil
    {
        //TODO: make these functions async

        /// <summary>
        /// Desirializes a JSON file to a desirable type
        /// </summary>
        /// <typeparam name="T">A desirable type</typeparam>
        /// <param name="jsonPath">Path were the JSON file is located</param>
        /// <returns>A specified type's object with desirialized JSON data</returns>
        public static T? DesirializeJSON<T>(string jsonPath)
        {
            try
            {
                string jsonData = File.ReadAllText(jsonPath);
                return JsonConvert.DeserializeObject<T>(jsonData);
            }
            catch(Exception)
            {
                return default(T);
            }
            
        }

    }
}
