using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fiorello.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDeactive { get; set; }
        public List<Product> Products { get; set; }

    }
}
