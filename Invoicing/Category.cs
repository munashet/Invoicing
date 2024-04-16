using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Invoicing {
    public class Category {

        public string CategoryName { get; set; }
        public string Description { get; set; }

        public Category(){}

        public override string ToString() {
            return CategoryName;
        }
    }
}
