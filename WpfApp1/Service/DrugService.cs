using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.Repository;
using WpfApp1.Repository.Interface;

namespace WpfApp1.Service
{
    public class DrugService
    {
        private readonly IDrugRepository _drugRepo;

        public DrugService(IDrugRepository drugRepo)
        {
            _drugRepo = drugRepo;
        }

        public IEnumerable<Drug> GetAll()
        {
            return _drugRepo.GetAll();
        }

        public Drug GetById(int id)
        {
            return _drugRepo.GetById(id);
        }

        public Drug Create(Drug drug)
        {
            return _drugRepo.Create(drug);
        }

        public Drug Update(Drug drug)
        {
            return _drugRepo.Update(drug);
        }

        public bool Delete(int id)
        {
            return _drugRepo.Delete(id);
        }
    }
}
