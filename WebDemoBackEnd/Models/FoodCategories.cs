using System;
using System.Collections.Generic;

namespace WebDemoBackEnd.Models
{
    public partial class FoodCategories
    {
        public FoodCategories()
        {
            Foods = new HashSet<Foods>();
        }

        public int FoodCategoryId { get; set; }
        public string FoodCategoryName { get; set; }

        public virtual ICollection<Foods> Foods { get; set; }
    }
}
