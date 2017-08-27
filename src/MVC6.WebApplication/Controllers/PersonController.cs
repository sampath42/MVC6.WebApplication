using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MVC6.WebApplication.Controllers
{
    public class PersonModel
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    [Route("api/people")]
    public class PersonController : Controller
    {
        static List<PersonModel> people = new List<PersonModel>()
        {
            new PersonModel {Name="One", Age=1 },
            new PersonModel {Name="Two", Age=2 },
            new PersonModel {Name="Three", Age=3}
        };

        [HttpGet]
        [Route("get")]
        public IActionResult Get()
        {
            return new ObjectResult(people);
        }
    }
}