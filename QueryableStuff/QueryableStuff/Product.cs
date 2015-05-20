using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryableStuff
{
    public class Product
    {
        readonly List<string> _categories;

        public Product()
        {
            _categories = new List<string>();
        }

        public string Name { get; set; }

        public List<string> Categories { get { return _categories; } }

        public int Price { get; set; }


    }
}
