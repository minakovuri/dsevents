using System;
using System.Text.Json;

namespace dsevents
{
    class JsonParser
    {
        public static JsonModel ParseJsonModel()
        {
            string input = "";
            while (true)
            {
                string line = Console.ReadLine();
                if (line != null)
                {
                    input += line;
                }
                else
                {
                    break;
                }
            }
            return JsonSerializer.Deserialize<JsonModel>(input);
        }
    }
}