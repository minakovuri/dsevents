using System.Collections.Generic;

namespace dsevents
{
   class JsonModel
    {
        public List<Process> Processes { get; set; }
        public List<Channel> Channels { get; set; }
        public List<Message> Messages { get; set; }
        public List<Event> Events { get; set; }
    }
}