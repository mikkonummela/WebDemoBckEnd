using System;
using System.Collections.Generic;

namespace WebDemoBackEnd.Models
{
    public partial class Foods
    {
        public Foods()
        {
            Entries = new HashSet<Entries>();
        }

        public int FoodId { get; set; }
        public string FoodName { get; set; }
        public int? Kcal { get; set; }
        public int? FoodCategoryId { get; set; }
        public int? AddedUserId { get; set; }

        public virtual Users AddedUser { get; set; }
        public virtual FoodCategories FoodCategory { get; set; }
        public virtual ICollection<Entries> Entries { get; set; }
    }
}
