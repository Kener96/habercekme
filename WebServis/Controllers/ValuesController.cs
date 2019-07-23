using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebServis.Models;

namespace WebServis.Controllers
{
    public class ValuesController : ApiController
    {

        ///// <summary>
        ///// GET api/GetTestResponse
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet]
        //public TestResponse GetTestResponse()
        //{
        //    return new TestResponse {
        //        tamSayisi = 55,
        //        test ="jdıwkdhwdn" };
        //}
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public TestResponse Get(int id)
        {
            return new TestResponse
            {
                        tamSayisi = 55,
                        test ="jdıwkdhwdn" };
            }

        // POST api/values
            public void Post([FromBody]string value)
        {

            //buraya yazıcaksınnn




        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
