using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfApp1.ViewModel.Commands.Patient
{
    public class UpdateNote : ICommand
    {
        public NotesViewModel NotesViewModel { get; set; }
        public UpdateNote(NotesViewModel notesViewModel)
        {
            NotesViewModel = notesViewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            NotesViewModel.UpdateNote();
        }
    }
}
