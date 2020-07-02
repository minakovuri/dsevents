using System.Collections.Generic;

namespace dsevents
{
    class DSModel
    {
        private Dictionary<string, List<Event>> _processes { get; set; } = new Dictionary<string, List<Event>>();
        private Dictionary<string, Channel> _channels { get; set; } = new Dictionary<string, Channel>();

        public void AddProcess(Process process)
        {
            _processes.Add(process.ID, new List<Event>());
        }

        public void AddChannel(Channel channel)
        {
            _channels.Add(channel.ID, channel);
        }

        public void AddEvent(string processID, Event e)
        {
            _processes[processID].Add(e);
        }

        public int GetProcessEventsCount(string processID)
        {
            return _processes[processID].Count;
        }

        public ISet<string> GetEvents(string eventID, Mode mode)
        {
            switch (mode)
            {
            case Mode.Past:
                return GetPast(eventID);
            case Mode.Future:
                return GetFuture(eventID);
            case Mode.Concurrent:
                return GetConcurrent(eventID);
            default:
                throw new System.Exception("");
            }
        }

        private ISet<string> GetPast(string eventID)
        {
            Event _event = GetEvent(eventID);

            ISet<string> past = new SortedSet<string>();
            FindPast(_event, past);

            return past;
        }

        private ISet<string> GetFuture(string eventID)
        {
            Event analyzableEvent = GetEvent(eventID);

            ISet<string> future = new SortedSet<string>();
            FindFuture(analyzableEvent, future);

            return future;
        }

        private ISet<string> GetConcurrent(string eventID)
        {
            Event _event = GetEvent(eventID);

            ISet<string> past = new SortedSet<string>();
            FindPast(_event, past);

            ISet<string> future = new SortedSet<string>();
            FindFuture(_event, future);

            ISet<string> concurrent = new SortedSet<string>();
            FindConcurrent(_event, past, future, concurrent);

            return concurrent;
        }

        private Event GetEvent(string eventID)
        {
            foreach (var process in _processes)
            {
                foreach (Event e in process.Value)
                {
                    if (e.ID == eventID)
                    {
                        return e;
                    }
                }
            }
            return null;
        }

        private void FindPast(Event _event, ISet<string> past)
        {
            if (_event.Seq > 1)
            {
                Event pastEvent = _processes[_event.ProcessID][_event.Seq - 2];
                if (!past.Contains(pastEvent.ID))
                {
                    past.Add(pastEvent.ID);
                    FindPast(pastEvent, past);
                }
            }

            if (_event.ChannelID != null)
            {
                Channel channel = _channels[_event.ChannelID];
                if (channel.To == _event.ProcessID)
                {
                    foreach (Event e in _processes[channel.From])
                    {
                        if (e.ChannelID == channel.ID && !past.Contains(e.ID))
                        {
                            past.Add(e.ID);
                            FindPast(e, past);
                        }
                    }
                }
            }
        }

        private void FindFuture(Event _event, ISet<string> future)
        {
            if (_event.Seq < _processes[_event.ProcessID].Count)
            {
                Event futureEvent = _processes[_event.ProcessID][_event.Seq];
                if (!future.Contains(futureEvent.ID))
                {
                    future.Add(futureEvent.ID);
                    FindFuture(futureEvent, future);
                }
            }

            if (_event.ChannelID != null)
            {
                Channel channel = _channels[_event.ChannelID];
                if (channel.From == _event.ProcessID)
                {
                    foreach (Event e in _processes[channel.To])
                    {
                        if (e.ChannelID == channel.ID && !future.Contains(e.ID))
                        {
                            future.Add(e.ID);
                            FindFuture(e, future);
                        }
                    }
                }
            }
        }

        private void FindConcurrent(Event _event, ISet<string> past, ISet<string> future, ISet<string> concurrent)
        {
            foreach (var process in _processes)
            {
                foreach (Event e in process.Value)
                {
                    if (e.ID != _event.ID && !past.Contains(e.ID) && !future.Contains(e.ID))
                    {
                        concurrent.Add(e.ID);
                    }
                }
            }
        }
    }
}