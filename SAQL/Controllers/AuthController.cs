using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAQL.Contexts;
using SAQL.Entities;

namespace SAQL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly SAQLContext _context;
        public AuthController(SAQLContext context)
        {
            _context = context;
        }
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            var role = LoginLogic(model);

            if (role.HasValue)
            {
                if (role == Role.Doctor)
                {
                    return Ok(new { Message = "Login successful as Doctor", Role = "Doctor" });
                }
                else if (role == Role.Patron)
                {
                    return Ok(new { Message = "Login successful as Patron", Role = "Patron" });
                }
            }

            return BadRequest(new { Message = "Login failed" });
        }

        private Role? LoginLogic(LoginModel model)
        {
            var doctor = _context.Doctors.FirstOrDefault(d => d.PhoneNumber == model.PhoneNumber && d.Password == model.Password);
            var patron = _context.Patrons.FirstOrDefault(p => p.PhoneNumber == model.PhoneNumber && p.Password == model.Password);
            
            if (doctor != null)
            {
                return Role.Doctor;
            }
            else if (patron != null)
            {
                return Role.Patron;
            }

            return null;
        }
    }
    public class LoginModel
    {
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
    }
}
