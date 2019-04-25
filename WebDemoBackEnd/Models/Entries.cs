using System;
using System.Collections.Generic;

namespace WebDemoBackEnd.Models
{
    public partial class Entries
    {
        public int EntryId { get; set; }
        public int UserId { get; set; }
        public int FoodId { get; set; }
        public int FoodAmount { get; set; }
        public DateTime? Date { get; set; }
        public int? TimeOfDay { get; set; }
        public TimeSpan? Time { get; set; }

        public virtual Foods Food { get; set; }
        public virtual TimesOfDay TimeOfDayNavigation { get; set; }
        public virtual Users User { get; set; }
    }
}
