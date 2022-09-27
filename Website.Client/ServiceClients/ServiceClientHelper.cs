using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Material.Blazor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Website.Client.ServiceClients
{
    public static class ServiceClientHelper
    {
        public static void Inject(IServiceCollection serviceCollection)
        {
            //
            // Third party library services
            //
            serviceCollection.AddMBServices(options =>
            {
                options.LoggingServiceConfiguration = Utilities.GetDefaultLoggingServiceConfiguration();
                options.ToastServiceConfiguration = Utilities.GetDefaultToastServiceConfiguration();
                options.SnackbarServiceConfiguration = Utilities.GetDefaultSnackbarServiceConfiguration();
            });
        }
    }
}
