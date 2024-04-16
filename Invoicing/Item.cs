using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Invoicing {
    public class Item {

        public string Description { get; set; }
        public double UnitPrice { get; set; }
        public int Quantity { get; set; }

        public Item() { }

        public string ItemUnitPriceString {
            get {
                return this.UnitPrice.ToString("0.00");
            }
        }
    }
}
