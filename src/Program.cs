using System.Collections.Generic;

namespace dsevents
{
    class Program
    {
        static void Main(string[] args)
        {
            JsonModel jsonModel = JsonParser.ParseJsonModel();
            DSModel dSModel = GetDSModel(jsonModel);

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

        private static DSModel GetDSModel(JsonModel jsonModel)
        {
            DSModel dSModel = new DSModel();

            foreach (Channel channel in jsonModel.Channels)
            {
                dSModel.AddChannel(channel);
            }

            foreach (Process process in jsonModel.Processes)
            {
                dSModel.AddProcess(process);
            }

            bool addEventFlag = false;
            while (true)
            {
                addEventFlag = false;
                foreach (Event e in jsonModel.Events)
                {
                    string processID = e.ProcessID;
                    if (dSModel.GetProcessEventsCount(processID) + 1 == e.Seq)
                    {
                        dSModel.AddEvent(processID, e);
                        addEventFlag = true;
                    }
                }

                if (!addEventFlag)
                {
                    break;
                }
            }

            return dSModel;
        }
    }
}
