using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Invoicing {
    public class Order {

        public DateTime Date { get; set; }
        public Quotation Quotation { get; set; }
        public Invoice Invoice { get; set; }
        public Receipt Receipt { get; set; }
        public Customer Customer { get; set; }
        public List<Item> Items { get; set; }
        public double VAT { get; set; }

        public Order(Customer customer, double vat) {
            Customer = customer;
            Items = new List<Item>();
            Date = DateTime.Now;
            VAT = vat;
        }

        public string OrderDateString {
            get {
                return this.Date.ToString("dd MMM yyyy");
            }
        }

        public double Amount
        {
            get
            {
                var amount = Items.Sum(i => (i.Quantity * i.UnitPrice));
                return amount += ((VAT/100) * amount);
            }
        }

        public string Paid
        {
            get
            {
                string s = string.Empty;
                if (Invoice != null)
                {
                    if (Invoice.Paid == true)
                        s = "Yes";
                    else
                        s = "No";
                }
                else
                {
                    s = "No";
                }
                return s;
            }
        }
    }
}
