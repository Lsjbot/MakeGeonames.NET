using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakeGeonames
{
    public class langclass
    {
        public string iso3 = "";
        public string iso2 = "";
        public Dictionary<string, string> name = new Dictionary<string, string>(); //name of language in different languages. Iso -> name.

    }
}
