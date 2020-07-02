using System;
using System.Text.Json;

namespace dsevents
{
    class Parser
    {
        public static DSModel ReadDsModel()
        {
            JsonModel jsonModel = ParseJsonModel();

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
        private static JsonModel ParseJsonModel()
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