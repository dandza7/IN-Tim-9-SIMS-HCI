﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Model
{
    public class Patient: User
    {

        public string _email { get; set; }

        public string Email
        {
            get { return _email; }
            set
            {
                if (value != _email)
                {
                    _email = value;
                    OnPropertyChanged("Email");
                }
            }
        }

        public Patient(int id, string name, string surname): base(id, name, surname, RoleType.patient)
        {
        }

        public Patient(string name, string surname): base(name, surname, RoleType.patient)
        {
        }



        public Patient(int id, string name, string surname, string jmbg, string username, string password): base(id, name, surname, jmbg, username, password, RoleType.patient)
        {
        }


        public Patient(string name, string surname, string jmbg, string username, string password): base(name, surname, jmbg, username, password, RoleType.patient)
        {
        }

        public Patient(int id, string name, string surname, string jmbg, string username, string password, string phoneNumber, string email)
            : base(id, name, surname, jmbg, username, password, phoneNumber, RoleType.patient)
        {
            Email = email;
        }

        public Patient(string name, string surname, string jmbg, string username, string password, string phoneNumber, string email)
            : base(name, surname, jmbg, username, password, phoneNumber, RoleType.patient)
        {
            Email = email;
        }
    }
}
