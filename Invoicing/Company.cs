using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoicing {
    
    [Serializable]
    public class Company {

        public string CompanyName { get; set; }
        public string PostalAddress { get; set; }
        public string PhysicalAddress { get; set; }
        public string Phone { get; set; }
        public string Cell { get; set; }
        public string Email { get; set; }
        public ExpiryDate ExpiryDate { get; set; }
        public double VAT { get; set; }

        public Company()
        {
            this.ExpiryDate = new ExpiryDate();
            VAT = 0;
        }

        public override string ToString() {
            return this.CompanyName + "\n" +
                   this.PostalAddress + "\n" +
                   this.PhysicalAddress + "\n" +
                   this.Phone + "\n" +
                   this.Cell + "\n" +
                   this.Email + "\n" +
                   this.ExpiryDate.Value;
        }
    }
}
