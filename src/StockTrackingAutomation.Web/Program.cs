using Application.Services;
using Domain.Abstract;
using EasMe.Logging;
using Infrastructure;
using StockTrackingAutomation.Web.Filters;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews(x =>
{
    x.Filters.Add<ExceptionHandleFilter>();
});

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(720);
    options.Cookie.HttpOnly = true;
    // Make the session cookie essential
    options.Cookie.IsEssential = true;
});
builder.Services.AddMemoryCache();
builder.Services.AddDataProtection();
builder.Services.AddDistributedMemoryCache();
//AppDomain.CurrentDomain.AddUnexpectedExceptionHandling();
//builder.Services.AddControllers(x =>
//{
//    x.Filters.Add(new ExceptionHandleFilter());
//});
//ADD Business services dependency
builder.Services.AddScoped<IPurchaseService, PurchaseService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IDebtService, DebtService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ISaleService, SaleService>();
builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddDbContext<BusinessDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();
app.UseCookiePolicy();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

BusinessDbContext.EnsureCreated();


app.Run();

EasLogFactory.StaticLogger.Info("Exiting...");