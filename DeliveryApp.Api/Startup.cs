using DeliveryApp.Core.Ports;
using DeliveryApp.Infrastructure.Adapters.Postgres.Repositories;
using DeliveryApp.Infrastructure.Adapters.Postgres;
using Primitives;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using DeliveryApp.Api.Adapters.BackgroundJobs;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Api.OpenApi;
using Api.Filters;
using Api.Formatters;
using DeliveryApp.Core.Application.UseCases.Commands.AssingOrder;
using DeliveryApp.Core.Application.UseCases.Commands.CreateOrder;
using DeliveryApp.Core.Application.UseCases.Commands.StartWork;
using DeliveryApp.Core.Application.UseCases.Commands.StopWork;
using DeliveryApp.Core.Application.UseCases.Queries.GetCouriers;
using MediatR;
using DeliveryApp.Api.Configuration;
using DeliveryApp.Core.DomainServices;
using DeliveryApp.Infrastructure.Adapters.Grpc.GeoService;
using DeliveryApp.Api.Adapters.Kafka.BasketConfirmed;
using DeliveryApp.Infrastructure.Adapters.Kafka.OrderStatusChanged;
using DeliveryApp.Infrastructure.Adapters.BackgroundJobs;

namespace DeliveryApp.Api
{
    public class Startup
    {
        public Startup()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddEnvironmentVariables();
            var configuration = builder.Build();
            Configuration = configuration;
        }

        /// <summary>
        /// Конфигурация
        /// </summary>
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Health Checks
            services.AddHealthChecks();

            // Cors
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    policy =>
                    {
                        policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                    });
            });
            services.AddControllers(options =>
            {
                options.InputFormatters.Insert(0, new InputFormatterStream());
            }).AddNewtonsoftJson(options =>
                 {
                     options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                     options.SerializerSettings.Converters.Add(new StringEnumConverter
                     {
                         NamingStrategy = new CamelCaseNamingStrategy()
                     });
                 });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("1.0.0", new OpenApiInfo
                {
                    Title = "Delivery Service",
                    Description = "Отвечает за учет курьеров, деспетчеризацию доставкуов, доставку",
                    Contact = new OpenApiContact
                    {
                        Name = "Dmitry K",
                        Url = new Uri("https://mardul.ru"),
                        Email = "mardul@inbox.ru"
                    }
                });
                options.CustomSchemaIds(type => type.FriendlyId(true));
                options.IncludeXmlComments($"{AppContext.BaseDirectory}{Path.DirectorySeparatorChar}{Assembly.GetEntryAssembly().GetName().Name}.xml");
                options.DocumentFilter<BasePathFilter>("");
                options.OperationFilter<GeneratePathParamsValidationFilter>();
            });
            services.AddSwaggerGenNewtonsoftSupport();



            // Configuration
            services.Configure<Settings>(options => Configuration.Bind(options));
            var connectionString = Configuration["CONNECTION_STRING"];
            var geoServiceGrpcHost = Configuration["GEO_SERVICE_GRPC_HOST"];
            var messageBrokerHost = Configuration["MESSAGE_BROKER_HOST"];

            // Configured MediatR
            services.AddConfiguredMediator(connectionString);

            // SQL
            services.AddDbContext<AppDbContext>(options =>
             options.UseNpgsql(connectionString));

            // Unit Of Work
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            // Ports & Adapters
            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddTransient<ICourierRepository, CourierRepository>();
            services.AddTransient<IGeoClient>(x => new GeoClient(geoServiceGrpcHost));
            services.AddTransient<IBusProducer>(x => new Producer(messageBrokerHost));

            // Domain Services
            services.AddTransient<IDispatchService, DispatchService>();

            // QuartZ
            services.AddQuartz(cfg =>

            {
                var assignOrdersJob = new JobKey(nameof(AssignOrdersJob));
                var moveToOrdersJob = new JobKey(nameof(MoveToOrdersJob));
                var outboxSendMessageJob = new JobKey(nameof(OutboxSendMessageJob));

                cfg.AddJob<AssignOrdersJob>(assignOrdersJob)
                .AddTrigger(
                    trigger => trigger.ForJob(assignOrdersJob)
                    .WithSimpleSchedule(
                        schedule => schedule.WithIntervalInSeconds(1)
                        .RepeatForever()))
                .AddJob<MoveToOrdersJob>(moveToOrdersJob)
                .AddTrigger(
                    trigger => trigger.ForJob(moveToOrdersJob)
                    .WithSimpleSchedule(
                        schedule => schedule.WithIntervalInSeconds(2)
                        .RepeatForever()))
                .AddJob<OutboxSendMessageJob>(outboxSendMessageJob)
                .AddTrigger(
                    trigger => trigger.ForJob(outboxSendMessageJob)
                    .WithSimpleSchedule(
                        schedule => schedule.WithIntervalInSeconds(5)
                        .RepeatForever()));
            });   
            services.AddQuartzHostedService();

            // Message Broker
            services.Configure<HostOptions>(options =>
            {
                options.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore;
                options.ShutdownTimeout = TimeSpan.FromSeconds(30);
            });
            // gRPC
            services.AddGrpcClient<GeoClient>(options => options.Address = new Uri(geoServiceGrpcHost));

            var sp = services.BuildServiceProvider();
            var mediator = sp.GetService<IMediator>();
            services.AddHostedService(x => new ConsumerService(mediator, messageBrokerHost));

          
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
            app.UseCors();

            app.UseRouting();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "openapi/{documentName}/openapi.json";
            })
                .UseSwaggerUI(options =>
                {
                    options.RoutePrefix = "openapi";
                    options.SwaggerEndpoint("/openapi/1.0.0/openapi.json", "Swagger Delivery Service");
                    options.RoutePrefix = string.Empty;
                    options.SwaggerEndpoint("/openapi-original.json", "Swagger Delivery Service");
                });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
           
            app.UseHealthChecks("/health");
            app.UseRouting();
        }
    }
}