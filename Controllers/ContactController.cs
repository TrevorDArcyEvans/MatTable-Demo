using System;
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
    private readonly List<Contact> _data;

    public ContactController()
    {
      _data =
        Enumerable.Range(0, 1000).Select(_ =>
          new Contact
          {
            FirstName = Faker.Name.First(),
            LastName = Faker.Name.Last()
          }).ToList();
    }

    [HttpGet]
    public ActionResult GetAll(
      [FromQuery] int Page, // Page is 1-based in MatTable
      [FromQuery] int PageSize,
      [FromQuery] string SortBy,
      [FromQuery] bool Descending,
      [FromQuery] string searchTerm
    )
    {
      // MatTable uses -1 for everything
      var dataPageSize = PageSize == -1 ? int.MaxValue : PageSize;

      var filtData =
        searchTerm is null ? _data : _data.Where(c => c.LastName.Contains(searchTerm, StringComparison.InvariantCultureIgnoreCase));
      var filtDataList = filtData.ToList();
      var contacts = filtDataList
        .Skip((Page - 1) * dataPageSize)
        .Take(dataPageSize);
      var res = new
      {
        // MatTable assumes camel case when deserialising JSON data
        contacts = contacts,
        totalContacts = filtDataList.Count()
      };

      return Ok(res);
    }
  }
}
