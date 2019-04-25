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
    public class FoodsController : ControllerBase
    {
        //Returns all foods inserted by the specified user
        //Returns all foods with no inserter when '0' is given
        //Returns an empty list with an invalid key
        [HttpGet]
        [Route("user/{key}")]
        public List<Foods> GetUsers(string key)
        {
            if (int.TryParse(key, out int newkey))
            {
                if (newkey == 0) return (from d in (new RpkContext()).Foods where d.AddedUserId == null select d).ToList();
                else return (from d in (new RpkContext()).Foods where d.AddedUserId == newkey select d).ToList();
            }
            return new List<Foods>();
        }

        //Returns the ID of the added food
        //Faulty food category will simply null it, front end needs to make sure this
        //doesn't happen needlessly
        //Returns 0 if the user specified in the Foods is not found
        [HttpPost]
        [Route("")]
        public int AddFood([FromBody] Foods food)
        {
            RpkContext context = new RpkContext();
            if (context.Users.Any(u => u.UserId == food.AddedUserId))
            {
                if (!(context.FoodCategories.Any(u => u.FoodCategoryId == food.FoodCategoryId))) food.FoodCategoryId = null;
                context.Foods.Add(food);
                context.SaveChanges();
                return food.FoodId;
            }
            return 0;
        }

        //Returns -1 if the key (food id to be changed) is invalid
        //Returns 0 if the user did in food is not the same as the one in the database for the
        //FoodID specified in key
        //Faulty food category will simply null it, front end needs to make sure this
        //doesn't happen needlessly
        //Returns 1 if everything is succesful
        [HttpPut]
        [Route("{key}")]
        public int ChangeFood(string key, [FromBody] Foods food)
        {
            //Checks for faulty key
            if (!(int.TryParse(key, out int newkey))) return -1;
            RpkContext context = new RpkContext();
            Foods oldfood = context.Foods.Find(newkey);
            //Checks for whether the food was originally added by a user
            if (oldfood.AddedUserId == null) return 0;
            //Checks whether the food was originally added by the same user trying to change it now
            if (oldfood.AddedUserId != food.AddedUserId) return 0;
            oldfood.FoodName = food.FoodName;
            oldfood.Kcal = food.Kcal;
            if (!(context.FoodCategories.Any(u => u.FoodCategoryId == food.FoodCategoryId))) food.FoodCategoryId = null;
            else oldfood.FoodCategoryId = food.FoodCategoryId;
            context.SaveChanges();
            return 1;
        }

        [HttpDelete]
        [Route("{foodid}/{userid}")]
        public int DeleteCustomer(string foodid, string userid)
        {
            //Check for the validity of our input ids
            if (!(int.TryParse(foodid, out int newfoodid) && int.TryParse(userid, out int newuserid))) return -1;
            RpkContext context = new RpkContext();
            Foods food = context.Foods.Find(newfoodid);
            if (food != null && food.AddedUserId == newuserid)
            {
                context.Foods.Remove(food);
                context.SaveChanges();
                return 1;
            }
            return 0;
        }
    }
}