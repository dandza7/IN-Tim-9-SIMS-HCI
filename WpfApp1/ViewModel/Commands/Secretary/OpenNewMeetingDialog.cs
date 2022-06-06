using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfApp1.ViewModel.Secretary;

namespace WpfApp1.ViewModel.Commands.Secretary
{
    public class OpenNewMeetingDialog : ICommand
    {
        public MeetingViewModel MeetingViewModel { get; set; }

        public OpenNewMeetingDialog(MeetingViewModel meetingViewModel)
        {
            MeetingViewModel = meetingViewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            MeetingViewModel.OpenAddNewMeeting();
        }
    }
}
