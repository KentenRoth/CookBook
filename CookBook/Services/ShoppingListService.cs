using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CookBook.Data;
using CookBook.Interfaces;
using CookBook.Models;
using Microsoft.AspNetCore.Identity;

namespace CookBook.Services
{
    public class ShoppingListService : IShoppingListService
    {
        private readonly ApplicationDBContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public ShoppingListService(ApplicationDBContext context, IMapper mapper, UserManager<AppUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }
    }
}