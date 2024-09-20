using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Claudinessa.Model
{
    public class Item
    {
        public class Extra
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public int? Amount { get; set; }
            public float Price { get; set; }
        }

        public int Id { get; set; }
        public int IdProduct { get; set; }
        public string Name { get; set; } = string.Empty;
        public float Price { get; set; }
        public string Img { get; set; } = string.Empty;
        public int Amount { get; set; }
        public string? Comment { get; set; }
        public int? IdOrder { get; set; }
        public IEnumerable<Extra> Extras { get; set; } = Enumerable.Empty<Extra>();
    }
}
