using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using StudentRegisteration.Data;
using StudentRegisteration.Interfaces;
using StudentRegisteration.Models;
using StudentRegisteration.Repository;
using StudentRegisteration.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
options
    .UseSqlServer(builder.Configuration
    .GetConnectionString("DefaultConnection")));





// Add Session
builder.Services.AddSession(options => 
{
    options.Cookie.HttpOnly = true;
});
builder.Services.AddDistributedMemoryCache();
builder.Services.Configure<SessionOptions>(option =>
{

    option.IdleTimeout = TimeSpan.FromMinutes(30);  
    
});




// interface and Service implamination

builder.Services.AddScoped<IUserService<User>, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<IAddressService, AddressServices>();

builder.Services.AddScoped<ICourseOfferingRepository, CourseOfferingRepository>();
builder.Services.AddScoped<ICourseOfferingService, CourseOfferingService>();

builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<ICourseService, CourseService>();

builder.Services.AddScoped<IStudentDetailsRepository, StudentDetailsRepository>();
builder.Services.AddScoped<IStudentDetailsService, StudentDetailsService>( 
    provider => new StudentDetailsService(
        provider.GetRequiredService<IStudentDetailsRepository>(),
        provider.GetRequiredService<IWebHostEnvironment>()
    ));

builder.Services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();

builder.Services.AddScoped<IEnrollmentCourseRepository, EnrollmentCourseRepository>();
builder.Services.AddScoped<IEnrollmentCourseService, EnrollmentCourseService>();

builder.Services.AddScoped<IMarkRepository, MarkRepository>();
builder.Services.AddScoped<IMarkService, MarkService>();

// builder.Services.AddScoped<ICourseService, CourseService>();
// builder.Services.AddScoped<IStudentService, StudentService>();





var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseStaticFiles( new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(app.Environment.WebRootPath, "StudentImages")),
    RequestPath = "/StudentImages",
});

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
