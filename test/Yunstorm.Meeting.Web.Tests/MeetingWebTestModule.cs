using System;
using System.Collections.Generic;
using System.Globalization;
using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using Yunstorm.Meeting.Localization;
using Yunstorm.Meeting.Web;
using Volo.Abp;
using Volo.Abp.AspNetCore.TestBase;
using Volo.Abp.Localization;
using Volo.Abp.Localization.Resources.AbpValidation;
using Volo.Abp.Modularity;

namespace Yunstorm.Meeting
{
    [DependsOn(
        typeof(AbpAspNetCoreTestBaseModule),
        typeof(MeetingWebModule),
        typeof(MeetingApplicationTestModule)
    )]
    public class MeetingWebTestModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.PreConfigure<IMvcBuilder>(builder =>
            {
                builder.PartManager.ApplicationParts.Add(new AssemblyPart(typeof(MeetingWebModule).Assembly));
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            ConfigureLocalizationServices(context.Services);
        }

        private static void ConfigureLocalizationServices(IServiceCollection services)
        {
            var cultures = new List<CultureInfo> { new CultureInfo("en"), new CultureInfo("tr") };
            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture("en");
                options.SupportedCultures = cultures;
                options.SupportedUICultures = cultures;
            });

            services.Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<MeetingResource>()
                    .AddBaseTypes(
                        typeof(AbpValidationResource),
                        typeof(AbpUiResource)
                    );
            });
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();

            app.Use(async (ctx, next) =>
            {
                try
                {
                    await next();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            });

            app.UseVirtualFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseAbpRequestLocalization();

            app.Use(async (ctx, next) =>
            {
                try
                {
                    await next();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            });

            app.UseMvcWithDefaultRouteAndArea();
        }
    }
}