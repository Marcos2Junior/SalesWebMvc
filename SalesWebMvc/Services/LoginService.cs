using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Data;
using SalesWebMvc.Models;
using SalesWebMvc.Services.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMvc.Services
{
    public class LoginService
    {
        private readonly SalesWebMvcContext _context;

        public LoginService(SalesWebMvcContext context)
        {
            _context = context;
        }

        public async Task<List<Login>> FindAllAsync()
        {
            return await _context.Login.ToListAsync();
        }

        public async Task InsertAsync(Login obj)
        {
            _context.Add(obj);
            await _context.SaveChangesAsync();
        }

        public async Task<Login> FindByIdAsync(int id)
        {
            return await _context.Login.FirstOrDefaultAsync(obj => obj.Id == id);
        }

        public async Task RemoveAsync(int id)
        {
            try
            {
                var obj = await _context.Login.FindAsync(id);
                _context.Login.Remove(obj);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new IntegrityException("Can't delete Login");
            }
        }

        public async Task UpdateAsync(Login obj)
        {
            bool hasAny = await _context.Login.AnyAsync(x => x.Id == obj.Id);
            if (!hasAny)
            {
                throw new NotFoundException("Id not found");
            }

            try
            {
                _context.Update(obj);
                await _context.SaveChangesAsync();
            }
            //Possivel exceção de concorrencia do banco de dados
            catch (DbUpdateConcurrencyException e)
            {
                /*
                 * --Importante para segregar as camadas--
                 * Interceptando uma exceção do nivel de acesso a dados e relançando essa mesma exceção
                 * só que em nível de serviço.
                 * A minha camada de serviço, não irá propagar uma exceção de acesso a dados, pois caso aconteça
                 * ela lançara uma exceção da camada dela mesmo.
                 * Sendo assim, o controlador apenas irá lidar com a exceção da camada de serviço.
                */
                throw new DbConcurrencyException(e.Message);
            }
        }
    }
}
