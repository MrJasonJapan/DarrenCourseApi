using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DarrenTestProject.Controllers
{
    [RoutePrefix("products")]
    public class ProductsController : ApiController
    {
        // GET: api/Products
        [HttpGet, Route("")]
        public IEnumerable<string> SweetReturnAllTheProducts()
        {
            return new string[] { "product1", "product2" };
        }

        // GET: api/Products/5
        [HttpGet, Route("{id}")]
        public string Get(int id)
        {
            return "product";
        }

        // GET: api/Products/5/orders/custid
        [HttpGet, Route("{id}/orders/{custid}")]
        public string Get(int id, string custid)
        {
            return "product-orders" + custid;
        }


        // POST: api/Products
        [HttpPost, Route("")]
        public void CreateProduct([FromBody]string value)
        {
        }

        // PUT: api/Products/5
        [HttpPut, Route("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Products/5
        [HttpDelete, Route("{id}")]
        public void Delete(int id)
        {
        }
    }
}
