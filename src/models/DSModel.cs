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
                return null;
            }
        }

        private ISet<string> GetPast(string eventID)
        {
            Event _event = GetEvent(eventID);

            ISet<string> past = new SortedSet<string>();
            FillPastEvents(_event, past);

            return past;
        }

        private ISet<string> GetFuture(string eventID)
        {
            Event analyzableEvent = GetEvent(eventID);

            ISet<string> future = new SortedSet<string>();
            FillFutureEvents(analyzableEvent, future);

            return future;
        }

        private ISet<string> GetConcurrent(string eventID)
        {
            Event _event = GetEvent(eventID);

            ISet<string> past = new SortedSet<string>();
            FillPastEvents(_event, past);

            ISet<string> future = new SortedSet<string>();
            FillFutureEvents(_event, future);

            ISet<string> concurrent = new SortedSet<string>();
            FillConcurrentEvents(_event, past, future, concurrent);

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

        private void FillPastEvents(Event _event, ISet<string> pastEvents)
        {
            if (_event.Seq > 1)
            {
                Event pastEvent = _processes[_event.ProcessID][_event.Seq - 2];
                if (!pastEvents.Contains(pastEvent.ID))
                {
                    pastEvents.Add(pastEvent.ID);
                    FillPastEvents(pastEvent, pastEvents);
                }
            }

            if (_event.ChannelID != null)
            {
                Channel channel = _channels[_event.ChannelID];
                if (channel.To == _event.ProcessID)
                {
                    foreach (Event e in _processes[channel.From])
                    {
                        if (e.ChannelID == channel.ID && !pastEvents.Contains(e.ID))
                        {
                            pastEvents.Add(e.ID);
                            FillPastEvents(e, pastEvents);
                        }
                    }
                }
            }
        }

        private void FillFutureEvents(Event _event, ISet<string> futureEvents)
        {
            if (_event.Seq < _processes[_event.ProcessID].Count)
            {
                Event futureEvent = _processes[_event.ProcessID][_event.Seq];
                if (!futureEvents.Contains(futureEvent.ID))
                {
                    futureEvents.Add(futureEvent.ID);
                    FillFutureEvents(futureEvent, futureEvents);
                }
            }

            if (_event.ChannelID != null)
            {
                Channel channel = _channels[_event.ChannelID];
                if (channel.From == _event.ProcessID)
                {
                    foreach (Event e in _processes[channel.To])
                    {
                        if (e.ChannelID == channel.ID && !futureEvents.Contains(e.ID))
                        {
                            futureEvents.Add(e.ID);
                            FillFutureEvents(e, futureEvents);
                        }
                    }
                }
            }
        }

        private void FillConcurrentEvents(Event _event, ISet<string> pastEvents, ISet<string> futureEvents, ISet<string> concurrentEvents)
        {
            foreach (var process in _processes)
            {
                foreach (Event e in process.Value)
                {
                    if (e.ID != _event.ID && !pastEvents.Contains(e.ID) && !futureEvents.Contains(e.ID))
                    {
                        concurrentEvents.Add(e.ID);
                    }
                }
            }
        }
    }
}