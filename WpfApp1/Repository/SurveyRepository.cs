using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.Repository.Interface;

namespace WpfApp1.Repository
{
    public class SurveyRepository : ISurveyRepository
    {
        private string _path;
        private string _delimiter;

        public SurveyRepository(string path, string delimiter)
        {
            _path = path;
            _delimiter = delimiter;
        }

        private int GetMaxId(IEnumerable<Survey> surveys)
        {
            return surveys.Count() == 0 ? 0 : surveys.Max(survey => survey.Id);
        }

        public IEnumerable<Survey> GetAll()
        {
            List<string> lines = File.ReadAllLines(_path).ToList();
            List<Survey> surveys = new List<Survey>();
            foreach (string line in lines)
            {
                if (line == "") continue;
                surveys.Add(ConvertCSVFormatToSurvey(line));
            }
            return surveys;
        }

        public Survey GetById(int id)
        {
            return GetAll().ToList().SingleOrDefault(survey => survey.Id == id);
        }

        public Survey Create(Survey survey)
        {
            int maxId = GetMaxId(GetAll());
            survey.Id = ++maxId;
            AppendLineToFile(_path, ConvertSurveyToCSVFormat(survey));
            return survey;
        }

        public bool Delete(int id)
        {
            List<Survey> surveys = GetAll().ToList();
            List<string> newFile = new List<string>();
            bool isDeleted = false;
            foreach (Survey survey in surveys)
            {
                if (survey.Id != id)
                {
                    newFile.Add(ConvertSurveyToCSVFormat(survey));
                    isDeleted = true;
                }
            }
            File.WriteAllLines(_path, newFile);
            return isDeleted;
        }

        private Survey ConvertCSVFormatToSurvey(string surveyCSVFormat)
        {
            var tokens = surveyCSVFormat.Split(_delimiter.ToCharArray());
            List<int> grades = ConvertCSVToIntList(tokens[4], "|");
            return new Survey(int.Parse(tokens[0]),
                int.Parse(tokens[1]),
                int.Parse(tokens[2]),
                int.Parse(tokens[3]),
                grades);
        }

        private string ConvertSurveyToCSVFormat(Survey survey)
        {
            return string.Join(_delimiter,
                survey.Id,
                survey.PatientId,
                survey.DoctorId,
                survey.AppointmentId,
                ConvertListToCSVFormat(survey.Grades, "|"));
        }

        private List<int> ConvertCSVToIntList(string stringToConvert, string delimiter)
        {
            string[] stringArray = stringToConvert.Split(delimiter.ToCharArray());
            List<string> strings = stringArray.ToList();
            List<int> integers = new List<int>();
            strings.ForEach(s => integers.Add(int.Parse(s)));
            return integers;
        }

        private string ConvertListToCSVFormat<T>(List<T> listToConvert, string delimiter)
        {
            return string.Join(delimiter, listToConvert.Select(item => item.ToString()).ToArray());
        }

        private void AppendLineToFile(string path, string line)
        {
            File.AppendAllText(path, line + Environment.NewLine);
        }
    }
}
