using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.View.Model.Patient;

namespace WpfApp1.Service
{
    public class TimeMenagerService
    {
        private TimeMenager _timeMenager;
        public TimeMenagerService(TimeMenager timeMenager) 
        {
            _timeMenager = timeMenager;
        }

        public DateTime IncrementBeginning()
        {
            _timeMenager.Beginning = _timeMenager.Beginning.AddHours(1);
            return _timeMenager.Beginning;
        }

        public DateTime GetIncrementedBeginning()
        {
            DateTime toIncrement = _timeMenager.Beginning;
            return toIncrement.AddHours(1);
        }

        public DateTime MoveStartOfIntervalToTheNextDay()
        {
            int year = _timeMenager.Beginning.Year;
            int month = _timeMenager.Beginning.Month;
            int day = _timeMenager.Beginning.Day;
            DateTime start = new DateTime(year, month, day, 20, 0, 0);
            _timeMenager.Beginning = start.AddHours(11);

            return _timeMenager.Beginning;
        }

        public DateTime MoveStartOfIntervalIfNeeded()
        {
            if (GetIncrementedBeginning().Hour >= 20)
            {
                MoveStartOfIntervalToTheNextDay();
            }
            return _timeMenager.Beginning;
        }

        public DateTime CalculateWorkingHours(string type)
        {
            if (type.Equals("start")) return new DateTime(_timeMenager.Beginning.Year, _timeMenager.Beginning.Month, _timeMenager.Beginning.Day, 7, 0, 0);

            return new DateTime(_timeMenager.Ending.Year, _timeMenager.Ending.Month, _timeMenager.Ending.Day, 20, 0, 0);
        }

        public void TrimExcessiveTime()
        {
            if (_timeMenager.Beginning.Hour < 7)
            {
                _timeMenager.Beginning = CalculateWorkingHours("start");
            }
            if (_timeMenager.Ending.Hour >= 20)
            {
                _timeMenager.Ending = CalculateWorkingHours("end");
            }
            if (_timeMenager.Ending.Hour < 8)
            {
                _timeMenager.Ending = CalculateWorkingHours("end").AddDays(-1);
            }
        }

        public bool AreAvailableAppointmentsCollected(List<AppointmentView> appointments)
        {
            MoveStartOfIntervalIfNeeded();
            if (GetIncrementedBeginning() > _timeMenager.Ending) return true;
            if (appointments.Count == 10) return true;
            return false;
        }
    }
}
