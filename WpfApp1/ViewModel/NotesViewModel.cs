using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Controller;
using WpfApp1.Model;
using WpfApp1.View.Dialog.PatientDialog;
using WpfApp1.View.Model.Patient;
using WpfApp1.ViewModel.Commands.Patient;

namespace WpfApp1.ViewModel
{
    public class NotesViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        private NoteController _noteController;
        private ObservableCollection<Note> _notes;
        private string _content;
        private string _alarmTime;

        public OpenAddNoteDialog OpenDialog { get; set; }
        public AddNewNote AddNote { get; set; }
        public DeleteNote Delete { get; set; }
        public OpenUpdateNoteDialog UpdateDialog { get; set; }
        public UpdateNote Update { get; set; }
        public OpenAlarmField OpenAlarm { get; set; }

        public NotesViewModel()
        {
            LoadPatientsNotes();
            OpenDialog = new OpenAddNoteDialog(this);
            AddNote = new AddNewNote(this);
            Delete = new DeleteNote(this);
            UpdateDialog = new OpenUpdateNoteDialog(this);
            Update = new UpdateNote(this);
            OpenAlarm = new OpenAlarmField(this);
        }

        public ObservableCollection<Note> Notes
        {
            get
            {
                return _notes;
            }
            set
            {
                if (value != _notes)
                {
                    _notes = value;
                    OnPropertyChanged("Notes");
                }
            }
        }

        public string Content
        {
            get
            {
                return _content;
            }
            set
            {
                if (value != _content)
                {
                    _content = value;
                    OnPropertyChanged("Content");
                }
            }
        }

        public string AlarmTime
        {
            get
            {
                return _alarmTime;
            }
            set
            {
                if (value != _alarmTime)
                {
                    _alarmTime = value;
                    OnPropertyChanged("AlarmTime");
                }
            }
        }

        private void LoadPatientsNotes()
        {
            var app = Application.Current as App;
            int patientId = (int)app.Properties["userId"];
            _noteController = app.NoteController;

            Notes = new ObservableCollection<Note>(_noteController.GetPatientsNotes(patientId));
        }

        public void OpenAddNoteDialog()
        {
            var app = Application.Current as App;
            Frame patientFrame = (Frame)app.Properties["PatientFrame"];
            patientFrame.Content = new AddNoteDialog();
        }

        public void OpenUpdateNoteDialog(int noteId)
        {
            var app = Application.Current as App;

            app.Properties["noteId"] = noteId;
            Frame patientFrame = (Frame)app.Properties["PatientFrame"];

            patientFrame.Content = new UpdateNoteDialog();
        }

        public void AddNewNote()
        {
            var app = Application.Current as App;

            int patientId = (int)app.Properties["userId"];
            string content = Content;
            string alarm = AlarmTime;
            DateTime alarmTime;
            if(alarm != null)
            {
                alarmTime = DateTime.Parse(alarm.ToString());
            } else
            {
                alarmTime = new DateTime(2030, 1, 1, 0, 0, 0);
            }
            Note note = new Note(patientId, content, alarmTime);

            _noteController = app.NoteController;
            _noteController.Create(note);

            Frame patientFrame = (Frame)app.Properties["PatientFrame"];
            patientFrame.Content = new PatientNotesView();
        }

        public void UpdateNote()
        {
            var app = Application.Current as App;

            int patientId = (int)app.Properties["userId"];
            int noteId = (int)app.Properties["noteId"];
            string content = Content;
            string alarm = AlarmTime;
            DateTime alarmTime;
            if (alarm != null)
            {
                alarmTime = DateTime.Parse(alarm.ToString());
            }
            else
            {
                alarmTime = new DateTime(2001, 1, 1, 0, 0, 0);
            }
            DateTime deletedTime = new DateTime(2030, 1, 1, 0, 0, 0);
            Note note = new Note(noteId, patientId, content, alarmTime);

            _noteController = app.NoteController;
            _noteController.Update(note);

            Frame patientFrame = (Frame)app.Properties["PatientFrame"];
            patientFrame.Content = new PatientNotesView();
        }

        public void DeleteNote(int noteId)
        {
            var app = Application.Current as App;
            _noteController = app.NoteController;

            _noteController.Delete(noteId);
            LoadPatientsNotes();
        }

        public void OpenAlarmField()
        {
            Console.WriteLine("Hello world");
        }

    }
}
