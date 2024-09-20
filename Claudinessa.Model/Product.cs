using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Claudinessa.Model
{
    public class Product
    {
        [JsonPropertyOrder(int.MinValue)]
        public int? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public IEnumerable<Option> Options { get; set; } = Enumerable.Empty<Option>();
        public string? Description { get; set; }
        public string? Img { get; set; }
        public bool IsAvailable { get; set; }
        public bool HasOptions { get; set; }
        public bool IsOnDiscount { get; set; }
    }

    public class NProduct : Product
    {
        public int Category { get; set; }
    }

    public class LProduct : Product
    {
        [JsonPropertyOrder(int.MaxValue)]
        public IEnumerable<LExtrasCategory>? Extras { get; set; }
        public int Purchases { get; set; }
    }
}
