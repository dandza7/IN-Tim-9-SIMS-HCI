﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.Repository;

namespace WpfApp1.Service
{
    public class RenovationService
    {
        public readonly RenovationRepository _renovationRepository;
        public readonly AppointmentRepository _appointmentRepository;

        public RenovationService(RenovationRepository renovationRepository, AppointmentRepository appointmentRepository)
        {
            _renovationRepository = renovationRepository;
            _appointmentRepository = appointmentRepository;
        }

        public Renovation Create(Renovation renovation)
        {
            return _renovationRepository.Create(renovation);
        }

        public void PrintAll()
        {
            List<Renovation> all = _renovationRepository.GetAll();
            foreach (Renovation item in all)
            {
                Console.WriteLine("{0}  {1}  {2}  {3}  {4}  TD: {5}", item.Id, item.RoomId, item.Description, item.Beginning.ToShortDateString(), item.Ending.ToShortDateString(), DateTime.Today);
            }
        }
        public List<String> GetBeginnings(int roomId)
        {
            List<String> beginnings = new List<String>();
            List<Appointment> appointmentsRaw = _appointmentRepository.GetAll().ToList();
            List<Appointment> appointments = new List<Appointment>();
            List<Renovation> renovationsRaw = _renovationRepository.GetAll().ToList();
            List<Renovation> renovations = new List<Renovation>();
            foreach (Appointment appointment in appointmentsRaw)
            {
                //Console.WriteLine("{0} == {1}", appointment.RoomId, roomId);
                if(appointment.RoomId == roomId)
                {
                    //Console.WriteLine("Dodato");
                    appointments.Add(appointment);
                }
            }
            foreach(Renovation renovation in renovationsRaw)
            {
                if(renovation.RoomId == roomId)
                {
                    renovations.Add(renovation);
                }
            }
            DateTime checker = DateTime.Today;
            for(int i = 0; i < 14; i++)
            {
                bool isFree = true;
                foreach(Appointment appointment in appointments)
                {
                    //Console.WriteLine("{0} ? {1} == {2}", checker.ToShortDateString(), appointment.Beginning.ToShortDateString(), checker.ToShortDateString().Equals(appointment.Beginning.ToShortDateString()));
                    if (checker.ToShortDateString().Equals(appointment.Beginning.ToShortDateString()))
                    {
                        isFree = false;
                        break;
                    }
                    
                    
                }
                foreach(Renovation renovation in renovations)
                {
                    if (DateTime.Compare(DateTime.Parse(checker.ToShortDateString()), DateTime.Parse(renovation.Beginning.ToShortDateString())) >= 0
                        && DateTime.Compare(DateTime.Parse(checker.ToShortDateString()), DateTime.Parse(renovation.Ending.ToShortDateString())) <= 0)
                    {
                        isFree = false;
                        break;
                    }
                }
                if (isFree)
                {
                    beginnings.Add(checker.ToShortDateString());
                }
                checker = checker.AddDays(1);
            }
            


            return beginnings;
        }
        public List<String> GetEndings(string beginning, int roomId)
        {
            List<String> endings = new List<String>();
            List<Appointment> appointmentsRaw = _appointmentRepository.GetAll().ToList();
            List<Appointment> appointments = new List<Appointment>();
            List<Renovation> renovationsRaw = _renovationRepository.GetAll().ToList();
            List<Renovation> renovations = new List<Renovation>();
            foreach (Appointment appointment in appointmentsRaw)
            {
                //Console.WriteLine("{0} == {1}", appointment.RoomId, roomId);
                if (appointment.RoomId == roomId)
                {
                    //Console.WriteLine("Dodato");
                    appointments.Add(appointment);
                }
            }
            foreach (Renovation renovation in renovationsRaw)
            {
                if (renovation.RoomId == roomId)
                {
                    renovations.Add(renovation);
                }
            }
            DateTime checker = DateTime.Parse(beginning);
            for (int i = 0; i < 14; i++)
            {
                bool isFree = true;
                foreach (Appointment appointment in appointments)
                {
                    //Console.WriteLine("{0} ? {1} == {2}", checker.ToShortDateString(), appointment.Beginning.ToShortDateString(), checker.ToShortDateString().Equals(appointment.Beginning.ToShortDateString()));
                    if (checker.ToShortDateString().Equals(appointment.Beginning.ToShortDateString()))
                    {
                        isFree = false;
                        break;
                    }


                }
                foreach (Renovation renovation in renovations)
                {
                    if (DateTime.Compare(DateTime.Parse(checker.ToShortDateString()), DateTime.Parse(renovation.Beginning.ToShortDateString())) >= 0
                        && DateTime.Compare(DateTime.Parse(checker.ToShortDateString()), DateTime.Parse(renovation.Ending.ToShortDateString())) <= 0)
                    {
                        isFree = false;
                        break;
                    }
                }
                if (isFree)
                {
                    
                    endings.Add(checker.ToShortDateString());
                } 
                else
                {
                    
                    break;
                }
                
                checker = checker.AddDays(1);
            }
            return endings;
        }
    }
}