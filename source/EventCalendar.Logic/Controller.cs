using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using EventCalendar.Entities;
using static System.String;

namespace EventCalendar.Logic
{
    public class Controller
    {
        private readonly ICollection<Event> _events;
        public int EventsCount { get { return _events.Count;} }

        public Controller()
        {
            _events = new List<Event>();
        }

        /// <summary>
        /// Ein Event mit dem angegebenen Titel und dem Termin wird für den Einlader angelegt.
        /// Der Titel muss innerhalb der Veranstaltungen eindeutig sein und das Datum darf nicht
        /// in der Vergangenheit liegen.
        /// Mit dem optionalen Parameter maxParticipators kann eine Obergrenze für die Teilnehmer festgelegt
        /// werden.
        /// </summary>
        /// <param name="invitor"></param>
        /// <param name="title"></param>
        /// <param name="dateTime"></param>
        /// <param name="maxParticipators"></param>
        /// <returns>Wurde die Veranstaltung angelegt</returns>
        public bool CreateEvent(Person invitor, string title, DateTime dateTime, int maxParticipators = 0)
        {
            DateTime now = DateTime.Now;

            if (_events.Count == 0)
            {
                if(!string.IsNullOrEmpty(title) && invitor != null)
                {
                    if(dateTime > now)
                    {
                        if(maxParticipators > 0)
                        {
                            LimitedEvent newLimitedEvent = new LimitedEvent(invitor, title, dateTime, maxParticipators);
                            _events.Add(newLimitedEvent);
                        }
                        else
                        {
                            Event newEvent = new Event(invitor, title, dateTime);
                            _events.Add(newEvent);
                        }
                       
                        return true;

                    }
                }
            }
            else
            {
                foreach (Event item in _events)
                {
                    if (!item.Title.Equals(title) && !string.IsNullOrEmpty(title))
                    {
                        if (dateTime > now)
                        {
                            Event newEvent = new Event(invitor, title, dateTime);
                            _events.Add(newEvent);
                            return true;
                        }
                    }
                }
            }
            

            return false;
          
        }


        /// <summary>
        /// Liefert die Veranstaltung mit dem Titel
        /// </summary>
        /// <param name="title"></param>
        /// <returns>Event oder null, falls es keine Veranstaltung mit dem Titel gibt</returns>
        public Event GetEvent(string title)
        {
            foreach (Event item in _events)
            {
                if(item.Title.Equals(title))
                {
                    return item;
                }
            }

            return null;
        }

        /// <summary>
        /// Person registriert sich für Veranstaltung.
        /// Eine Person kann sich zu einer Veranstaltung nur einmal registrieren.
        /// </summary>
        /// <param name="person"></param>
        /// <param name="ev">Veranstaltung</param>
        /// <returns>War die Registrierung erfolgreich?</returns>
        public bool RegisterPersonForEvent(Person person, Event ev)
        {
            if(person != null && ev != null)
            {
                foreach (Event item in _events)
                {
                    if (!item.Participators.Contains(person))
                    {
                        if (ev is LimitedEvent)
                        {
                            if ((ev as LimitedEvent).Participators.Count < (ev as LimitedEvent).MaxParticipators)
                            {
                                ev.Participators.Add(person);
                                return true;
                            }
                        }
                        else
                        {
                            ev.Participators.Add(person);
                            return true;
                        }
                        
                    }
                }

            }

            return false;
        }

        /// <summary>
        /// Person meldet sich von Veranstaltung ab
        /// </summary>
        /// <param name="person"></param>
        /// <param name="ev">Veranstaltung</param>
        /// <returns>War die Abmeldung erfolgreich?</returns>
        public bool UnregisterPersonForEvent(Person person, Event ev)
        {
            if (person != null && ev != null)
            {
                foreach (Event item in _events)
                {
                    if (item.Equals(ev))
                    {
                        if (item.Participators.Contains(person))
                        {
                            ev.Participators.Remove(person);
                            return true;

                        }
                    }
                }

            }

            return false;
        }

        /// <summary>
        /// Liefert alle Teilnehmer an der Veranstaltung.
        /// Sortierung absteigend nach der Anzahl der Events der Personen.
        /// Bei gleicher Anzahl nach dem Namen der Person (aufsteigend).
        /// </summary>
        /// <param name="ev"></param>
        /// <returns>Liste der Teilnehmer oder null im Fehlerfall</returns>
        public IList<Person> GetParticipatorsForEvent(Event ev)
        {
            IList<Person> SortedParticipators = new List<Person>();

            foreach (Event item in _events)
            {
                if (item.Participators.Count > 0)
                {
                    if (item.Equals(ev))
                    {
                        SortedParticipators = item.Participators;
                    }
                }
            }

            return ev != null ? SortedParticipators : null;
        }

        /// <summary>
        /// Liefert alle Veranstaltungen der Person nach Datum (aufsteigend) sortiert.
        /// </summary>
        /// <param name="person"></param>
        /// <returns>Liste der Veranstaltungen oder null im Fehlerfall</returns>
        public List<Event> GetEventsForPerson(Person person)
        {
            List<Event> FilteredPersonEvent = new List<Event>();

            foreach (Event item in _events)
            {
                if (item.Participators.Count > 0)
                {
                    if (item.Participators[0].Equals(person))
                    {
                        FilteredPersonEvent.Add(item);
                    }
                }
            }

            return person != null ? FilteredPersonEvent : null;
        }

        /// <summary>
        /// Liefert die Anzahl der Veranstaltungen, für die die Person registriert ist.
        /// </summary>
        /// <param name="participator"></param>
        /// <returns>Anzahl oder 0 im Fehlerfall</returns>
        public int CountEventsForPerson(Person participator)
        {
            //int count = 0;
            //int index = 0;

            //foreach (Event item in _events)
            //{
            //    if(item.Participators.Count > 0)
            //    {
            //        if (item.Participators[index].Equals(participator))
            //        {
            //            count++;
            //        }
            //        else
            //        {
            //            index++;
            //        }
            //    }

            //}

            //return count > 0 ? count : 0;
            return participator != null ? GetEventsForPerson(participator).Count : 0;
        }

    }
}
