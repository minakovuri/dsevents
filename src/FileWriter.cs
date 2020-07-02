using System.Collections.Generic;
using System;

namespace dsevents
{
    class FileWriter
    {
        public static void PrintEventIDs(Dictionary<string, ISet<string>> eventIDsMap)
        {
            foreach (var eventIDs in eventIDsMap)
            {
                Console.WriteLine(eventIDs.Key + ":");
                foreach (string eventID in eventIDs.Value)
                {
                    Console.WriteLine("--" + eventID);
                }
            }
        }
    }
}