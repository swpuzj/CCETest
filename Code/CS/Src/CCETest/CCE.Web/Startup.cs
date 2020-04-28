using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CCE.Common;
using CCE.Common.Auth;
using CCE.Middleware;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Serilog.Extensions.Hosting;

namespace CCETest
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
            services.AddControllers();

            //增加Cookie中间件配置
            services.AddAuthentication("MyAuthentication")
                .AddCookie("MyAuthentication", o =>
                {
                    o.LoginPath = new PathString("/Home/Login"); // 登录页面的url
                    o.AccessDeniedPath = new PathString("/Home/Login");//没有授权跳转的页面
                    o.ExpireTimeSpan = TimeSpan.FromHours(1); // cookies的过期时间
                    o.Cookie.Name = "MyCookieName";
                    o.Cookie.Path = "/";
                    o.Cookie.MaxAge = TimeSpan.FromHours(4);
                    o.Cookie.HttpOnly = true;
                    o.SessionStore = services.BuildServiceProvider().GetService<ITicketStore>();
                    o.SlidingExpiration = true;

                    o.DataProtectionProvider = new MyDataProtector();

                    o.Events.OnRedirectToLogin = context =>
                    {
                        context.Response.Clear();
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.Headers["Location"] = "/Home/Login";
                        return Task.CompletedTask;
                    };
                });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "CCETest"
                });
            });
         
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            DependencyInjection.Init(app.ApplicationServices);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.RoutePrefix = "swagger";
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "CCETest API Doc");
            });

            app.UseMiddleware<LoggingMiddleware>();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
