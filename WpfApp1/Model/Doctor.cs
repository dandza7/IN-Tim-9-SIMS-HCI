using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            pediatrician,
            anesthesiologist
        }

        private SpecType _specialization;
        private bool _isAvailable;
        private int _roomId;
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

        public int RoomId
        {
            get
            {
                return _roomId;
            }
            set
            {
                if (value != _roomId)
                {
                    _roomId = value;
                    OnPropertyChanged("RoomId");
                }
            }
        }

        public Doctor(int id, string name, string surname, string username, string password, string phoneNumber, string jmbg, RoleType role,
            SpecType specialization, bool isAvailable, int roomId): base(id, name, surname, username, password, phoneNumber, jmbg, RoleType.doctor)
        {
            Specialization = specialization;
            IsAvailable = isAvailable;
            RoomId = roomId;
        }

        public Doctor(int id, SpecType specialization, bool isAvailable, int roomId)
        {
            Id = id;
            Specialization = specialization;
            IsAvailable = isAvailable;
            RoomId = roomId;
        }

        public Doctor()
        {
        }
    }
}
