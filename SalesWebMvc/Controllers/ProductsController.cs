using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Models;
using SalesWebMvc.Services;
using SalesWebMvc.Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SalesWebMvc.Controllers
{
    public class ProductsController : Controller
    {
        private readonly CategoryService _categoryService;
        private readonly ProductService _productService;

        public ProductsController(CategoryService categoryService, ProductService productService)
        {
            _categoryService = categoryService;
            _productService = productService;
        }

        // GET: Products
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var list = await _productService.FindAllAsync();
            return View(list);
        }

        // GET: Products/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provide" });
            }

            var obj = await _productService.FindByIdAsync(id.Value);

            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }

            return View(obj);
        }

        // GET: Products/Create
        [Authorize]
        public async Task<IActionResult> Create()
        {
            var categories = await _categoryService.FindAllAsync();
            var viewModel = new ProductFormViewModel { Categories = categories };
            return View(viewModel);
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                var categories = await _categoryService.FindAllAsync();
                var viewModel = new ProductFormViewModel { Categories = categories, Product = product };
                return View(viewModel);
            }

            product.Imagem = UploadImage();

            await _productService.InsertAsync(product);
            return RedirectToAction(nameof(Index));
        }

        // GET: Products/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provide" });
            }

            var obj = await _productService.FindByIdAsync(id.Value);

            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }

            List<Category> categories = await _categoryService.FindAllAsync();
            ProductFormViewModel viewModel = new ProductFormViewModel { Categories = categories, Product = obj };

            return View(viewModel);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (!ModelState.IsValid)
            {
                var categories = await _categoryService.FindAllAsync();
                var viewModel = new ProductFormViewModel { Product = product, Categories = categories };
                return View(viewModel);
            }

            if (id != product.Id)
            {
                return RedirectToAction(nameof(Error), new { message = "Id ismatch" });
            }

            try
            {
                product.Imagem = UploadImage();

                await _productService.UpdateAsync(product);
                return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException ex)
            {
                return RedirectToAction(nameof(Error), new { message = ex.Message });
            }
        }

        // GET: Products/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provide" });
            }

            var obj = await _productService.FindByIdAsync(id.Value);

            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }

            return View(obj);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _productService.RemoveAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (IntegrityException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }

        public IActionResult Error(string message)
        {
            var viewModel = new ErrorViewModel
            {
                Message = message,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };

            return View(viewModel);
        }

        private string UploadImage()
        {
            var newFileName = string.Empty;

            if (HttpContext.Request.Form.Files != null)
            {
                var fileName = string.Empty;

                var files = HttpContext.Request.Form.Files;

                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');

                        var myUniqueFileName = Convert.ToString(Guid.NewGuid());

                        var FileExtension = Path.GetExtension(fileName);

                        newFileName = myUniqueFileName + FileExtension;

                        fileName = Path.Combine(@"C:\Users\marcos.junior\Documents\ProjetoWeb\SalesWebMvc\SalesWebMvc\wwwroot", "images\\products") + $@"\{newFileName}";

                        try
                        {
                            using (FileStream fs = System.IO.File.Create(fileName))
                            {
                                file.CopyTo(fs);
                                fs.Flush();
                            }

                            return newFileName;

                        }
                        catch (Exception)
                        {
                            return string.Empty;
                        }
                    }
                }
            }

            return string.Empty;
        }
    }
}
