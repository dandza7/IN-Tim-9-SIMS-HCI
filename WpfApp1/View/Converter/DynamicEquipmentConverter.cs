using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.View.Model.Secretary;

namespace WpfApp1.View.Converter
{
    internal class DynamicEquipmentConverter : AbstractConverter
    {
        public static DynamicEquipmentView ConvertDynEqToDynEqView(Inventory inv)
        => new DynamicEquipmentView
        {
            Id = inv.Id,
            EquipmentName = inv.Name,
            Amount = inv.Amount

        };

        public static IList<DynamicEquipmentView> ConvertDynEqListToDynEqViewList(IList<Inventory> inv)
            => ConvertEntityListToViewList(inv, ConvertDynEqToDynEqView);


    }
}