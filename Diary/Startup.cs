using AutoMapper;
using Diary.Consumers;
using Diary.DataAccess;
using Diary.Mapping;
using Diary.MiddleWares;
using Diary.Settings;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FluentValidation.AspNetCore;
using FluentValidation;
using Diary.Validators;
using Diary.GRPC.Services;
using Grpc.Core;
using GrpcDiary;
using Grpc.Net;
using Grpc.AspNetCore;
using Google.Protobuf.WellKnownTypes;

namespace Diary
{
    public class Startup
    {
        private IConfiguration Configuration { get; }
        private const string Origin = "DiarySpecificOrigins";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {       
            var connectionString = Environment.GetEnvironmentVariable("diary_connection_db_string");
            var corsConfig       = Configuration.GetSection("CorsSettings").Get<CorsSettings>();

            services.AddDbContext<EfDbContext>(optionsBuilder
               => optionsBuilder
                   .UseNpgsql(connectionString));

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = Configuration.GetConnectionString("Redis");
            });



            services.AddCors(options =>
            {
                options.AddPolicy(name: Origin,
                                  build =>
                                  {
                                      build.WithOrigins(corsConfig.Origins)
                                      .WithMethods(corsConfig.Methods)
                                      .WithHeaders(corsConfig.Headers);
                                  });
            });

            InstallAutomapper(services);

            services.AddServices(Configuration);
            services.AddControllers();
            services.AddHealthChecks().AddCheck<DiaryHealthCheck>("diaryHealth", tags: new string[] { "diaryHealthCheck" });
            services.AddServices(Configuration);
            services.AddControllers();
            services.AddFluentValidationAutoValidation();
            services.AddValidators();
            services.AddGrpc();

            services.AddMassTransit(configurator =>
            {
                configurator.SetKebabCaseEndpointNameFormatter();
                //configurator.AddConsumer<CreateDiaryLineFromMagazineConsumer>();
                configurator.AddConsumer<RoomDesignerStartingRoomConsumer>();

                configurator.UsingRabbitMq((context, cfg) =>
                {
                    var rmqSettings = Configuration.Get<ApplicationSettings>()!.RmqSettings;
                    cfg.Host(rmqSettings.Host,
                                rmqSettings.VHost,
                                h =>
                                {
                                    h.Username(rmqSettings.Login);
                                    h.Password(rmqSettings.Password);
                                });
                  cfg.ConfigureEndpoints(context);
                });

            });

            services.AddOpenApiDocument(options =>
            {
                options.Title = "Diary API doc";
                options.Version = "1.0";
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseCors(Origin);

            app.UseOpenApi();
            app.UseSwaggerUi(x =>
            {
                x.DocExpansion = "list";
            });

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseHealthChecks("/diaryHealth", new HealthCheckOptions(){
                Predicate = healthCheck => healthCheck.Tags.Contains("diaryHealthCheck")
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGrpcService<GRPC.Services.DiaryGrpcService>();
            });
        }

        private static IServiceCollection InstallAutomapper(IServiceCollection services)
        {
            services.AddSingleton<IMapper>(new Mapper(GetMapperConfiguration()));
            return services;
        }

        private static MapperConfiguration GetMapperConfiguration()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<HabitDiaryOwnerMappingsProfile>();
                cfg.AddProfile<HabitDiaryMappingsProfile>();
                cfg.AddProfile<HabitDiaryLineMappingsProfile>();
                cfg.AddProfile<HabitMappingsProfile>();
                cfg.AddProfile<HabitStateMappingsProfile>();
            });

            configuration.AssertConfigurationIsValid();
            return configuration;
        }      

    }
}
