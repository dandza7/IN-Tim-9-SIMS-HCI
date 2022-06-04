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
        public List<Survey> GetAllByDoctorsId(int docId)
        {
            return _surveyService.GetAllByDoctorsId(docId);
        }
        public Survey GetById(int id)
        {
            return _surveyService.GetById(id);
        }

        public bool IsAlreadyGraded(int patientId, int appointmentId)
        {
            return _surveyService.IsAlreadyGraded(patientId, appointmentId);
        }

        public Survey Create(List<int> grades, int appointmentId, int patientId)
        {
            return _surveyService.Create(grades, appointmentId, patientId);
        }

        public bool Delete(int id)
        {
            return _surveyService.Delete(id);
        }
    }
}
