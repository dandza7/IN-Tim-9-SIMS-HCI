using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.View.Model.Doctor;
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
        public static DoctorReportView Convert_DoctorReport_To_DoctorReportView(DoctorsReport dr)
        => new DoctorReportView
        {
            Id=dr.Id,
            AppointmentId=dr.AppointmentId,
            Description=dr.Description
        };
    }
}
