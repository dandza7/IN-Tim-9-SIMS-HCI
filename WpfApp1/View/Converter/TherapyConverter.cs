using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1.Controller;
using WpfApp1.Model;
using WpfApp1.View.Model.Patient;

namespace WpfApp1.View.Converter
{
    internal class TherapyConverter
    {
        private static string CreateAdministrationFrequency(float frequency)
        {
            if(frequency < 1)
            {
                int administrationFrequency = (int)Math.Round(1 / frequency);
                return "Once every " + administrationFrequency + " days";
            }
            return frequency + " times a day";
        }

        public static TherapyView ConvertTherapyAndDrugToTherapyView(Therapy therapy, Drug drug)
        {
            string frequency = CreateAdministrationFrequency(therapy.Frequency);
            string duration = therapy.Duration + " days";
            TherapyView newView = new TherapyView(therapy.Id, therapy.MedicalRecordId, therapy.DrugId, frequency, duration, drug.Name);
            return newView;
        }
    }
}
