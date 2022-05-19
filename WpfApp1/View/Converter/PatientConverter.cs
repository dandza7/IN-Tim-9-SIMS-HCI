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
        public static PatientView ConvertPatientToPatientView(User user)
        => new PatientView
        {
            Id = user.Id,
            FirstName = user.Name,
            Surname = user.Surname,
            Jmbg = user.Jmbg,
            Username = user.Username
        };

        public static IList<PatientView> ConvertPatientListToPatientViewList(IList<User> patients)
            => ConvertEntityListToViewList(patients, ConvertPatientToPatientView);
    }
}
