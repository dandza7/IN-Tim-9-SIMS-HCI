﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.Service;

namespace WpfApp1.Controller
{
    public class AllergyController
    {
        private readonly AllergyService _allergyService;

        public AllergyController(AllergyService allergyservice)
        {
            _allergyService = allergyservice;
        }
        public IEnumerable<Allergy> GetAll()
        {
            return _allergyService.GetAll();
        }

        public Allergy Create(Allergy allergy)
        {
            return _allergyService.Create(allergy);
        }
        public IEnumerable<Allergy> GetAllAllergiesForPatient(int medicalRecordId)
        {
            return _allergyService.GetAllAllergiesForPatient(medicalRecordId);
        }
    }
}