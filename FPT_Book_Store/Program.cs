using FPT_Book_Store.Data;
using FPT_Book_Store.Models;
using FPT_Book_Store.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
   options.UseSqlServer(connectionString));

// builder.Services.AddTransient<ApplicationDbContext>();


//builder.Services.AddDefaultIdentity<Account>(options => options.SignIn.RequireConfirmedAccount = true)
//.AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();


builder.Services.AddIdentity<Accounts, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI().AddDefaultTokenProviders();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddControllersWithViews();

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

app.UseAuthentication();
app.UseAuthorization();

// app.MapAreaControllerRoute(
//             name: "Admin",
//             areaName: "Admin",
//             pattern: "Admin/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

// app.UseEndpoints(endpoints =>
// {
//     endpoints.MapDefaultControllerRoute();
//     endpoints.MapControllerRoute(
//         name: "MyArea",
//         pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");
// });



using (var scope = app.Services.CreateScope())
{
    await SeedRole.SeedRolesAndAdminAsync(scope.ServiceProvider);
}



app.Run();
