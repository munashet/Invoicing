using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace Invoicing {
    public abstract class Document {

        public DateTime Date { get; set; }
        public string Notes { get; set; }
        public Order Order { get; set; }
        public long DocumentNumber { get; set; }
        public double VAT { get; set; }

        public Document(Order order, double vat) {
            Order = order;
            VAT = vat;
        }

        public string DocumentNumberString {
            get {
                return GetPrefix() + DocumentNumber.ToString().PadLeft(6, '0');
            }
        }

        public abstract string GetPrefix();
        public abstract string GetTitle();
        public abstract FlowDocument GetDocument();
    }
}
