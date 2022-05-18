using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp1.Controller;
using WpfApp1.Model;
using WpfApp1.View.Model.Patient;

namespace WpfApp1.View.Dialog.PatientDialog
{
    /// <summary>
    /// Interaction logic for HospitalSurveyDialog.xaml
    /// </summary>
    public partial class HospitalSurveyDialog : Page
    {
        private SurveyController _surveyController;

        private List<string> _questions;

        public List<string> Questions
        {
            get { return _questions; }
            set
            {
                if (value != null) _questions = value;
            }
        }

        public HospitalSurveyDialog()
        {
            InitializeComponent();
            DataContext = this;

            var app = Application.Current as App;
            _surveyController = app.SurveyController;

            Questions = GetHospitalSurvey();
        }

        private void GradeButton_Click(object sender, RoutedEventArgs e)
        {
            List<int> grades = GetGrades();
            if (IsEveryQuestionsAnswered(grades) == false)
            {
                PatientErrorMessageBox.Show("ERROR: You did not answer all of the questions!");
                return;
            }

            var app = Application.Current as App;
            _surveyController = app.SurveyController;
            
            _surveyController.Create(grades, -1, -1);
            PatientErrorMessageBox.Show("Thank you for completing the survey!");
            Frame patientFrame = (Frame)app.Properties["PatientFrame"];
            patientFrame.Content = new PatientProfileView();
        }

        private bool IsEveryQuestionsAnswered(List<int> grades)
        {
            return (grades.Count < 5 ? false : true);
        }

        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            const string SURVEY_HELP = "Surveys help us see the areas that we lack in and improve on those. " +
                "In order to fill a survey successfully answer" +
                "the 5 questions given to you by grading them from 1 to 5. " +
                "5 being the best grade, meaning your experience was impeccable." +
                "On the other hand, 1 means you were not happy with our service at all regarding that area.";

            PatientHelp.Show(SURVEY_HELP);
        }

        public static IEnumerable<T> FindVisualChilds<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj == null) yield return (T)Enumerable.Empty<T>();
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                DependencyObject ithChild = VisualTreeHelper.GetChild(depObj, i);
                if (ithChild == null) continue;
                if (ithChild is T t) yield return t;
                foreach (T childOfChild in FindVisualChilds<T>(ithChild)) yield return childOfChild;
            }
        }

        private List<int> GetGrades()
        {
            List<int> grades = new List<int>();
            var app = Application.Current as App;
            Window patientMenu = (Window)app.Properties["PatientMenu"];
            Console.WriteLine(patientMenu.GetType());
            foreach (RadioButton rb in FindVisualChilds<RadioButton>(patientMenu))
            {
                if (rb.IsChecked == true)
                {
                    grades.Add(int.Parse(rb.Content.ToString()));
                }
            }
            return grades;
        }

        private List<string> GetHospitalSurvey()
        {
            const string QUESTION_ONE = "1. How would you rate the investigative diagnosis process that you underwent?";
            const string QUESTION_TWO = "2. How would you rate our appointment making system?";
            const string QUESTION_THREE = "3. How do we compare to other hospitals in your area?";
            const string QUESTION_FOUR = "4. How satisfied are you with the treatments you have received?";
            const string QUESTION_FIVE = "5. How likely are you to come back to us if needed?";

            List<string> survey = new List<string>();
            survey.Add(QUESTION_ONE);
            survey.Add(QUESTION_TWO);
            survey.Add(QUESTION_THREE);
            survey.Add(QUESTION_FOUR);
            survey.Add(QUESTION_FIVE);

            return survey;
        }
    }
}
