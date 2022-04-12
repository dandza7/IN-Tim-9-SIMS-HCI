﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.Service;

namespace WpfApp1.Controller
{
    public class PatientController
    {
        private readonly PatientService _patientService;

        public PatientController(PatientService patientService)
        {
            _patientService = patientService;
        }

        public IEnumerable<Patient> GetAll()
        {
            //Console.WriteLine("nesto\n");
            return _patientService.GetAll();
        }

        public Patient Create(Patient patient)
        {
            return _patientService.Create(patient);
        }
        public Patient Update(Patient patient)
        {
            return _patientService.Update(patient);
        }
        public bool Delete(int patientId)
        {
            return _patientService.Delete(patientId);
        }
        public Patient Find(int patientId)
        {
            return _patientService.Find(patientId);
        }
    }
}