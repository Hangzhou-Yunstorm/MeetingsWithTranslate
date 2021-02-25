using System.IO;
using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Yunstorm.Meeting.EntityFrameworkCore;
using Yunstorm.Meeting.Localization;
using Microsoft.OpenApi.Models;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.Autofac;
using Volo.Abp.AutoMapper;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.VirtualFileSystem;
using Yunstorm.Meeting.Web.Hubs;
using Volo.Abp.BackgroundJobs;
using Yunstorm.Meeting.OnlineMeetingRoom;
using Volo.Abp.BackgroundWorkers;
using Microsoft.Extensions.Options;
using Volo.Abp.Threading;

namespace Yunstorm.Meeting.Web
{
    [DependsOn(
        typeof(MeetingApplicationModule),
        typeof(MeetingEntityFrameworkCoreDbMigrationsModule),
        typeof(AbpAutofacModule),
        typeof(AbpAspNetCoreMvcModule)
        )]
    public class MeetingWebModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
            {
                options.AddAssemblyResource(
                    typeof(MeetingResource),
                    typeof(MeetingDomainModule).Assembly,
                    typeof(MeetingDomainSharedModule).Assembly,
                    typeof(MeetingApplicationModule).Assembly,
                    typeof(MeetingApplicationContractsModule).Assembly,
                    typeof(MeetingWebModule).Assembly
                );
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var hostingEnvironment = context.Services.GetHostingEnvironment();
            var configuration = context.Services.GetConfiguration();

            ConfigureUrls(configuration);
            ConfigureAutoMapper();
            ConfigureVirtualFileSystem(hostingEnvironment);
            ConfigureLocalizationServices();
            ConfigureAutoApiControllers();
            ConfigureSwaggerServices(context.Services);
            ConfigureSignalr(context.Services);
        }

        private void ConfigureUrls(IConfiguration configuration)
        {
            Configure<AppUrlOptions>(options =>
            {
                options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"];
            });
        }

        private void ConfigureAutoMapper()
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<MeetingWebModule>();
            });
        }

        private void ConfigureVirtualFileSystem(IWebHostEnvironment hostingEnvironment)
        {
            if (hostingEnvironment.IsDevelopment())
            {
                Configure<AbpVirtualFileSystemOptions>(options =>
                {
                    options.FileSets.ReplaceEmbeddedByPhysical<MeetingDomainSharedModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}Yunstorm.Meeting.Domain.Shared"));
                    options.FileSets.ReplaceEmbeddedByPhysical<MeetingDomainModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}Yunstorm.Meeting.Domain"));
                    options.FileSets.ReplaceEmbeddedByPhysical<MeetingApplicationContractsModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}Yunstorm.Meeting.Application.Contracts"));
                    options.FileSets.ReplaceEmbeddedByPhysical<MeetingApplicationModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}Yunstorm.Meeting.Application"));
                    options.FileSets.ReplaceEmbeddedByPhysical<MeetingWebModule>(hostingEnvironment.ContentRootPath);
                });
            }
        }

        private void ConfigureLocalizationServices()
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<MeetingResource>()
                    .AddBaseTypes(
                        typeof(AbpUiResource)
                    );

                options.Languages.Add(new LanguageInfo("cs", "cs", "Čeština"));
                options.Languages.Add(new LanguageInfo("en", "en", "English"));
                options.Languages.Add(new LanguageInfo("pt-BR", "pt-BR", "Português"));
                options.Languages.Add(new LanguageInfo("tr", "tr", "Türkçe"));
                options.Languages.Add(new LanguageInfo("zh-Hans", "zh-Hans", "简体中文"));
            });
        }

        private void ConfigureAutoApiControllers()
        {
            Configure<AbpAspNetCoreMvcOptions>(options =>
            {
                options.ConventionalControllers.ConventionalControllerSettings.Clear();
                options.ConventionalControllers.Create(typeof(MeetingApplicationModule).Assembly);
            });
        }

        private void ConfigureSwaggerServices(IServiceCollection services)
        {
            services.AddSwaggerGen(
                options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Meeting API", Version = "v1" });
                    options.DocInclusionPredicate((docName, description) => true);
                    options.CustomSchemaIds(type => type.FullName);
                }
            );
        }

        private void ConfigureSignalr(IServiceCollection services)
        {
            services.AddSignalR();
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();

            app.UseCorrelationId();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseErrorPage();
            }

            app.UseVirtualFiles();
            app.UseRouting();

            app.UseAbpRequestLocalization();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Meeting API");
            });

            app.UseAuditing();

            app.UseEndpoints(builder =>
            {
                builder.MapDefaultControllerRoute();
                builder.MapHub<ChatHub>("/chat");
            });
        }
    }
}
