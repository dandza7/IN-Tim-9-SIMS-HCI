using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.View.Model.Patient;

namespace WpfApp1.View.Converter
{
    internal class ReportConverter : AbstractConverter
    {
        public static ReportDetailsView ConvertAppointmentViewAndReportToReportDetailsView(DateTime beginning, DateTime ending, string username, string nametag, string reportContent)
        => new ReportDetailsView
        {
            Beginning = beginning,
            Ending = ending,
            Username = username,
            Nametag = nametag,
            ReportContent = reportContent
        };
    }
}
