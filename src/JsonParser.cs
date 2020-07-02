using System;
using System.Text.Json;

namespace dsevents
{
    class JsonParser
    {
        public static JsonModel ParseJsonModel()
        {
            string inputString = "";
            while (true)
            {
                string line = Console.ReadLine();
                if (line != null)
                {
                    inputString += line;
                }
                else
                {
                    break;
                }
            }
            return JsonSerializer.Deserialize<JsonModel>(inputString);
        }
    }
}