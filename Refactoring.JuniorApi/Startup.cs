using System.ServiceModel;
using System.ServiceModel.Channels;
using Refactoring.LegacyService;
using Refactoring.LegacyService.Candidates;
using Refactoring.LegacyService.Candidates.Repositories;
using Refactoring.LegacyService.Candidates.Services;
using Refactoring.LegacyService.Positions.Repositories;

namespace Refactoring.JuniorApi
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddScoped<ICandidateService, CandidateService>();
            services.AddScoped<ICandidateBuilder, CandidateBuilder>();
            services.AddScoped<IPositionRepository, PositionRepository>(x => new PositionRepository(Configuration.GetConnectionString("appDatabase")));
            services.AddScoped<ICandidateCreditService, CandidateCreditServiceClient>(x => new CandidateCreditServiceClient(new CustomBinding(), new EndpointAddress("http://test.com.au")));
            services.AddScoped<ICandidateDataAccessProxy, CandidateDataAccessProxy>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Refactoring.JuniorApi", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Refactoring.JuniorApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
