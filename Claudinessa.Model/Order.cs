using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Claudinessa.Model
{
    public class Order
    {
        public int? Id { get; set; }

        public float Price { get; set; } = 0;
        public float Shipment { get; set; } = 0;

        public string Phone { get; set; } = String.Empty;
        public string Client { get; set; } = String.Empty;

        public string Adress { get; set; } = String.Empty;
        public string HouseNumber { get; set; } = String.Empty;
        public string? Neighborhood { get; set; }
        public string? Complement { get; set; }
        public string? Instructions { get; set; }
        public int State { get; set; } = 0;

        public int Method { get; set; } = 0;

        public int Service { get; set; } = 0;
        public DateTime? Created { get; set; }
        public IEnumerable<Item>? Products { get; set; }
    }
}
