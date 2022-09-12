using ITPHAcademyOMAWebAPI.Models;
using ITPHAcademyOMAWebAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CSGBPanel.Web.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITPHAcademyOMAContext _context;
        private IConfiguration _config;
        private ITokenService _tokenService;


        public AuthController(ITPHAcademyOMAContext context, IConfiguration config, ITokenService tokenService)
        {
            _context = context;
            _config = config;
            _tokenService = tokenService;
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login([FromBody] UserLoginDTO user)
        {
            IActionResult response = Unauthorized();
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            var reqHash = sha256_hash(user.Password);

            User? userLogin = _context.Users.Where(u => u.Username.Equals(user.Username) && u.Password.Equals(user.Password) && !u.IsCanceled).Select(x => new User() { Id = x.Id, Username = x.Username, RoleId = x.RoleId }).FirstOrDefault();

            if (userLogin != null)
            {

                var tokenString = _tokenService.BuildToken(userLogin);
                //HttpContext.Session.SetInt32("AUNo", user.No);

                return Ok(new { token = tokenString, roleId = userLogin.RoleId });
            }
            else return response;
        }


        [HttpPost]
        [Route("Register")]
        public IActionResult Register([FromBody] UserRegisterDTO user)
        {
            IActionResult response = Unauthorized();
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}


            User? userLogin = _context.Users.Where(u => u.Username.Equals(user.Username)).FirstOrDefault();

            if (userLogin == null)
            {
                // Database kaydet sonra login ekranına yönlendir

                var tokenString = _tokenService.BuildToken(userLogin);
                //HttpContext.Session.SetInt32("AUNo", user.No);

                return Ok(new { token = tokenString });
            }
            else return response;
        }

        public static String sha256_hash(string value)
        {
            StringBuilder Sb = new StringBuilder();

            using (var hash = SHA256.Create())
            {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes(value));

                foreach (Byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }




    }
}