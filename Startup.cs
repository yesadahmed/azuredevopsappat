using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using azuredevopsappat.Config;
using Microsoft.Extensions.ObjectPool;
using azuredevopsappat.DevOpsBuilds;
using azuredevopsappat.ObjectPooling;
using Microsoft.OpenApi.Models;

namespace azuredevopsappat
{
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
            DevOpsConnectionPoolPolicy DevOpsPolicy = null;//default
            services.AddControllers();
            services.AddSingleton<IBuildOperations, BuildOperations>();
            services.AddSingleton<IWorkItemOperation, WorkItemOperation>();

            //Poling objects
            var DevOpsAuths = new DevOpsAuths();
            Configuration.GetSection(DevOpsAuths.DevOpsAuth).Bind(DevOpsAuths);

            if (DevOpsAuths != null) //must have PAT and URL Project
            {
                DevOpsPolicy = new DevOpsConnectionPoolPolicy(DevOpsAuths.PAT, DevOpsAuths.PURL);
                services.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();

                services.AddSingleton<ObjectPool<DevOpsConnectionPool>>(serviceProvider =>
                {
                    var provider = serviceProvider.GetRequiredService<ObjectPoolProvider>();
                    return provider.Create(DevOpsPolicy);

                });
            }

            services.AddSwaggerGen(c =>
           {
               c.SwaggerDoc("v1", new OpenApiInfo { Title = "Azure DevOps SDK API Dockers", Version = "v1" });
           });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Azure DevOps API V1 Dockers");
            });
        }
    }
}
