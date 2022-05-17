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
        public static PatientView ConvertPatientToPatientView(User user, Patient patient)
        => new PatientView
        {
            Id = user.Id,
            FirstName = user.Name,
            Surname = user.Surname,
            Jmbg = user.Jmbg,
            Username = user.Username,
            Password = user.Password,
            PhoneNumber = user.PhoneNumber,
            Email = patient.Email,
            Street = patient.Street,
            City = patient.City,
            Country = patient.Country
        };
    }
}
