using AutoMapper;
using MyBlog.BLL.Models;
using MyBlog.DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyBlog.WebService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyBlog.WebService.Extentions;
using MyBlog.DAL.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging.Console;
using NLog.Extensions.Logging;
using NLog.Web;
using NLog;
using MyBlog.WebService.Middlewares;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Host.UseNLog();

var mapperConfig = new MapperConfiguration((v) =>
{
    v.AddProfile(new MappingProfile());
});

IMapper mapper = mapperConfig.CreateMapper();

builder.Services.AddRazorPages();
builder.Services.AddMvc().AddRazorRuntimeCompilation();

builder.Services.AddIdentity<User, Role>(opts => {
        opts.Password.RequiredLength = 5;
        opts.Password.RequireNonAlphanumeric = false;
        opts.Password.RequireLowercase = false;
        opts.Password.RequireUppercase = false;
        opts.Password.RequireDigit = false;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.ConfigureApplicationCookie(optons =>
{
    optons.LoginPath = "/User/Login";
    optons.AccessDeniedPath = "/Home/ErrorForbidden";
});

builder.Services.AddSingleton(mapper);

var loggerFactory
    = new LoggerFactory(new[] { new NLogLoggerProvider() });

string? connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(connection))
    .AddUnitOfWork()
        .AddCustomRepository<Article, ArticleRepository>()
        .AddCustomRepository<Comment, CommentRepository>()
        .AddCustomRepository<Tag, TagRepository>();

builder.Services.AddSwaggerGen();
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var userManager = services.GetRequiredService<UserManager<User>>();
        var roleManager = services.GetRequiredService<RoleManager<Role>>();
        if (!(await roleManager.RoleExistsAsync("employee")))
        {
            await roleManager.CreateAsync(new Role("employee"));
        }
        if (!(await roleManager.RoleExistsAsync("admin")))
        {
            await roleManager.CreateAsync(new Role("admin"));
        }
        if (!(await roleManager.RoleExistsAsync("moderator")))
        {
            await roleManager.CreateAsync(new Role("moderator"));
        }
    }
    catch (Exception ex)
    {
        logger.Error(ex, "An error occurred while seeding the database.");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/Error/{0}");

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

//надо загуглить
app.UseDeveloperExceptionPage();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<LogMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
