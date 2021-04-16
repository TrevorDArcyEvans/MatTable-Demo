using System.Collections.Generic;
using System.Linq;
using MatTableDemo.Models;
using Microsoft.AspNetCore.Mvc;

namespace MatTableDemo.Controllers
{
  [Route("api/[controller]")]
  [Produces("application/json")]
  public class ContactController : ControllerBase
  {
    private readonly List<Contact> _data = new List<Contact>();

    public ContactController()
    {
      foreach (var i in Enumerable.Range(0, 100))
      {
        _data.Add(
          new Contact
          {
            FirstName = Faker.Name.First(),
            LastName = Faker.Name.Last()
          });
      }
    }

    [HttpGet]
    public ActionResult<IEnumerable<Contact>> GetAll(
      [FromQuery] int Page,
      [FromQuery] int PageSize,
      [FromQuery] string SortBy,
      [FromQuery] bool Descending,
      [FromQuery] string searchTerm
      )
    {
      return _data;
    }
  }
}
