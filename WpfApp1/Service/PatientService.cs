using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.Repository;

namespace WpfApp1.Service
{
    public class PatientService
    {
        private readonly PatientRepository _patientRepo;
        private readonly TherapyRepository _therapyRepo;
        private readonly UserRepository _userRepo;
        public PatientService(UserRepository userRepo, PatientRepository patientRepo, TherapyRepository therapyRepository) 
        {
            _patientRepo = patientRepo;
            _therapyRepo = therapyRepository;
            _userRepo = userRepo;
        }

        public IEnumerable<Patient> GetAll()
        {
            return _patientRepo.GetAll();
        }

        /*public List<Therapy> GetPatientsTherapies(int patientId)
        {
            List<Therapy> patientsTherapies = new List<Therapy>();
            //List<int> therapyIds = _patientRepo.GetById(patientId).TherapyIds;

            //therapyIds.ForEach(therapy => patientsTherapies.Add(_therapyRepo.GetById(therapy)));

            return patientsTherapies;
        }*/
      
        public Patient Create(Patient patient)
        {


            return _patientRepo.Create(patient);
        }

        public Patient Update(Patient patient)
        {


            return _patientRepo.Update(patient);
        }
        public bool Delete(int patientId)
        {


            return _patientRepo.Delete(patientId);
        }
        public Patient GetById(int patientId)
        {


            return _patientRepo.GetById(patientId);
        }
    }
}
