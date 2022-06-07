using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfApp1.ViewModel.Secretary;

namespace WpfApp1.ViewModel.Commands.Secretary
{
    public class ScheduleMeeting : ICommand
    {
        public CreateMeetingPageViewModel CreateMeetingViewModel { get; set; }
        public ScheduleMeeting(CreateMeetingPageViewModel createMeetingViewModel)
        {
            CreateMeetingViewModel = createMeetingViewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            DateTime beginning = (DateTime)parameter;
            Console.WriteLine(beginning);
            CreateMeetingViewModel.ScheduleMeeting(beginning);
        }
    }
}