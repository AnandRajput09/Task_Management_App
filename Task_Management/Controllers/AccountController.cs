using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Task_Management.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace Task_Management.Controllers
{
    public class AccountController : Controller
    {
        private TaskAppEntities db = new TaskAppEntities();

        // GET: Account/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        public ActionResult Login(string username, string password, string role)
        {
            var key = Convert.FromBase64String("TXlWZXJ5U2VjcmV0S2V5VGhhdElzTG9uZ0Vub3VnaDEyMyE="); // Base64-encoded key

            // Static admin login logic
            if (username == "admin" && password == "admin" && role == "Admin")
            {
                SetSessionAndToken(new User { UserId = 0, Username = "Admin", Role = "Admin" }, key);
                return RedirectToAction("Index", "Task");
            }

            // Regular user login logic
            var user = db.Users.FirstOrDefault(u => u.Username == username && u.Password == password && u.Role == role);
            if (user != null)
            {
                SetSessionAndToken(user, key);
                return RedirectToAction("Index", "Task");
            }

            ViewBag.ErrorMessage = "Invalid username or password.";
            return View();
        }

        // Helper function to set session and token
        private void SetSessionAndToken(User user, byte[] key)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            TempData["Token"] = tokenString;
            Session["UserId"] = user.UserId;
            Session["Username"] = user.Username;
            Session["Role"] = user.Role;

            FormsAuthentication.SetAuthCookie(user.Username, false);
        }



        //public ActionResult Login(string username, string password, string role)
        //{
        //    var user = db.Users.FirstOrDefault(u => u.Username == username && u.Password == password && u.Role == role);

        //    if (user != null)
        //    {
        //        // Generate JWT token
        //        var tokenHandler = new JwtSecurityTokenHandler();
        //        var key = Convert.FromBase64String("TXlWZXJ5U2VjcmV0S2V5VGhhdElzTG9uZ0Vub3VnaDEyMyE="); // Base64-encoded key

        //        var tokenDescriptor = new SecurityTokenDescriptor
        //        {
        //            Subject = new ClaimsIdentity(new[]
        //            {
        //                new Claim(ClaimTypes.Name, user.Username),
        //                new Claim(ClaimTypes.Role, user.Role) // Set the user's role as a claim
        //            }),
        //            Expires = DateTime.UtcNow.AddHours(1),
        //            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        //        };

        //        var token = tokenHandler.CreateToken(tokenDescriptor);
        //        var tokenString = tokenHandler.WriteToken(token);

        //        // Store token and other info in session for use across the application
        //        TempData["Token"] = tokenString;
        //        Session["UserId"] = user.UserId;
        //        Session["Username"] = user.Username;
        //        Session["Role"] = user.Role;

        //        // Set the authentication cookie
        //        FormsAuthentication.SetAuthCookie(user.Username, false);

        //        return RedirectToAction("Index", "Task");
        //    }

        //    ViewBag.ErrorMessage = "Invalid username or password.";
        //    return View();
        //}



        public ActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        public ActionResult Register(User newUser)
        {
            // Check if username already exists
            if (db.Users.Any(u => u.Username == newUser.Username))
            {
                ViewBag.ErrorMessage = "Username already exists.";
                return View();
            }

            if (string.IsNullOrEmpty(newUser.Role))
            {
                newUser.Role = "User"; // Default role
            }

            // newUser.Password = HashPassword(newUser.Password);

            db.Users.Add(newUser); // Add user to database
            db.SaveChanges(); // Save changes

            ViewBag.SuccessMessage = "Registration successful. Please login.";
            return RedirectToAction("Login"); // Redirect to Login page
        }

        //private string HashPassword(string password)
        //{
        //    // Implement a hashing function here, like SHA256
        //    using (var sha = new System.Security.Cryptography.SHA256Managed())
        //    {
        //        byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);
        //        byte[] hashedBytes = sha.ComputeHash(passwordBytes);
        //        return Convert.ToBase64String(hashedBytes);
        //    }
        //}


        // GET: Account/UpdateProfile
        [HttpGet]
        [Authorize]
        public ActionResult UpdateProfile()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var userId = Convert.ToInt32(Session["UserId"]);
            var user = db.Users.Find(userId);

            if (user == null)
            {
                return RedirectToAction("Login");
            }

            return View(user);
        }

        [HttpPost]
        public ActionResult UpdateProfile(User updatedUser)
        {
            if (ModelState.IsValid)
            {
                int userId = Convert.ToInt32(Session["UserId"]);
                var user = db.Users.Find(userId);

                if (user != null)
                {
                    user.Password = updatedUser.Password;

                    db.SaveChanges();
                    ViewBag.SuccessMessage = "Profile updated successfully.";
                }
                else
                {
                    ViewBag.ErrorMessage = "User not found.";
                }
            }
            else
            {
                ViewBag.ErrorMessage = "Please correct the errors in the form.";
            }

            return View(updatedUser);
        }


        // GET: Account/Logout
        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            FormsAuthentication.SignOut(); // Sign out the user

            return RedirectToAction("Index","Task");
        }
    }
}