﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
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

        [JsonConverter(typeof(StringEnumConverter))]
        public enum Widgets
        {
            Bolt,
            Screw,
            Nut,
            Motor
        };

        // GET: Products/widget/xxx
        [HttpGet, Route("widget/{widget:enum(DarrenTestProject.Controllers.ProductsController+Widgets)}")]
        public string GetProductsWithWidget(Widgets widget)
        {
            return "widget-" + widget.ToString();
        }

        // GET/VIEW: api/Products
        [Route("")]
        [AcceptVerbs("GET", "VIEW")]
        public IEnumerable<string> SweetReturnAllTheProducts()
        {
            return new string[] { "product1", "product2" };
        }

        // GET: api/Products/5
        [HttpGet, Route("{id:int:range(1000,3000)}")]
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
        [HttpPut, Route("{id:int:range(1000,3000)}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Products/5
        [HttpDelete, Route("{id:int:range(1000,3000)}")]
        public void Delete(int id)
        {
        }
    }
}
