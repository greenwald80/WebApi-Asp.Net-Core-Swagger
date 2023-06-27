using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using WebApi_Asp.Net_Core_Swagger.Models;

namespace WebApi_Asp.Net_Core_Swagger.Controllers
{
    [Route("/api/[controller]")]//https://localhost:44336/api/products
    public class ProductsController : Controller
    {
        //comment for swagger like https://localhost:44336/swagger/index.html
        //public IActionResult Index()
        //{
        //    return View();
        //}

        private static List<Product> products = new List<Product>(new[]
        {
    new Product() {Id=1,Name="Notebook",Price=1000},
    new Product() {Id=2,Name="Car",Price=2000},
    new Product() {Id=3,Name="Apple",Price=50},
        });

        [HttpGet]
        public IEnumerable<Product> Get() => products;

        [HttpGet("{id}")]///api/Products/2
        public IActionResult Get(int id)
        {
            var product = products.SingleOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();//404
            }
            return Ok(product);
            //curl -X GET https://localhost:44336/api/products
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var product = products.SingleOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();//404
            }
            products.Remove(products.SingleOrDefault(p => p.Id == id));
            return Ok(new { Message = "Product with ID=" + product.Id + " deleted successfully" });
            //if no swagger use CURL in terminal CMD: 
            //curl -X DELETE https://localhost:44336/api/products/1
        }

        //create id for new product
        private int NextProductId => products.Count() == 0 ? 1 : products.Max(x => x.Id) + 1;
        [HttpGet("GetNextProductId")]
        public int GetNextProductId()
        {
            return NextProductId;
        }

        [HttpPost]//parameters
        public IActionResult Post(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            product.Id = NextProductId;
            products.Add(product);
            return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
            //for CURL form fields
            //curl -X POST -F Name=someName Price=123 https://localhost:44336/api/products/
            //or as JSON object
            //curl -X POST -H "Content-Type:application/json" -d "{\"name\":\"JsonName\",\"price\":123}" https://localhost:44336/api/products/AddProduct
        }

        [HttpPost("AddProduct")]//JSON
        public IActionResult PostBody([FromBody] Product product) => Post(product);

        [HttpPut]//from form fields
        public IActionResult Put(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var storedProduct = products.SingleOrDefault(p => p.Id == product.Id);
            if (storedProduct == null)
                return NotFound();
            storedProduct.Name = product.Name;
            storedProduct.Price = product.Price;
            return Ok(storedProduct);
        }

        [HttpPut("UpdateProduct")]//JSON
        public IActionResult PutBody([FromBody] Product product) => Put(product);




    }
}
