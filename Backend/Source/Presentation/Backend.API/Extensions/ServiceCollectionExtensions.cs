﻿using AutoMapper;
using Backend.Business.Authorization;
using Backend.Business.Authorization.Extensions;
using Backend.Domain;
using Backend.Infrastructure.Extensions;
using Backend.Infrastructure.Providers;
using Backend.Library.AmazonS3.Extensions;
using Backend.Library.Logging.Extensions;
using Backend.Persistence;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Backend.API.Extensions
{
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        /// CORS policies
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureCors(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(opt =>
            {
                opt.AddPolicy("AllowAllCorsPolicy",
                    builder => builder
                        .WithOrigins(configuration.GetSection("CORSAllowedOrigins").Get<string[]>())
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .WithExposedHeaders("Content-Disposition")
                );
            });
        }

        /// <summary>
        /// Configures MVC filters, json options and createUsers validators
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureMvc(this IServiceCollection services)
        {
            services
                .AddControllers()
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblies(new[]
                {
                    // list all assemblies containing validators
                    Assembly.GetAssembly(typeof(Business.Notifications.Mappings)),
                    Assembly.GetAssembly(typeof(Business.Authorization.Mappings)),
                    Assembly.GetAssembly(typeof(Business.Users.Mappings)),
                    Assembly.GetAssembly(typeof(Business.Contacts.Mappings)),
                }))
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.AllowInputFormatterExceptionMessages = true;
                });
        }

        /// <summary>
        /// Adds database context and initializes database using connection string provided
        /// </summary>
        /// <param name="services"></param>
        /// <param name="connectionString"></param>
        public static void ConfigureDatabase(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                //options.UseLazyLoadingProxies();
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking); //TODO: Revisit this setting
                options.UseNpgsql(connectionString);
            });

            services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());
        }

        /// <summary>
        /// Adds and configures JWT Token based authorization
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void ConfigureJwtAuth(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettingsSection = configuration.GetSection("JwtSettings"); // get app setting section
            var jwtSettings = jwtSettingsSection.Get<JwtSettings>(); // get instance of forementioned POCO CLASS
            var jwtSecretKey = Encoding.ASCII.GetBytes(jwtSettings.JwtSecret); // extract secret

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                    .AddJwtBearer(x =>
                    {
                        x.RequireHttpsMetadata = true;
                        x.SaveToken = true;
                        x.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(jwtSecretKey),
                            ValidateIssuer = false,
                            ValidateAudience = false
                        };

                        // We have to hook the OnMessageReceived event in order to
                        // allow the JWT authentication handler to read the access
                        // token from the query string when a WebSocket or
                        // Server-Sent Events request comes in.
                        //https://docs.microsoft.com/en-us/aspnet/core/signalr/authn-and-authz?view=aspnetcore-2.2
                        //x.Events = new JwtBearerEvents()
                        //{
                        //    OnMessageReceived = context =>
                        //    {
                        //        var accessToken = context.Request.Cookies["jwt"];

                        //        // If the request is for our hub...
                        //        var path = context.HttpContext.Request.Path;
                        //        if (!string.IsNullOrEmpty(accessToken) && path.Equals("/api/notifications-hub"))
                        //        {
                        //            // Read the token out of the query string
                        //            context.Token = $"{accessToken}";
                        //        }

                        //        return Task.CompletedTask;
                        //    }
                        //};
                    });
        }

        /// <summary>
        /// Configures response compression
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureResponseCompression(this IServiceCollection services)
        {
            services.AddResponseCompression();

            services.Configure<BrotliCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Optimal;
            });
        }

        /// <summary>
        /// Configures swagger ( Swashbuckle Core )
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(action =>
            {
                action.SwaggerDoc("v1.0", new OpenApiInfo { Title = "Backend API", Version = "1.0" });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                action.IncludeXmlComments(xmlPath);

                action.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
            });

            services.AddSwaggerGenNewtonsoftSupport(); // explicit opt-in - needs to be placed after AddSwaggerGen()
        }

        /// <summary>
        /// Configures MediatR for request pipeline and adds some middleware
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureMediatR(this IServiceCollection services)
        {
            var assemblies = new Assembly[]
            {
                Assembly.GetAssembly(typeof(Business.Notifications.Mappings)),
                Assembly.GetAssembly(typeof(Business.Contacts.Mappings)),
                Assembly.GetAssembly(typeof(Business.Authorization.Mappings)),
                Assembly.GetAssembly(typeof(Business.Users.Mappings)),
            };

            services.AddMediatR(assemblies.ToArray());
            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPostProcessorBehavior<,>));
        }


        /// <summary>
        /// Sets up license context for epplus
        /// </summary>
        public static void ConfigureEPPlus(this IServiceCollection services)
        {
            //TODO if using excel
            //ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        /// <summary>
        /// Configures lazy cache library
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureLazyCache(this IServiceCollection services)
        {
            services.AddLazyCache();
        }

        /// <summary>
        /// Configures automapper using automapper extension for dependency injection
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureAutomapper(this IServiceCollection services)
        {
            // AddAutoMapper is Syntactic sugar.. we can do it manually
            // needed because of chat mappings and it's constructor that needs s3Service
            //services.AddAutoMapper(types);

            services.AddSingleton<IMapper>(provider =>
            {
                var config = new MapperConfiguration(c =>
                {
                    c.AddProfile<Business.Notifications.Mappings>();
                    c.AddProfile<Business.Contacts.Mappings>();
                    c.AddProfile<Business.Authorization.Mappings>();
                    c.AddProfile<Business.Users.Mappings>();
                });

                return config.CreateMapper();
            });
        }

        /// <summary>
        /// Configures signalR
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureSignalR(this IServiceCollection services)
        {
            services
                .AddSignalR(options =>
                {
                    options.EnableDetailedErrors = true;
                })
                .AddNewtonsoftJsonProtocol(options =>
                {
                    options.PayloadSerializerSettings = new JsonSerializerSettings()
                    {
                        Formatting = Formatting.Indented,
                        Converters = new List<JsonConverter>()
                        {
                            new StringEnumConverter()
                        },
                        ContractResolver = new CamelCasePropertyNamesContractResolver(),
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    };
                });

            // Change to use Name as the user identifier for SignalR
            // WARNING: This requires that the source of your JWT token
            // ensures that the Name claim is unique!
            // If the Name claim isn't unique, users could receive messages
            // intended for a different user!

            // For this application implementation claim type NAME of JWT is USERID so it is Unique
            services.AddSingleton<IUserIdProvider, NameUserIdProvider>();
        }

        /// <summary>
        /// Register http context accessor in fail safe way
        /// </summary>
        public static void ConfigureHttpContextAccessor(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        /// <summary>
        /// Configures all core services (business and shared)
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureCoreServices(this IServiceCollection services)
        {
            services.ConfigureAuthorizationServices();
            services.ConfigureInfrastructureServices();
            services.ConfigureS3Services();
            services.ConfigureLoggingService();
            //services.ConfigureMediaCompressionService(); // when needed
            services.ConfigureHttpContextAccessor();
        }


        /// <summary>
        /// Configures core settings
        /// </summary>
        public static void ConfigureCoreSettings(this IServiceCollection services, IConfiguration config)
        {
            services.ConfigureJwtSettings(config);
            services.ConfigureAppSettings(config);
            services.ConfigureS3Settings(config);
            services.ConfigureLogLevelSettings(config);
        }
    }
}
