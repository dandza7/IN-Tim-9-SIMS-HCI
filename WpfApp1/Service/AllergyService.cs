﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.Repository;

namespace WpfApp1.Service
{
    public class AllergyService
    {
        private readonly AllergyRepository _allergyRepo;

        public AllergyService(AllergyRepository therapyRepo)
        {
            _allergyRepo = therapyRepo;
        }
        public IEnumerable<Allergy> GetAll()
        {
            return _allergyRepo.GetAll();
        }

        public Allergy Create(Allergy allergy)
        {
            return _allergyRepo.Create(allergy);
        }
        public IEnumerable<Allergy> GetAllAllergiesForPatient(int medicalRecordId)
        {
            return _allergyRepo.GetPatientsAllergies(medicalRecordId);
        }
    }
}
