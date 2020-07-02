using System.Collections.Generic;

namespace dsevents
{
    class Program
    {
        static void Main(string[] args)
        {
            DSModel dSModel = Parser.ReadDsModel();

            Mode mode = Mode.Past;

            if (args[0] == "past")
            {
                mode = Mode.Past;
            }
            else if (args[0] == "future")
            {
                mode = Mode.Future;
            }
            else if (args[0] == "concurrent")
            {
                mode = Mode.Concurrent;
            }

            string eventID = args[1];
            ISet<string> events = dSModel.GetEvents(eventID, mode);

            FileWriter.PrintEvents(events);
        }
    }
}
