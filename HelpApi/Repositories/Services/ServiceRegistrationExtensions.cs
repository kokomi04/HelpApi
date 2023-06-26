using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Services
{
    public static class ServiceRegistrationExtensions
    {
        public static IServiceCollection AddMyLibraryServices(this IServiceCollection services)
        {
            var myLibraryAssembly = typeof(GuideService).Assembly; // Replace MyService with your service type

            var types = myLibraryAssembly.GetExportedTypes()
                                         .Where(t => t.IsClass && !t.IsAbstract);

            foreach (var type in types)
            {
                var interfaces = type.GetInterfaces().Where(i => i.Name.StartsWith("I"));
                foreach (var @interface in interfaces)
                {
                    services.AddScoped(@interface, type);
                }
            }

            return services;
        }
    }
}
