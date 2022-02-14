using MHRSLiteBusinessLayer.Contracts;
using MHRSLiteBusinessLayer.EmailService;
using MHRSLiteBusinessLayer.Implementations;
using MHRSLiteDataAccessLayer;
using MHRSLiteEntityLayer.IdentityModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MHRSLiteUI
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
            //AspNet Core ConnectionString ba�lant�s� yapabilmesi i�in servislerine DbContext eklenmesi gerekiyor.
            services.AddDbContext<MyContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("SqlConnection"));
            });

            //**********************************************//
            //IUnitOfWork g�rd���n zaman bana UnitOfWork nesnesi �ret!
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            //IEmailSender g�rd���n zaman bana EmailSender nesnesi �ret!
            services.AddScoped<IEmailSender, EmailSender>();
            //**********************************************//
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddMvc();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(60);
            });

            //*********************************************//
            services.AddIdentity<AppUser, AppRole>(opts=>
            {
                opts.User.RequireUniqueEmail = true;
                opts.Password.RequiredLength = 6;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireLowercase = false;
                opts.Password.RequireUppercase = false;
                opts.Password.RequireDigit = false;
                opts.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            }).AddDefaultTokenProviders().AddEntityFrameworkStores<MyContext>();
            //*********************************************//
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
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();


            app.UseStaticFiles();   //wwwroot klas�r�n�n kullan�labilmesi i�in
            app.UseRouting(); //rooting mekanizmas� i�in
            app.UseSession();   //session oturum mekanizmas� i�in
            app.UseAuthentication(); //login logout kullanabilmek i�in
            app.UseAuthorization(); //authorization attiribute kullanabilmek i�in



            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
