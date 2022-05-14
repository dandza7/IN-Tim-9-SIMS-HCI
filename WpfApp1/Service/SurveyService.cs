﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.Repository;

namespace WpfApp1.Service
{
    public class SurveyService
    {
        private readonly SurveyRepository _surveyRepository;
        private readonly AppointmentRepository _appointmentRepository;
        private readonly DoctorRepository _doctorRepository; 

        public SurveyService(SurveyRepository surveyRepository, AppointmentRepository appointmentRepository, DoctorRepository doctorRepository)
        {
            _surveyRepository = surveyRepository;
            _appointmentRepository = appointmentRepository;
            _doctorRepository = doctorRepository;
        }

        public IEnumerable<Survey> GetAll()
        {
            return _surveyRepository.GetAll();
        }

        public Survey GetById(int id)
        {
            return _surveyRepository.GetById(id);
        }

        public bool CheckIfAlreadyGraded(int patientId, int appointmentId)
        {
            bool isGraded = false;
            List<Survey> allSurveys = _surveyRepository.GetAll().ToList();
            foreach(Survey survey in allSurveys)
            {
                if(survey.PatientId == patientId && survey.AppointmentId == appointmentId)
                {
                    isGraded = true;
                }
            }
            return isGraded;
        }

        public Survey Create(List<int> grades, int appointmentId, int patientId)
        {
            Appointment appointment = _appointmentRepository.GetById(appointmentId);
            Doctor doctor = _doctorRepository.GetById(appointment.DoctorId);
            Survey completedSurvey = new Survey(patientId, doctor.Id, appointmentId, grades);
            return _surveyRepository.Create(completedSurvey);
        }

        public bool Delete(int id)
        {
            return _surveyRepository.Delete(id);
        }
    }
}
