using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Claudinessa.Model
{
    public class Option
    // Precio de las opciones (Ej: Porcion grande, Porcion chica)
    {
        public int? Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public float Price { get; set; } = 0;
        public float OffPrice { get; set; } = 0;
        public bool IsDefault { get; set; } = false;
        public int? ProductId { get; set; }
    }
}
