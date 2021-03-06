﻿using SalesWebMvc.Data;
using SalesWebMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace SalesWebMvc.Services
{
    public class SalesRecordService
    {
        private readonly SalesWebMvcContext _context;

        public SalesRecordService(SalesWebMvcContext context)
        {
            _context = context;
        }

        public async Task<List<SalesRecord>> FindByDateAsync(DateTime? minDate, DateTime? maxDate)
        {
            var result = from obj in _context.SalesRecord select obj;

            if (minDate.HasValue)
            {
                result = result.Where(x => x.Date >= minDate.Value).Where(x => x.Status != Models.Enums.SaleStatus.Canceled);
            }

            if (maxDate.HasValue)
            {
                result = result.Where(x => x.Date <= maxDate.Value).Where(x => x.Status != Models.Enums.SaleStatus.Canceled);
            }

            return await result
                .Include(x => x.Seller)
                .Include(x => x.Seller.Department)  //Include realiza o join com as tabelas
                .OrderByDescending(x => x.Date)     //Ordena em ordem decrescente
                .ToListAsync();
        }

        public async Task<List<IGrouping<Department, SalesRecord>>> FindByDateGroupingAsync(DateTime? minDate, DateTime? maxDate)
        {
            var result = from obj in _context.SalesRecord select obj;

            if (minDate.HasValue)
            {
                result = result.Where(x => x.Date >= minDate.Value).Where(x => x.Status != Models.Enums.SaleStatus.Canceled);
            }

            if (maxDate.HasValue)
            {
                result = result.Where(x => x.Date <= maxDate.Value).Where(x => x.Status != Models.Enums.SaleStatus.Canceled);
            }

            return await result
                .Include(x => x.Seller)
                .Include(x => x.Seller.Department)  //Include realiza o join com as tabelas
                .OrderByDescending(x => x.Date)     //Ordena em ordem decrescente
                .GroupBy(x => x.Seller.Department)
                .ToListAsync();
        }
    }
}
