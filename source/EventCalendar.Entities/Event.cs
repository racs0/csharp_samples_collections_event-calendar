using System;
using System.Collections.Generic;

namespace EventCalendar.Entities
{
    public class Event
    {
        private string _title;
        private DateTime _termin;
        private Person _invitor;
        private IList<Person> _participators;


        public string Title {
            get { return _title; }
        }

        public DateTime Termin
        {
            get { return _termin; }
        }

        public Person Invitor
        {
            get { return _invitor; }
        }

        public IList<Person> Participators
        {
            get { return _participators; }
            set { _participators = value; }
        }

        public Event(Person invitor, string title, DateTime termin)
        {
            _invitor = invitor;
            _title = title;
            _termin = termin;
            _participators = new List<Person>();
        }
    }
}
