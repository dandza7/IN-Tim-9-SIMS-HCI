using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Model
{
    public class TimeMenager
    {
        private DateTime _beginning;
        private DateTime _ending;

        public TimeMenager(DateTime beginning, DateTime ending)
        {
            _beginning = beginning;
            _ending = ending;
        }

        public DateTime Beginning { get { return _beginning; } set { _beginning = value; } }
        public DateTime Ending { get { return _ending; } set { _ending = value; } }

        public DateTime IncrementBeginning()
        {
            Beginning = Beginning.AddHours(1);
            return Beginning;
        }

        public DateTime GetIncrementedBeginning()
        {
            DateTime toIncrement = Beginning;
            return toIncrement.AddHours(1);
        }

        public DateTime MoveStartOfIntervalToTheNextDay()
        {
            int year = Beginning.Year;
            int month = Beginning.Month;
            int day = Beginning.Day;
            DateTime start = new DateTime(year, month, day, 20, 0, 0);
            Beginning = start.AddHours(11);

            return Beginning;
        }

        public DateTime MoveStartOfIntervalIfNeeded()
        {
            if (GetIncrementedBeginning().Hour >= 20)
            {
                MoveStartOfIntervalToTheNextDay();
            }
            return Beginning;
        }

        public DateTime CalculateWorkingHours(string type)
        {
            if (type.Equals("start")) return new DateTime(Beginning.Year, Beginning.Month, Beginning.Day, 7, 0, 0);

            return new DateTime(Ending.Year, Ending.Month, Ending.Day, 20, 0, 0);
        }
    }
}
