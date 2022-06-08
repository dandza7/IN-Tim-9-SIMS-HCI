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
    public class NoteRepository : INoteRepository
    {
        private string _path;
        private string _delimiter;
        private readonly string _datetimeFormat;

        public NoteRepository(string path, string delimiter, string datetimeFormat)
        {
            _path = path;
            _delimiter = delimiter;
            _datetimeFormat = datetimeFormat;
        }

        private int GetMaxId(IEnumerable<Note> notes)
        {
            return notes.Count() == 0 ? 0 : notes.Max(note => note.Id);
        }

        public IEnumerable<Note> GetAll()
        {
            List<string> lines = File.ReadAllLines(_path).ToList();
            List<Note> notes = new List<Note>();
            foreach (string line in lines)
            {
                if (line == "") continue;
                notes.Add(ConvertCSVFormatToNote(line));
            }
            return notes;
        }

        public Note GetById(int id)
        {
            return GetAll().ToList().FirstOrDefault(note => note.Id == id);
        }

        public IEnumerable<Note> GetPatientsNotes(int patientId)
        {
            List<Note> allNotes = GetAll().ToList();
            List<Note> patientsNotes = new List<Note>();
            foreach(Note n in allNotes)
            {
                if(n.PatientId == patientId)
                {
                    patientsNotes.Add(n);
                }
            }
            return patientsNotes;
        }

        public Note Create(Note note)
        {
            int maxId = GetMaxId(GetAll());
            note.Id = ++maxId;
            AppendLineToFile(_path, ConvertNoteToCSVFormat(note));
            return note;
        }

        public Note Update(Note note)
        {
            List<Note> notes = GetAll().ToList();
            List<string> newFile = new List<string>();
            foreach (Note n in notes)
            {

                if (n.Id == note.Id)
                {
                    n.PatientId = note.PatientId;
                    n.Content = note.Content;
                    n.AlarmTime = note.AlarmTime;
                }
                newFile.Add(ConvertNoteToCSVFormat(n));
            }
            File.WriteAllLines(_path, newFile);
            return note;
        }

        public bool Delete(int id)
        {
            List<Note> notes = GetAll().ToList();
            List<string> newFile = new List<string>();
            bool isDeleted = false;
            foreach (Note n in notes)
            {
                if (n.Id != id)
                {
                    newFile.Add(ConvertNoteToCSVFormat(n));
                    isDeleted = true;
                }
            }
            File.WriteAllLines(_path, newFile);
            return isDeleted;
        }

        private Note ConvertCSVFormatToNote(string noteCSVFormat)
        {
            var tokens = noteCSVFormat.Split(_delimiter.ToCharArray());
            return new Note(int.Parse(tokens[0]),
                int.Parse(tokens[1]),
                tokens[2],
                DateTime.Parse(tokens[3]));
        }

        private string ConvertNoteToCSVFormat(Note note)
        {
            return string.Join(_delimiter,
                note.Id,
                note.PatientId,
                note.Content,
                note.AlarmTime.ToString(_datetimeFormat));
        }

        private void AppendLineToFile(string path, string line)
        {
            File.AppendAllText(path, line + Environment.NewLine);
        }
    }
}
