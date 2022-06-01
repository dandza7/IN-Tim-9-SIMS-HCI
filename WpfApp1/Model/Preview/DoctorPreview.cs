using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Model.Preview
{
    public class DoctorPreview
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DoctorPreview(int id, string name, string surname)
        {
            Id = id;
            Name = name + " " + surname;
        }
    }
}
