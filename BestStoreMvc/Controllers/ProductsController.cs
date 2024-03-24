using BestStoreMvc.Models;
using BestStoreMvc.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System;

namespace BestStoreMvc.Controllers
{
    public class ProductsController : Controller
    {

        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment environment;

        public ProductsController(ApplicationDbContext context, IWebHostEnvironment environment)
        {

            this.context = context;
            this.environment = environment;

        }
        public IActionResult Index()
        {

            var products = context.Products.ToList();
            return View();
        }

        public IActionResult Create()

        {


            return View();


        }

        [HttpPost]

        public IActionResult Create(ProductDto productDto)

        {

            if (productDto.ImageFile == null)
            {

                ModelState.AddModelError("ImageFile", "The image file is required");


            }

            if (!ModelState.IsValid)
            {
                return View(productDto);
            }

            // save the image file

            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            newFileName += Path.GetExtension(productDto.ImageFile!.FileName);

            string imageFullPath = environment.WebRootPath + "/products/" + newFileName;

            using (var stream = System.IO.File.Create(imageFullPath))
            {

                productDto.ImageFile.CopyTo(stream);

            }

            Product product = new Product()

            {

                Name = productDto.Name,
                Brand = productDto.Brand,
                Category = productDto.Category,
                Price = productDto.Price,
                Description = productDto.Description,
                ImageFileName = newFileName,
                CreatedAt = DateTime.Now,


            };

            context.Products.Add(product);
            context.SaveChanges();


            return RedirectToAction("Index", "products");


        }


        public IActionResult Edit(int id)

        {

            var product = context.Products.Find(id);


            if (product == null)

            {

                return RedirectToAction("Index", "products");
            }

            var productDto = new Product()

            {


                Name = product.Name,
                Brand = product.Brand,
                Category = product.Category,
                Price = product.Price,
                Description = product.Description,



            };

            ViewData["ProductId"] = product.Id;
            ViewData["ImageFileName"] = product.ImageFileName;
            ViewData["CreatedAt"] = product.CreatedAt.ToString("MM/dd/yyyy");


            return View(productDto);



        }


        [HttpPost]

        public IActionResult Edit(int id, ProductDto productDto)

        {

            var product = context.Products.Find(id);

            if (product == null)

            {

                return RedirectToAction("Index", "Products");
            }

            if (!ModelState.IsValid)
            {
                ViewData["ProductId"] = product.Id;
                ViewData["ImageFileName"] = product.ImageFileName;
                ViewData["CreatedAt"] = product.CreatedAt.ToString("MM/dd/yyyy");

                return View(productDto);
            }

            string newFileName = product.ImageFileName;
            if(productDto.ImageFile != null) 
            
            
        {
            newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            newFileName += Path.GetExtension(productDto.ImageFile!.FileName);

            string imageFullPath = environment.WebRootPath + "/products/" + newFileName;

            using (var stream = System.IO.File.Create(imageFullPath))
            {

                productDto.ImageFile.CopyTo(stream);

            }

            //delete the old image

            string oldImageFullPath = environment.WebRootPath + "/products/" + product.ImageFileName;
            System.IO.File.Delete(oldImageFullPath);
            
          }

            product.Name = productDto.Name;
            product.Brand = productDto.Brand;
            product.Price = productDto.Price;
            product.Category = productDto.Category;
            product.Description = productDto.Description;
            product.ImageFileName = newFileName;



            context.SaveChanges();

            return RedirectToAction("index", "Products");

           

        }


        public IActionResult Delete(int id)

        {

            var product = context.Products.Find(id);

            if(product == null) 
            {


                return RedirectToAction("Index", "Products");
            
            }

            string imageFullPath = environment.WebRootPath + "/products/" + product.ImageFileName;
            System.IO.File.Delete($"{imageFullPath}");

            context.Products.Remove(product);

            context.SaveChanges();


            return RedirectToAction("Index", "Products");

        }

    }
}
