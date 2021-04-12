using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ContactInfoApp.Server.Configuration;
using ContactInfoApp.Server.Persistence;
using GetContactAPI;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;

namespace ContactInfoApp.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddSwaggerGen();

            services.AddScoped(s => new GetContact(new Data(
                Configuration.GetValue<string>("GetContact:Token"),
                Configuration.GetValue<string>("GetContact:AesKey")
            )));
            services.AddScoped<IComputerVisionClient>(s => new ComputerVisionClient(new ApiKeyServiceClientCredentials(
                Configuration["ComputerVisionApi:Key"]
            )) { Endpoint = Configuration["ComputerVisionApi:Endpoint"] });

            services.Configure<GetContactSettings>(Configuration.GetSection("GetContact"));

            services.AddDbContext<AppDbContext>();
            services.Configure<DatabaseSettings>(Configuration.GetSection("Database"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ContactInfo API");
            });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
