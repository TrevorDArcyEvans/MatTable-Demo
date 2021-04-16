using System.Collections.Generic;
using MatTableDemo.Models;
using Microsoft.AspNetCore.Mvc;

namespace MatTableDemo.Controllers
{
  [Route("api/[controller]")]
  [Produces("application/json")]
  public class ContactController : ControllerBase
  {
    private readonly List<Contact> _data = new List<Contact>
    {
      new Contact{ FirstName = "Ken", LastName = "Gray"},
      new Contact{ FirstName = "Steve", LastName = "Grace"},
      new Contact{ FirstName = "Robin", LastName = "Hood"}
    };

    [HttpGet]
    public ActionResult<IEnumerable<Contact>> GetAll()
    {
      return _data;
    }
  }
}
