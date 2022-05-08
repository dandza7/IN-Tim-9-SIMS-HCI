using System;
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

        public SurveyService(SurveyRepository surveyRepository)
        {
            _surveyRepository = surveyRepository;
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

        public Survey Create(Survey survey)
        {
            return _surveyRepository.Create(survey);
        }

        public bool Delete(int id)
        {
            return _surveyRepository.Delete(id);
        }
    }
}
