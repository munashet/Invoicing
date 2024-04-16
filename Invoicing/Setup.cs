using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoicing {
    public class Setup {
        private long invoiceNumber;
        private long quotationNumber;
        private long receiptNumber;

        public Setup() {
            invoiceNumber = 0;
            quotationNumber = 0;
            receiptNumber = 0;
        }

        public long GetNextInvoiceNumber() {
            ++invoiceNumber;
            return invoiceNumber;
        }

        public long GetNextQuotationNumber() {
            ++quotationNumber;
            return quotationNumber;
        }

        public long GetNextrReceiptNumber() {
            ++receiptNumber;
            return receiptNumber;
        }
    }
}
