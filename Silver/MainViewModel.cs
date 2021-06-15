using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Karambolo.PO;
using System.IO;

namespace Silver
{
    public class MainViewModel
    {

        public void Initialize()
        {

        }

        public void AddPoFile()
        {
            var filePath = "C:\\users\\pgpoulsen\\Downloads\\Locale\\da-DK\\LC_MESSAGES\\ShopFloorManagementSuite.po";

            var parser = new POParser(new POParserSettings
            {
                // parser options...
            });

            var reader = File.OpenText(filePath);
            var result = parser.Parse(reader);

            var key = result.Catalog.Keys.First();
            var value = result.Catalog[key];
            var v = value[0];
            var plurals = result.Catalog.Keys.Where(x => x.PluralId != null).ToList();
        }
    }
}
