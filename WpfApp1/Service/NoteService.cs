using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.Repository;

namespace WpfApp1.Service
{
    public class NoteService
    {
        private readonly NoteRepository _noteRepository;

        public NoteService(NoteRepository noteRepo)
        {
            _noteRepository = noteRepo;
        }

        public IEnumerable<Note> GetAll()
        {
            return _noteRepository.GetAll();
        }

        public Note GetById(int id)
        {
            return _noteRepository.GetById(id); 
        }

        public IEnumerable<Note> GetPatientsNotDeletedNotes(int patientId)
        {
            List<Note> allPatientsNotes = _noteRepository.GetPatientsNotes(patientId).ToList();
            List<Note> notDeleted = new List<Note>();

            foreach (Note note in allPatientsNotes)
            {
                if (!note.IsDeleted)
                {
                    notDeleted.Add(note);
                }
            }

            return notDeleted;
        }
        public Note Create(Note note)
        {
            return _noteRepository.Create(note);
        } 

        public Note Update(Note note)
        {
            return _noteRepository.Update(note);
        } 

        public void DeleteOldLogicallyDeletedNotes(int patientId)
        {
            List<Note> allPatientsNotes = _noteRepository.GetPatientsNotes(patientId).ToList();

            foreach (Note note in allPatientsNotes)
            {
                if (note.IsDeleted && note.DeletedTime.AddDays(1) < DateTime.Now)
                {
                    _noteRepository.Delete(note.Id);
                }
            }
        }

        public bool DeleteLogically(int id)
        {
            Note note = _noteRepository.GetById(id);
            note.IsDeleted = true;
            note.DeletedTime = DateTime.Now;
            _noteRepository.Update(note);
            return true;
        }
    }
}
