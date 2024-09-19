using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Claudinessa.Model
{
    public class Shipment
    {
        public class Type
        {
            public int? Id { get; set; }
            public string Name { get; set; }
            public float Value { get; set; }
        }

        public int? Id { get; set; }
        public string Name { get; set; } = String.Empty;
    }

    public class NShipment : Shipment
    {
        public int TypeId { get; set; } = 0;
    }

    public class LShipment : Shipment
    {
        [JsonPropertyOrder(int.MaxValue)]
        public Type type { get; set; } = new Type();
    }
}
