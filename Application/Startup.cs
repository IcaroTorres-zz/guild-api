using Application.Extensions;
using Application.Middlewares;
using Application.Swagger;
using Business.Validators.Requests.Guilds;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace Application
{
	public class Startup
	{
		public Startup(IConfiguration configuration, IWebHostEnvironment env)
		{
			Configuration = configuration;
			HostEnvironment = env;
		}

		private readonly OpenApiInfo _swaggerOpenApiInfo = new OpenApiInfo {Title = "Guild-api", Version = "v1"};
		public IConfiguration Configuration { get; }
		public IWebHostEnvironment HostEnvironment { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			services
				// enable access to Current HttpContext
				.AddHttpContextAccessor()

				// swagger
				.AddSwaggerGen(c => c.SwaggerDoc(_swaggerOpenApiInfo.Version, _swaggerOpenApiInfo))

				// register cache services DI
				.BootstrapCacheService(Configuration)

				// register Data Access Layer services DI
				.BootstrapDALServices(HostEnvironment, Configuration)

				// register MediatR Pipelines, Handlers and behaviors
				.BootstrapPipelinesServices()

				// register FluentValidators
				.BootstrapValidators()

				// enabling Mvc framework services and resources
				.AddControllers() //options => options.EnableEndpointRouting = false)

				// enabling validations
				// .AddFluentValidation(fv =>
				// {
				// 	fv.ConfigureClientsideValidation(enabled: false);
				// 	fv.ImplicitlyValidateChildProperties = true;
				// 	fv.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
				// 	fv.RegisterValidatorsFromAssemblyContaining<CreateGuildCommandValidator>();
				// })

				// new integration with newtonsoft json net for net core 3.0 +
				.AddNewtonsoftJson(options =>
				{
					options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
					options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
					options.SerializerSettings.Formatting = Formatting.Indented;
				})

				// custom hateoas resources options for JsonHateoasFormatter
				.EnableHateoasOutput()

				// latest compatibility recommended after 3.0
				.SetCompatibilityVersion(CompatibilityVersion.Latest);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
				app.UseDeveloperExceptionPage();
			else
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			var swaggerOptions = new MySwaggerOptions();
			Configuration.GetSection(nameof(swaggerOptions)).Bind(swaggerOptions);

			app
				// new .net core routing services required before using middleware
				.UseRouting()

				// exception handling as Internal server error output
				.UseMiddleware(typeof(ExceptionHandlingMiddleware))

				// swagger
				.UseSwagger(option => option.RouteTemplate = swaggerOptions.JsonRoute)
				.UseSwaggerUI(option => option.SwaggerEndpoint(swaggerOptions.UiEndpoint, swaggerOptions.Description))

				// redirection
				.UseHttpsRedirection()

				// new endpoint resources registrations
				.UseEndpoints(e => e.MapControllers());
		}
	}
}