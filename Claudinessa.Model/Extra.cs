using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Claudinessa.Model
{
    public class Extra
    {
        public int? Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public float Price { get; set; }
        public bool IsAvailable { get; set; }
        
    }
    public class NExtra : Extra {
        public int Category { get; set; }
    }
}
