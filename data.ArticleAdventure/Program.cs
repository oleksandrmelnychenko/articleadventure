using common.ArticleAdventure.ResponceBuilder.Contracts;
using common.ArticleAdventure.ResponceBuilder;
using domain.ArticleAdventure.Repositories.Blog.Contracts;
using domain.ArticleAdventure.Repositories.Blog;
using service.ArticleAdventure.Services.Blog.Contracts;
using service.ArticleAdventure.Services.Blog;
using domain.ArticleAdventure.DbConnectionFactory.Contracts;
using domain.ArticleAdventure.DbConnectionFactory;
using Newtonsoft.Json.Serialization;
using database.ArticleAdventure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.SetBasePath(builder.Environment.ContentRootPath);
builder.Configuration.AddJsonFile("appsettings.json");
builder.Configuration.AddEnvironmentVariables();

common.ArticleAdventure.Helpers.ConfigurationManager.SetAppSettingsProperties(builder.Configuration);
common.ArticleAdventure.Helpers.ConfigurationManager.SetAppEnvironmentRootPath(builder.Environment.ContentRootPath);

builder.Services.AddDbContext<ArticleAdventureDataContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString(common.ArticleAdventure.Helpers.ConnectionStringNames.DbConnectionString)));

builder.Services
    .AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ContractResolver = new DefaultContractResolver();
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        options.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Local;
    });

builder.Services.AddResponseCompression();
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAntDesign();

builder.Services.AddScoped<IResponseFactory, ResponseFactory>();

builder.Services.AddTransient<IBlogRepositoryFactory, BlogRepositoryFactory>();

builder.Services.AddScoped<IBlogService, BlogService>();
builder.Services.AddTransient<IDbConnectionFactory, DbConnectionFactory>();


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

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action}/{id?}",
    new { controller = "Home", action = "Index"});

app.MapControllerRoute(
    name: "all",
    pattern: "{controller}/{action}/{id?}",
    new { controller = "All", action = "All" });

app.MapControllerRoute(
    name: "images",
    pattern: "{controller}/{action}/{id?}",
    new { controller = "Images", action = "Images" });

app.MapControllerRoute(
    name: "new",
    pattern: "{controller}/{action}/{id?}",
    new { controller = "New", action = "New" });

app.MapControllerRoute(
    name: "tags",
    pattern: "{controller}/{action}/{id?}",
    new { controller = "Tags", action = "Tags" });

app.Run();
