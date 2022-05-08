using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.Service;

namespace WpfApp1.Controller
{
    public class SurveyController
    {
        private readonly SurveyService _surveyService;

        public SurveyController(SurveyService surveyService)
        {
            _surveyService = surveyService;
        }

        public IEnumerable<Survey> GetAll()
        {
            return _surveyService.GetAll();
        }

        public Survey GetById(int id)
        {
            return _surveyService.GetById(id);
        }

        public bool CheckIfAlreadyGraded(int patientId, int appointmentId)
        {
            return _surveyService.CheckIfAlreadyGraded(patientId, appointmentId);
        }

        public Survey Create(Survey survey)
        {
            return _surveyService.Create(survey);
        }

        public bool Delete(int id)
        {
            return _surveyService.Delete(id);
        }
    }
}
