using Newtonsoft.Json;
using System.Diagnostics;

namespace WebApplication2.Utilities
{
    public class JsonConvertUtil
    {
        //TODO: make these functions async

        /// <summary>
        /// Desirializes a JSON file to a desirable type
        /// </summary>
        /// <typeparam name="T">A desirable type</typeparam>
        /// <param name="jsonPath">Path were the JSON file is located</param>
        /// <returns>A specified type's object with desirialized JSON data</returns>
        public static T DesirializeJSON<T>(string jsonPath)
        {
            T? tempList = default(T);
            
            using (StreamReader r = new StreamReader(jsonPath))
            {
                var json = r.ReadToEnd();
                tempList = JsonConvert.DeserializeObject<T>(json);
            }
            return tempList;


            //AN OPTION WITHOUT NEWTONSOFT
            //JsonSerializer serializer = new JsonSerializer();
            //using (StreamReader r = new StreamReader(_jsonPath))
            //{
            //    using (JsonTextReader reader = new JsonTextReader(r))
            //    {
            //        _userList = serializer.Deserialize<List<UserModel>>(reader);
            //    }
            //}


        }


        /// <summary>
        /// Serializes and object to JSON format and writes it to a specified .json file
        /// </summary>
        /// <typeparam name="T">A desirable type</typeparam>
        /// <param name="jsonPath">Path were the JSON file is located</param>
        /// <param name="objectToSerialize">Object which is to be serialized</param>
        public static void SerializeJSON<T>(string jsonPath, T objectToSerialize)
        {
            var jsonData = JsonConvert.SerializeObject(objectToSerialize);
            File.WriteAllText(jsonPath, jsonData);
        }
    }
}
