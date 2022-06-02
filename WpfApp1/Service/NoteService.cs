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

        public IEnumerable<Note> GetPatientsNotes(int patientId)
        {
            return _noteRepository.GetPatientsNotes(patientId).ToList();
        }

        public Note Create(Note note)
        {
            return _noteRepository.Create(note);
        } 

        public Note Update(Note note)
        {
            return _noteRepository.Update(note);
        }

        public bool Delete(int id)
        {
            return _noteRepository.Delete(id);
        }
    }
}
