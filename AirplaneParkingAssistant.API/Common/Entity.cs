using System;
using System.Collections.Generic;
using System.Linq;

namespace AirplaneParkingAssistant.API.Common
{
    public interface IEvent
    {

    }
    public class Entity
    {
        public Guid Id { get; } = Guid.NewGuid();

        private readonly List<IEvent> _events = new List<IEvent>();
        public IReadOnlyList<IEvent> Events => _events?.ToList();

        protected Entity()
        {

        }

        public void AddEvent(IEvent @event)
        {
            _events.Add(@event);
        }
    }
}
