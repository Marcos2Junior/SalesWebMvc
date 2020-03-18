using System.Collections.Generic;

namespace SalesWebMvc.Models
{
    public class ProductFormViewModel
    {
        public Product Product { get; set; }
        public ICollection<Category> Categories { get; set; }
    }
}
