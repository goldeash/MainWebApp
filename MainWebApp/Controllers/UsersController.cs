﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using UserManagementApp.Data;
using UserManagementApp.Models;

namespace UserManagementApp.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _context.Users.OrderBy(u => u.LastLoginTime).ToListAsync();
            return View(users);
        }

        [HttpPost]  
        public async Task<IActionResult> Block(string[] userIds)
        {
            if (userIds != null && userIds.Any())
            {
                var users = await _context.Users.Where(u => userIds.Contains(u.Id)).ToListAsync();
                foreach (var user in users)
                {
                    user.IsActive = false;
                }
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Unblock(string[] userIds)
        {
            if (userIds != null && userIds.Any())
            {
                var users = await _context.Users.Where(u => userIds.Contains(u.Id)).ToListAsync();
                foreach (var user in users)
                {
                    user.IsActive = true;
                }
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string[] userIds)
        {
            if (userIds != null && userIds.Any())
            {
                var users = await _context.Users.Where(u => userIds.Contains(u.Id)).ToListAsync();
                _context.Users.RemoveRange(users);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}