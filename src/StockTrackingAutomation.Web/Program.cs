using Application.Manager;
using Domain.Abstract;
using Domain.Entities;
using EasMe;
using EasMe.Logging;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllersWithViews();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(720);
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
builder.Services.AddScoped<IBuyLogMgr, BuyLogMgr>();
builder.Services.AddScoped<ICustomerMgr, CustomerMgr>();
builder.Services.AddScoped<IDebtLogMgr, DebtLogMgr>();
builder.Services.AddScoped<IProductMgr, ProductMgr>();
builder.Services.AddScoped<ISaleLogMgr, SaleLogMgr>();
builder.Services.AddScoped<ISupplierMgr, SupplierMgr>();
builder.Services.AddScoped<IUserMgr, UserMgr>();
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