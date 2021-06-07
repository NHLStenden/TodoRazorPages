using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace TodoDemo
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
            services.AddRazorPages();

           
            services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }


            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseSession();
            
            app.UseRouting();

            app.UseAuthorization();

            // app.Use(async (context, next) =>
            // {
            //     int? userId = context.Session.GetInt32("userid");
            //     
            //     string path = context.Request.Path;
            //     if (path.StartsWith("/Login") && userId == null)
            //     {
            //         await next.Invoke();
            //     }
            //     
            //     if (userId.HasValue)
            //     {
            //         await next.Invoke();
            //     }
            //     else
            //     {
            //         context.Response.Redirect("/Login");
            //     }
            // });

            app.UseEndpoints(endpoints => { endpoints.MapRazorPages(); });
        }
    }
}