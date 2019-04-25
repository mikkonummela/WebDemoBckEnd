using System;
using System.Collections.Generic;

namespace WebDemoBackEnd.Models
{
    public partial class Users
    {
        public Users()
        {
            Entries = new HashSet<Entries>();
            Foods = new HashSet<Foods>();
        }

        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool? IsDisabled { get; set; }

        public virtual ICollection<Entries> Entries { get; set; }
        public virtual ICollection<Foods> Foods { get; set; }
    }
}
