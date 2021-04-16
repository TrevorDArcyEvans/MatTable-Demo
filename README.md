# MatBlazor MatTable Demo Project
- it's all about the data!

## Background
[MatBlazor](https://www.matblazor.com/) is a great, open source, set of components for _Blazor_ development.

It includes a a data aware table component, [MatTable](https://www.matblazor.com/Table), 
which can page data from a web API.  However, the shape of the API is not documented; along with
how various other parameters interact.

## Installation
_MatTable_ requires additional support:<p/>

`Startup.cs`
```c#
    public void ConfigureServices(IServiceCollection services)
    {
      // ...

      // MatTable needs this to call its data URL
      services.AddScoped<HttpClient>();

      services
        .AddNewtonsoftJson(options =>
        {
          // MatTable assumes camel case when deserialising JSON data
          options.SerializerSettings.ContractResolver = 
            new CamelCasePropertyNamesContractResolver();
        });

      // ...
    }
```

* _HttpClient_ scope should be setup with the correct permissions/identity since it will
  probably be calling an API which requires suitable authentication/authorisation
* we must force all JSON to be camel case as this is a hard coded assumption in _MatTable_

## _MatTable_ data related properties

### Data API
* ApiUrl `String`
    * Specifies the API Url form for the table data
* LoadInitialData `Boolean`
   * Specifies whether to Load the Initial Table Data
* RequestApiOnlyOnce `Boolean`
   * Specifies whether to Request the API only once.
   * This will load **all** the data in a single call, so is only suitable for
     small data sets.

### Pageable
Requested data returned directly
* CurrentPage `Int32`
    * The current page, starting from one.
* PageSize `Int32`
    * The number of rows per page.
    
### Extended Pageable
Requested data and additional information returned 
* PagingDataPropertyName `String`
* PagingRecordsCountPropertyName `String`

### Searchable
* FilterByColumnName `String`
    * Specifies which column is used for the filter / search term.
      If this is populated the Search Textbox will be visible.

### Sortable
* SortBy `String`
  * data column on which API should order returned data
* Descending `Boolean`
  * whether API should sort returned data in descending order

## API Shapes
All data is assumed to be retrieved by an _HTTP.Get_ on a URL specified by the _ApiUrl_ property.
The assumed shape of the returned data will depend on several _MatTable_ properties.

### API Endpoint
_MatTable_ makes a very strong assumption about how its data URL will look but
makes reasonable assumptions about query parameter names.

```c#
    [HttpGet]
    public ActionResult GetAll(
      [FromQuery] int Page,
      [FromQuery] int PageSize,
      [FromQuery] string SortBy,
      [FromQuery] bool Descending,
      [FromQuery] string searchTerm)
    {
      // ...
    }
```

Note that _ActionResult_ will wrap the requested data.

* _Page_
  * 1-based 
  * corresponds to _MatTable.CurrentPage_
* _PageSize_
  * the number of rows per page
  * -1 if all rows are requested
* _SortBy_
  * can be `null`
  * data column on which to sort returned data
* _Descending_
  * defaults to `false`
  * whether to sort returned data in descending order
* _searchTerm_
  * can be `null`
  * user entered string in search box
  * API is called every time user changes string in search box

### Pageable Data
This is the simplest case as the requested data is returned directly:

```c#  
    [HttpGet]
    public ActionResult<IEnumerable<Contact>> GetAll(...)
    {
      // ...
    }
```

### Extended Pageable Data
If both _MatTable.PagingDataPropertyName_ **and** _MatTable.PagingRecordsCountPropertyName_ 
are set, then the shape of the returned data is a little more complicated:

```c#
    public class ContactDataEx
    {
      public IEnumerable<Contact> Contacts { get; set; }
      public int TotalContacts { get; set; }
    }
    
    [HttpGet]
    public ActionResult<ContactDataEx> GetAll(...)
    {
      // ...
    }
```

* _Contacts_ data property is specified by _MatTable.PagingDataPropertyName_
* _TotalContacts_ data property is specified by _MatTable.PagingRecordsCountPropertyName_
