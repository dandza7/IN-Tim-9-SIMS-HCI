using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.Service;

namespace WpfApp1.Controller
{
    public class NoteController
    {
        private readonly NoteService _noteService;

        public NoteController(NoteService noteService)
        {
            _noteService = noteService;
        }

        public IEnumerable<Note> GetAll()
        {
            return _noteService.GetAll();
        }

        public Note GetById(int id)
        {
            return _noteService.GetById(id);
        }

        public IEnumerable<Note> GetPatientsNotDeletedNotes(int patientId)
        {
            return _noteService.GetPatientsNotDeletedNotes(patientId);
        }

        public Note Create(Note note)
        {
            return _noteService.Create(note);
        }

        public Note Update(Note note)
        {
            return _noteService.Update(note);
        }

        public void DeleteOldLogicallyDeletedNotes(int patientId)
        {
            _noteService.DeleteOldLogicallyDeletedNotes(patientId);
        }

        public bool DeleteLogically(int id)
        {
            return _noteService.DeleteLogically(id);
        }
    }
}
