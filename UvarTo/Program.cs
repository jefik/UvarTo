using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using UvarTo.Areas.Identity.Data;
using UvarTo.Data;
public class Program { 
    public static async Task Main(string[] args) {

    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    var defaultConnectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(defaultConnectionString));

    builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>();

    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(defaultConnectionString));

    // NA HESLO
    builder.Services.Configure<IdentityOptions>(options =>
    {
        options.Password.RequireUppercase = false;
    });

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseMigrationsEndPoint();
    }
    else
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }
       
        app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
    app.MapRazorPages();

    // ROLE
    using (var scope = app.Services.CreateScope())
    {
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var roles = new[] { "Admin", "Manager", "User" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));

            }
    }
    // ADMIN ÚÈET
    using (var scope = app.Services.CreateScope())
    {
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        string email = "jansilak@admin.cz";
        string password = "#Zkouska888";
            string FirstName = "Jan";
            string LastName = "Silák";
        if (await userManager.FindByEmailAsync(email) == null)
        {
            var user = new ApplicationUser();
                user.FirstName = FirstName;
                user.LastName = LastName;
            user.UserName = email;
            user.Email = email;
            
            await userManager.CreateAsync(user, password);

            await userManager.AddToRoleAsync(user, "Admin");
        }
           
    }

        app.Run();
    }
}