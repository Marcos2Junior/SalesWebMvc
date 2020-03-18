using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Data;
using SalesWebMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMvc.Services
{
    public class CategoryService
    {
        private readonly SalesWebMvcContext _context;

        public CategoryService(SalesWebMvcContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> FindAllAsync()
        {
            return await _context.Category.OrderBy(x => x.Name).ToListAsync();
        }
    }
}
