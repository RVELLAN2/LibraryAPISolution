using LibraryAPI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using LibraryAPI.Services;

namespace LibraryAPIIntegrationTests
{
    public class WebTestFixture : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
             {
                 var systemTimeDescriptor = services.SingleOrDefault(
                     d => d.ServiceType == typeof(ISystemTime));


                 services.Remove(systemTimeDescriptor);
                 services.AddScoped<ISystemTime, FakeTime>();
             });
        }
    }
    public class FakeTime : ISystemTime
    {
        public DateTime GetCurrent()
        {
            return new DateTime(1969, 4, 20, 23, 59, 00);
        }
    }
}
