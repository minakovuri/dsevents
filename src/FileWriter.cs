using System.Collections.Generic;
using System;

namespace dsevents
{
    class FileWriter
    {
        public static void PrintEvents(ISet<string> eventIDs)
        {
            foreach (string eventID in eventIDs)
            {
                Console.Write(eventID + ' ');
            }
            Console.WriteLine();
        }
    }
}