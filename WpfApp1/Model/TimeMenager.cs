using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.View.Model.Patient;

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
    }
}
