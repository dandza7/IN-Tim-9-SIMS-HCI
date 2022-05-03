using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.View.Model.Secretary;

namespace WpfApp1.View.Converter
{
    internal class AllergyConverter : AbstractConverter
    {
        public static AllergyView ConvertAllergyToAllergyView(Allergy allergy)
        => new AllergyView
        {
            Id = allergy.Id,
            AllergyName = allergy.AllergyName
        
        };

        public static IList<AllergyView> ConvertAllergyListToAllergyViewList(IList<Allergy> allergies)
            => ConvertEntityListToViewList(allergies, ConvertAllergyToAllergyView);
    
   
    }
}
