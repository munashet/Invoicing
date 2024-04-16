using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Db4objects.Db4o;
using Invoicing;

namespace LicenceManager
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = Db4oEmbedded.NewConfiguration();
            config.Common.UpdateDepth = 100;
            config.Common.ActivationDepth = 100;
            IEmbeddedObjectContainer Db = Db4oEmbedded.OpenFile(config, "Licence_Up_To_2020-2-28.munlic");
            ExpiryDate ed = new ExpiryDate();
            ed.Value = new DateTime(2020, 2, 28);
            Db.Store(ed);
            Db.Close();
            Console.ReadKey();
        }
    }
}
