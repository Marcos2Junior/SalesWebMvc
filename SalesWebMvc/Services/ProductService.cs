using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Data;
using SalesWebMvc.Models;
using SalesWebMvc.Services.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SalesWebMvc.Classes;

namespace SalesWebMvc.Services
{
    public class ProductService
    {
        private readonly SalesWebMvcContext _context;

        public ProductService(SalesWebMvcContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> FindAllAsync()
        {
            return await _context.Product.Include(x => x.Category).OrderBy(x => x.Category).ThenBy(x => x.Name).ToListAsync();
        }

        public async Task InsertAsync(Product obj)
        {
            _context.Add(obj);
            await _context.SaveChangesAsync();
        }

        public async Task<Product> FindByIdAsync(int id)
        {
            return await _context.Product.Include(obj => obj.Category).FirstOrDefaultAsync(obj => obj.Id == id);
        }

        public async Task RemoveAsync(int id)
        {
            try
            {
                var obj = await _context.Product.FindAsync(id);
                _context.Product.Remove(obj);
                await _context.SaveChangesAsync();

                Utils util = new Utils();
                util.RemoveImage("products", obj.Imagem);
            }
            catch (DbUpdateException)
            {
                throw new IntegrityException("Can't delete Product because he/she has dependency");
            }
        }

        public async Task UpdateAsync(Product obj)
        {
            bool hasAny = await _context.Product.AnyAsync(x => x.Id == obj.Id);
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
