using System;
using System.Collections.Generic;

namespace WebDemoBackEnd.Models
{
    public partial class TimesOfDay
    {
        public TimesOfDay()
        {
            Entries = new HashSet<Entries>();
        }

        public int TimeOfDay { get; set; }
        public string NameOfTime { get; set; }

        public virtual ICollection<Entries> Entries { get; set; }
    }
}
