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
        private Note _note;
        private string _content;
        private string _alarmTime;

        public OpenAddNoteDialog OpenDialog { get; set; }
        public AddNewNote AddNote { get; set; }
        public DeleteNote Delete { get; set; }
        public OpenUpdateNoteDialog UpdateDialog { get; set; }
        public UpdateNote Update { get; set; }
        public DiscardNote Discard { get; set; }
        public NoteHelp Help { get; set; }

        public NotesViewModel()
        {
            LoadPatientsNotes();
            OpenDialog = new OpenAddNoteDialog(this);
            AddNote = new AddNewNote(this);
            Delete = new DeleteNote(this);
            UpdateDialog = new OpenUpdateNoteDialog(this);
            Update = new UpdateNote(this);
            Discard = new DiscardNote(this);
            Help = new NoteHelp(this);  
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

        public Note Note
        {
            get
            {
                return _note;
            }
            set
            {
                if (value != _note)
                {
                    _note = value;
                    OnPropertyChanged("Note");
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
            int noteId = Note.Id;
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
            Note note = new Note(noteId, patientId, content, alarmTime);

            _noteController = app.NoteController;
            _noteController.Update(note);

            Frame patientFrame = (Frame)app.Properties["PatientFrame"];
            patientFrame.Content = new PatientNotesView();
        }

        public void DeleteNote()
        {
            var app = Application.Current as App;
            _noteController = app.NoteController;

            _noteController.Delete(Note.Id);
            LoadPatientsNotes();
        }

        public void DiscardNote()
        {
            var app = Application.Current as App;
            Frame patientFrame = (Frame)app.Properties["PatientFrame"];
            patientFrame.Content = new PatientNotesView();
        }

        public void NoteHelp()
        {
            const string ADD_NOTE_HELP_CONTENT = "Notes are a convenient way for you to remind yourself of something inside of " +
                "the application. If you need an extra reminder you can set an alarm which will go off and new notification will " +
                "arive for you to make sure you do not forget what you wrote in your note. Just write the note down and then set alarm " +
                "if you would like and when. Then click confirm in order to add the new note.";

            PatientHelp.Show(ADD_NOTE_HELP_CONTENT);
        }
    }
}
