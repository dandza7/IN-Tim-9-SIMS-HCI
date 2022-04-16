using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Model
{
    public class Doctor: User
    {
        public enum SpecType
        {
            cardiologist,
            neurologist,
            radiologist,
            generalPracticioner,
            pediatricians,
            anesthesiologists
        }

        private SpecType _specialization;
        private bool _isAvailable;

        public SpecType Specialization
        {
            get { return _specialization; }
            set
            {
                if (value != _specialization)
                {
                    _specialization = value;
                    OnPropertyChanged("Specialization");
                }
            }
        }

        public bool IsAvailable
        {
            get { return _isAvailable; }
            set
            {
                if (value != _isAvailable)
                {
                    _isAvailable = value;
                    OnPropertyChanged("IsAvailable");
                }
            }
        }

        public Doctor(string name, string surname, int id, string username, string password, string phoneNumber, string jmbg, RoleType role,
            SpecType specialization, bool isAvailable): base(name, surname, id, username, password, phoneNumber, jmbg, role)
        {
            Specialization = specialization;
            IsAvailable = isAvailable;
        }
    }
}
