using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Controller;
using WpfApp1.Model;
using WpfApp1.Model.Preview;

namespace WpfApp1.ViewModel
{
    internal class DoctorStatisticsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
        private SurveyController surveyController;
        public string DoctorsName { get; set; }
        public double Grade1 { get; set; }
        public double Grade2 { get; set; }
        public double Grade3 { get; set; }
        public double Grade4 { get; set; }
        public double Grade5 { get; set; }
        public double AvgGrade { get; set; }
        public DoctorStatisticsViewModel(DoctorPreview doctor)
        {
            DoctorsName = doctor.Name;
            var app = Application.Current as App;
            surveyController = app.SurveyController;
            List<Survey> surveys = surveyController.GetAllByDoctorsId(doctor.Id);
            int count = 0;
            foreach (Survey survey in surveys)
            {
                count++;
                int gradeId = 0;
                foreach(int grade in survey.Grades)
                {
                    switch (gradeId)
                    {
                        case 0:
                            Grade1 += grade;
                            AvgGrade += grade;
                            gradeId++;
                            break;
                        case 1:
                            Grade2 += grade;
                            AvgGrade += grade;
                            gradeId++;
                            break;
                        case 2:
                            Grade3 += grade;
                            AvgGrade += grade;
                            gradeId++;
                            break;
                        case 3:
                            Grade4 += grade;
                            AvgGrade += grade;
                            gradeId++;
                            break;
                        default:
                            Grade5 += grade;
                            AvgGrade += grade;
                            gradeId++;
                            break;
                    }
                }
            }
            if(count > 0)
            {
                Grade1 = Math.Round(Grade1 / count, 2);
                Grade2 = Math.Round(Grade2 / count, 2);
                Grade3 = Math.Round(Grade3 / count, 2);
                Grade4 = Math.Round(Grade4 / count, 2);
                Grade5 = Math.Round(Grade5 / count, 2);
                AvgGrade = Math.Round(AvgGrade / (count*5), 2);
            }
            else
            {
                Grade1 = 0;
                Grade2 = 0;
                Grade3 = 0;
                Grade4 = 0;
                Grade5 = 0;
                AvgGrade = 0;
            }
            
        }
    }
}
