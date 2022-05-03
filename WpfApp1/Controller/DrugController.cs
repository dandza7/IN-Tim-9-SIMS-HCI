using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.Service;

namespace WpfApp1.Controller
{
    public class DrugController
    {
        private readonly DrugService _drugService;

        public DrugController(DrugService drugService)
        {
            _drugService = drugService;
        }

        internal IEnumerable<Drug> GetAll()
        {
            return _drugService.GetAll();
        }

        public Drug GetById(int id)
        {
            return _drugService.GetById(id);
        }

        public Drug Create(Drug drug)
        {
            return _drugService.Create(drug);
        }

        public Drug Update(Drug drug)
        {
            return _drugService.Update(drug);
        }

        public bool Delete(int id)
        {
            return _drugService.Delete(id);
        }
    }
}
