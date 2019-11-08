using System;
using System.Collections.Generic;
using System.Text;

namespace EventCalendar.Entities
{
    public class LimitedEvent : Event
    {
        private int _maxParticipators;

        public int MaxParticipators
        {
            get { return _maxParticipators; }
            set { _maxParticipators = value; }
        }

        public LimitedEvent(Person invitor, string title, DateTime termin, int maxParticipators) : base(invitor, title, termin)
        {
            _maxParticipators = maxParticipators;
        }
    }
}
