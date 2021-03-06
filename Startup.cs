using System.Net.Http;
using MatBlazor;
using MatTableDemo.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace MatTableDemo
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
      // MatTable needs this
      // NOTE:  have to disable ssl checking, esp on Linux
      services.AddScoped(_ => new HttpClient(
        new HttpClientHandler
        {
          ServerCertificateCustomValidationCallback = delegate { return true; }
        }));

      // make singleton so contacts are not regenerated on very call
      services.AddSingleton<ContactController>();

      services
        .AddMvc(options => options.EnableEndpointRouting = false)
        .AddControllersAsServices()
        .AddNewtonsoftJson(options =>
        {
          // MatTable assumes camel case when deserialising JSON data
          options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

          options.SerializerSettings.Converters.Add(new StringEnumConverter());
        });

      services.AddRazorPages();
      services.AddServerSideBlazor();
      services.AddMatBlazor();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        app.UseExceptionHandler("/Error");
      }

      app.UseHttpsRedirection();
      app.UseStaticFiles();

      app.UseRouting();

      app.UseMvcWithDefaultRoute();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapBlazorHub();
        endpoints.MapFallbackToPage("/_Host");
      });
    }
  }
}
