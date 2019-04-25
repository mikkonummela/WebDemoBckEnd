using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebDemoBackEnd.Models;


// Controllers for the Users table in the database
// At least initially there is no reason to get all customers
// or delete them via the program itself, so they are not implemented
namespace WebDemoBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        //Gets a user via the UserName
        //Usernames should be guaranteed to be unique in the database
        [HttpGet]
        [Route("{key}")]
        public Users GetUser(string key)
        {
            RpkContext context = new RpkContext();
            //returns null if no user found, or the single user
            //if more than one user with the same username e_ists, throws an error (should never happen)
            return context.Users.SingleOrDefault(u => u.UserName == key);
        }

        //Adds a new user to the database after checking validity
        //Returns the UserID of the added user
        //Returns -1 if there is already a user with that UserName
        [HttpPost]
        [Route("")]
        public int AddUser([FromBody] Users NewUser)
        {
            RpkContext context = new RpkContext();
            if (context.Users.Any(u => u.UserName == NewUser.UserName)) return -1;
            context.Users.Add(NewUser);
            context.SaveChanges();
            return NewUser.UserId;
        }

        //Changes the data of a user to the specified user
        //Replicate usernames are still not allowed
        //Returns 1 if the change is valid, or -1 if it isn't
        //Returns 0 if user not found (how did that happen though)
        [HttpPut]
        [Route("{key}")]
        public int ChangeUser(string key, [FromBody] Users NewData)
        {
            RpkContext context = new RpkContext();
            Users user = context.Users.Find(int.Parse(key));
            if (user == null) return 0;
            //Checks for duplicate usernames in users not the one being changed
            if (context.Users.Any(u => u.UserName == NewData.UserName && u.UserId != int.Parse(key))) return -1;
            user.UserName = NewData.UserName;
            user.Password = NewData.Password;
            user.IsDisabled = NewData.IsDisabled;
            context.SaveChanges();
            return 1;

        }
    }
}