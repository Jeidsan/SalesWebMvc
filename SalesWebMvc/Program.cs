using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Data;
using SalesWebMvc.Services;
using System.Globalization;
using System.Linq.Expressions;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<SalesWebMvcContext>(options =>
    options.UseMySQL(
        builder.Configuration.GetConnectionString("SalesWebMvcContext") ?? throw new InvalidOperationException("Connection string 'SalesWebMvcContext' not found."),
        bd => bd.MigrationsAssembly("SalesWebMvc")
    )
);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<SellerService>();
builder.Services.AddScoped<DepartmentService>();
builder.Services.AddScoped<SalesRecordService>();

var enUS = new CultureInfo("en-US");
var localizationOptions = new RequestLocalizationOptions()
{
    DefaultRequestCulture = new RequestCulture(enUS),
    SupportedUICultures = new List<CultureInfo>() { enUS },
    SupportedCultures = new List<CultureInfo>() { enUS }
};


var app = builder.Build();

app.UseRequestLocalization(localizationOptions);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.Seed();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
