using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfApp1.Model;

namespace WpfApp1.ViewModel.Commands.Patient
{
    public class OpenUpdateNoteDialog : ICommand
    {
        public NotesViewModel NotesViewModel { get; set; }

        public OpenUpdateNoteDialog(NotesViewModel notesViewModel)
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
            Note note = (Note)parameter;
            int noteId = note.Id;
            NotesViewModel.OpenUpdateNoteDialog(noteId);
        }
    }
}
