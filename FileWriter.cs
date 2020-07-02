using System.Collections.Generic;
using System;

namespace dsevents
{
    class FileWriter
    {
        public static void PrintEventIDs(Mode mode, Dictionary<string, ISet<string>> eventIDsMap)
        {
            string title = "";
            switch (mode)
            {
            case Mode.Past:
                title = "Past: ";
                break;
            case Mode.Future:
                title = "Future: ";
                break;
            case Mode.Concurrent:
                title = "Concurrent: ";
                break;
            }
            Console.WriteLine(title);

            foreach (var eventIDs in eventIDsMap)
            {
                Console.WriteLine("  " + eventIDs.Key + ":");
                foreach (string eventID in eventIDs.Value)
                {
                    Console.WriteLine("    " + eventID);
                }
            }
        }
    }
}