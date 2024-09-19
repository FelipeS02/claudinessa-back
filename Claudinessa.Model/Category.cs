using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Claudinessa.Model
{
    public class Category
    {
        [JsonPropertyOrder(int.MinValue)]
        public int? Id { get; set; }

        [JsonPropertyOrder(-1)]
        public string Name { get; set; } = String.Empty;
    }

    public class LExtrasCategory : Category
    {
        public int Min { get; set; }
        public int Max { get; set; }
        public bool? IsOptional { get; set; }
        public bool? IsQuantifiable { get; set; }
        public IEnumerable<Extra>? Extras { get; set; } = Enumerable.Empty<Extra>();
    }

    public class LProductCategory : Category
    {
        public IEnumerable<LProduct>? Products { get; set; } = Enumerable.Empty<LProduct>();
    }

    public class NExtrasCategory : Category
    {
        public int Min { get; set; } = int.MinValue;
        public int Max { get; set; } = int.MinValue;
        public bool? IsOptional { get; set; }
        public bool? IsQuantifiable { get; set; }
        public IEnumerable<NExtra>? Extras { get; set; } = Enumerable.Empty<NExtra>();
    }

    public class NProductCategory : Category
    {
        public IEnumerable<NProduct>? Products { get; set; } = Enumerable.Empty<NProduct>();
    }
}
