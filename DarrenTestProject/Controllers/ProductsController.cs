using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DarrenTestProject.Controllers
{
    public class ProductsController : ApiController
    {
        // GET: api/Products
        public IEnumerable<string> GetAllProducts()
        {
            return new string[] { "product1", "product2" };
        }

        // GET: api/Products/5
        public string Get(int id)
        {
            return "product";
        }

        // POST: api/Products
        [HttpPost]
        public void CreateProduct([FromBody]string value)
        {
        }

        // PUT: api/Products/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Products/5
        public void Delete(int id)
        {
        }
    }
}
