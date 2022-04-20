using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.View.Model;

namespace WpfApp1.View.Converter
{
    internal class PatientConverter : AbstractConverter
    {
        public static PatientView ConvertPatientToPatientView(User patient)
    => new PatientView
    {
        Id = patient.Id,
        Name = patient.Name,
        Surname = patient.Surname,
        JMBG = patient.Jmbg,
        Username = patient.Username,
        Password = patient.Password
    };

        public static IList<PatientView> ConvertPatientListToPatientViewList(IList<User> patients)
            => ConvertEntityListToViewList(patients, ConvertPatientToPatientView);
    }
}
