using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebsiteGenZ.Models;
using WebsiteGenZ.Repository;
using WebsiteGenZ.Services.uploadImage;
using WebsiteGenZ.Services.elasticsearch;
using Microsoft.Extensions.Options;
using CloudinaryDotNet;
using Nest;

var builder = WebApplication.CreateBuilder(args);

// ===== Cấu hình chuỗi kết nối tới cơ sở dữ liệu =====
builder.Services.AddDbContext<DataContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DbConnection");
    options.UseSqlServer(connectionString); // Sử dụng SQL Server với chuỗi kết nối được cấu hình
});

// ===== Cấu hình ElasticSearch =====
var elasticUri = new Uri("http://localhost:9200"); // Thay thế bằng URL Elasticsearch của bạn
var connectionSettings = new ConnectionSettings(elasticUri).DefaultIndex("products");
var elasticClient = new ElasticClient(connectionSettings);
builder.Services.AddSingleton(elasticClient);
builder.Services.AddSingleton<ElasticSearchRepository>();

// ===== Cấu hình Cloudinary =====
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));
builder.Services.AddSingleton(serviceProvider =>
{
    var config = serviceProvider.GetRequiredService<IOptions<CloudinarySettings>>().Value;
    var account = new Account(config.CloudName, config.ApiKey, config.ApiSecret);
    return new Cloudinary(account);
});

// ===== Cấu hình Identity =====
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    // Cấu hình mật khẩu
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 4;

    // Cấu hình người dùng
    options.User.RequireUniqueEmail = true;
})
    .AddEntityFrameworkStores<DataContext>()
    .AddDefaultTokenProviders(); // Thêm hỗ trợ tạo token cho xác thực

// ===== Cấu hình Session =====
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian hết hạn của session (30 phút)
    options.Cookie.HttpOnly = true; // Cookie session chỉ được truy cập qua HTTP
    options.Cookie.IsEssential = true; // Cookie session là bắt buộc
});

// ===== Đăng ký các dịch vụ khác =====
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages(); // Nếu bạn có Razor Pages
builder.Services.AddScoped<ProductRepository>();

// ===== Cấu hình Authorization =====
builder.Services.AddAuthorization(options =>
{
    // Thêm các quyền hạn tại đây nếu cần
});

var app = builder.Build();

// ===== Cấu hình xử lý lỗi =====
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error"); // Sử dụng trang lỗi chung khi không phải môi trường phát triển
}
else
{
    app.UseDeveloperExceptionPage(); // Hiển thị trang lỗi chi tiết trong môi trường phát triển
}

app.Use(async (context, next) =>
{
    var area = context.Request.RouteValues["area"]?.ToString();
    if (area == "Admin" && !context.User.IsInRole("Admin"))
    {
        context.Response.Redirect("/Account/Login");
        return;
    }
    await next();
});

app.UseStatusCodePagesWithRedirects("/Home/Error?statuscode={0}");

// ===== Pipeline HTTP =====
app.UseStaticFiles(); // Sử dụng file tĩnh (CSS, JS, hình ảnh)
app.UseRouting(); // Kích hoạt định tuyến
app.UseSession(); // Kích hoạt Session
app.UseAuthentication(); // Kích hoạt xác thực
app.UseAuthorization();  // Kích hoạt ủy quyền

// ===== Định tuyến =====
// Định tuyến cho các Areas
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

// Định tuyến cho category
app.MapControllerRoute(
    name: "category",
    pattern: "category/{slug?}",
    defaults: new { controller = "Category", action = "Index" });

// Định tuyến mặc định
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
