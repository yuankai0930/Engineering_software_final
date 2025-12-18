using Microsoft.EntityFrameworkCore;
using work_webapp.Data;

namespace work_webapp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews()
                .AddRazorRuntimeCompilation();

            // EF Core DbContext (SQL Server LocalDB by default)
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            var app = builder.Build();

            // Ensure database exists and seed JSON once
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                db.Database.EnsureCreated();

                try
                {
                    var jsonPath = Path.Combine(app.Environment.ContentRootPath, "Data", "A01010000C-000674-011.json");
                    if (File.Exists(jsonPath))
                    {
                        // Avoid duplicate insertion if already imported
                        if (!db.RawJson.Any(r => r.SourceFile == jsonPath))
                        {
                            var json = File.ReadAllText(jsonPath);
                            db.RawJson.Add(new Models.RawJson
                            {
                                SourceFile = jsonPath,
                                JsonContent = json,
                                ImportedAt = DateTime.UtcNow
                            });
                            db.SaveChanges();
                        }
                    }
                }
                catch
                {
                    // Swallow seeding errors to not block app startup; can be logged later
                }
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
