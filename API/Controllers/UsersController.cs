using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;
        public UsersController(DataContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers(){
            return await _context.Users.ToListAsync(); //Users :ten cua database
                                                //ToList(): sap xep theo danh sach
            
        } 
        // ham tim kiem id cua user
        [HttpGet("{Id}")] // truyen tham so id cua users
        public async Task<ActionResult <AppUser>> GetUsers(int id){
            return await _context.Users.FindAsync(id); //Users :ten cua database
                                                //ToList(): sap xep theo danh sach
        } 
    }
}