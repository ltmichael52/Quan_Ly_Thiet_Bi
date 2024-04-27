using Microsoft.EntityFrameworkCore;
using ThietBiDienTu_2.Areas.Admin.InterfaceRepositories;
using ThietBiDienTu_2.Areas.Admin.Repositories;
using ThietBiDienTu_2.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IOTimeout = TimeSpan.FromMinutes(15); //thoi gian ton tai
    options.Cookie.IsEssential = true;
});

builder.Services.AddDbContext<ToolDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Tool"));
});

builder.Services.AddSession();
// Register HttpContextAccessor

builder.Services.AddScoped<ICoSoAdmin, CoSoAdminRepo>();
builder.Services.AddScoped<IPhieuMuonAdmin, PhieuMuonAdminRepo>();
builder.Services.AddScoped<IPhongAdmin, PhongAdminRepo>();
builder.Services.AddScoped<INhanVien, NhanvienAdminRepo>();
builder.Services.AddScoped<IDongThietBiAdmin, DongThietBiAdminRepo>();
builder.Services.AddScoped<IThietBiAdmin, ThietBiAdminRepo>();
builder.Services.AddScoped<IKhoa, KhoaAdminRepo>();
builder.Services.AddHttpContextAccessor();
var app = builder.Build();
app.UseSession();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseSession();

app.MapAreaControllerRoute(
    name: "Admin",
    areaName: "Admin",
    pattern: "admin/{controller=Coso}/{action=Index}/{id?}"
    );
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Histroy}/{action=Index}/{id?}");





app.Run();
