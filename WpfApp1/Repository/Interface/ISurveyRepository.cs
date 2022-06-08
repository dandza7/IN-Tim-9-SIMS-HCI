using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;

namespace WpfApp1.Repository.Interface
{
    public interface ISurveyRepository
    {
        IEnumerable<Survey> GetAll();

        Survey GetById(int id);

        Survey Create(Survey survey);

        bool Delete(int id);
    }
}
