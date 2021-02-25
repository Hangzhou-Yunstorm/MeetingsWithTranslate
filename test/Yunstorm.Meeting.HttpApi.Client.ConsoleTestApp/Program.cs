using System;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.Threading;

namespace Yunstorm.Meeting.HttpApi.Client.ConsoleTestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var application = AbpApplicationFactory.Create<MeetingConsoleApiClientModule>())
            {
                application.Initialize();

                var demo = application.ServiceProvider.GetRequiredService<ClientDemoService>();
                AsyncHelper.RunSync(() => demo.RunAsync());

                Console.WriteLine("Press ENTER to stop application...");
                Console.ReadLine();
            }
        }
    }
}
