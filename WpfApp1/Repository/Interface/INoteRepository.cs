using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;

namespace WpfApp1.Repository.Interface
{
    public interface INoteRepository
    {
        IEnumerable<Note> GetAll();

        Note GetById(int id);

        IEnumerable<Note> GetPatientsNotes(int patientId);

        Note Create(Note note);

        Note Update(Note note);

        bool Delete(int id);
    }
}
