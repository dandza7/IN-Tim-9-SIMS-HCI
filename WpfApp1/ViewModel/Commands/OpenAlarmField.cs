﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfApp1.ViewModel.Commands
{
    public class OpenAlarmField : ICommand
    {
        public NotesViewModel NotesViewModel { get; set; }  
        public OpenAlarmField(NotesViewModel notesViewModel)
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
            Console.WriteLine("Parametar koji je poslan je " + parameter);
            NotesViewModel.OpenAlarmField();
        }
    }
}
