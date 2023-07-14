using common.ArticleAdventure.ResponceBuilder.Contracts;
using common.ArticleAdventure.ResponceBuilder;
using domain.ArticleAdventure.Repositories.Blog.Contracts;
using domain.ArticleAdventure.Repositories.Blog;
using service.ArticleAdventure.Services.Blog.Contracts;
using service.ArticleAdventure.Services.Blog;
using domain.ArticleAdventure.DbConnectionFactory.Contracts;
using domain.ArticleAdventure.DbConnectionFactory;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.SetBasePath(builder.Environment.ContentRootPath);
builder.Configuration.AddJsonFile("appsettings.json");
builder.Configuration.AddEnvironmentVariables();

common.ArticleAdventure.Helpers.ConfigurationManager.SetAppSettingsProperties(builder.Configuration);
common.ArticleAdventure.Helpers.ConfigurationManager.SetAppEnvironmentRootPath(builder.Environment.ContentRootPath);
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
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
