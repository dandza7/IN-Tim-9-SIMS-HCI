using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;

namespace WpfApp1.Repository.Interface
{
    public interface IAllergyRepository 
    {

        IEnumerable<Allergy> GetAll();

        IEnumerable<Allergy> GetPatientsAllergies(int medicalRecordId);

        Allergy GetById(int id);

        Allergy Create(Allergy allergy);

        bool Delete(int allergyId);

    }
}
