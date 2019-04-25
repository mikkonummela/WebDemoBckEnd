using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebDemoBackEnd.Models;

namespace WebDemoBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntryController : ControllerBase
    {
        //Returns all the entries for the specified user id
        //Returns an empty list if the key is invalid
        [HttpGet]
        [Route("user/{key}")]
        public List<Entries> GetEntries(string key)
        {
            if (!(int.TryParse(key, out int newkey))) return new List<Entries>();
            return (from d in (new RpkContext()).Entries where d.UserId == newkey select d).ToList();
        }

        //Adds a new entry
        //Returns 0 if there is a problem in the data
        //TimeOfDay will be nulled if invalid
        [HttpPost]
        [Route("")]
        public int AddEntry([FromBody] Entries entry)
        {
            RpkContext context = new RpkContext();
            if (context.Users.Any(u => u.UserId == entry.UserId) && context.Foods.Any(f => f.FoodId == entry.FoodId))
            {
                if (!(context.TimesOfDay.Any(t => t.TimeOfDay == entry.TimeOfDay))) entry.TimeOfDay = null;
                context.Entries.Add(entry);
                context.SaveChanges();
                return entry.EntryId;
            }
            return 0;
        }

        [HttpPut]
        [Route("{key}")]
        public int ChangeEntry(string key, [FromBody] Entries entry)
        {
            //Checks for faulty key
            if (!(int.TryParse(key, out int newkey))) return -1;
            RpkContext context = new RpkContext();
            Entries oldentry = context.Entries.Find(newkey);
            if (oldentry == null) return -1;
            //Checks whether the food was originally added by the same user trying to change it now
            if (oldentry.UserId != entry.UserId) return 0;
            //Checks whether the new food id e_ists
            if (!(context.Foods.Any(e => e.FoodId == entry.FoodId))) return 0;
            //Checks whether the food added to the entry was added by the user, or a default food
            if (context.Foods.Find(entry.FoodId).AddedUserId == null ||
                    context.Foods.Find(entry.FoodId).AddedUserId == entry.UserId)
            {
                oldentry.FoodId = entry.FoodId;
                oldentry.FoodAmount = entry.FoodAmount;
                oldentry.Date = entry.Date;
                oldentry.TimeOfDay = entry.TimeOfDay;
                oldentry.Time = entry.Time;

                context.SaveChanges();
                return 1;
            }
            return 0;

            
        }

        [HttpDelete]
        [Route("{entryid}/{userid}")]
        public int DeleteCustomer(string entryid, string userid)
        {
            //Check for the validity of our input ids
            if (!(int.TryParse(entryid, out int newentryid) && int.TryParse(userid, out int newuserid))) return -1;
            RpkContext context = new RpkContext();
            Entries entry = context.Entries.Find(newentryid);
            if (entry != null && entry.UserId == newuserid)
            {
                context.Entries.Remove(entry);
                context.SaveChanges();
                return 1;
            }
            return 0;
        }
    }
}