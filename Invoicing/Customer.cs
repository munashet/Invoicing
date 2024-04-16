using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Invoicing {
    public class Customer {

        public const string INDIVIDUAL = "Individual";
        public const string COMPANY = "Company";
        public const string CHURCH = "Church";
        public const string SCHOOL = "School";
        public const string NGO = "NGO";
        public string Name { get; set; }
        public string CustomerType { get; set; }
        public string ContactPerson { get; set; }
        public string PostalAddress { get; set; }
        public string PhysicalAddress { get; set; }
        public string Phone { get; set; }
        public string Cell { get; set; }
        public string Email { get; set; }
        public List<Order> Orders { get; set; }
        public Company Company { get; set; }

        public Customer(Company company) {
            Company = company;
            Orders = new List<Order>();
        }

        public double Balance
        {
            get
            {
                var paid = Orders.Where(i => i.Receipt != null);
                var unPaid = Orders.Where(i => i.Invoice != null);
                var paidAmount = paid.Sum(i => i.Items.Sum(x => x.Quantity * x.UnitPrice));
                var unPaidAmount = unPaid.Sum(i => i.Items.Sum(x => x.Quantity * x.UnitPrice));
                paidAmount += ((Company.VAT / 100) * paidAmount);
                unPaidAmount += ((Company.VAT / 100) * unPaidAmount);
                return paidAmount - unPaidAmount;
            }
        }
    }
}
