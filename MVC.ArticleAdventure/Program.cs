using MVC.ArticleAdventure.Services.Contract;
using MVC.ArticleAdventure.Services;
using MVC.ArticleAdventure.Extensions.Contract;
using MVC.ArticleAdventure.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Authentication;
using domain.ArticleAdventure.IdentityEntities;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<AppSettings>(builder.Configuration);
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".MyApp.Session";
    options.IdleTimeout = TimeSpan.FromSeconds(3600);
    options.Cookie.IsEssential = true;
});
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient<IAuthService, AuthService>();
builder.Services.AddHttpClient<IArticleService, ArticleService>();
builder.Services.AddHttpClient<IMainArticleService, MainArticleService>();
builder.Services.AddHttpClient<ITagService, TagService>();
builder.Services.AddHttpClient<IUserProfileService, UserProfileService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IUser, AspNetUser>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    //options.AuthenticationScheme = "MyCookieMiddlewareInstance";
                    //options.CookieName = "MyCookieMiddlewareInstance";
                    //options.AccessDeniedPath = new PathString("/Home/AccessDenied/");
                    //options.AutomaticAuthenticate = true;
                    //options.AutomaticChallenge = true;
                    //Add route
                    options.LoginPath = new PathString("/Login");
                    options.AccessDeniedPath = "/error/403";
                });
builder.Services.AddControllersWithViews();
builder.Services.AddAuthorization();
var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/error/500");
    app.UseStatusCodePagesWithRedirects("/error/{0}");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ExceptionMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action}/{id?}",
    new { controller = "Home", action = "Index" });
app.MapControllerRoute(
    name: "login",
    pattern: "{controller}/{action}/{id?}",
    new { controller = "All", action = "All" });
app.MapControllerRoute(
    name: "all",
    pattern: "{controller}/{action}/{id?}",
    new { controller = "Identity", action = "Login" });

app.MapControllerRoute(
    name: "images",
    pattern: "{controller}/{action}/{id?}",
    new { controller = "User", action = "User" });

app.MapControllerRoute(
    name: "tags",
    pattern: "{controller}/{action}/{id?}",
    new { controller = "Tags", action = "Tags" });

app.MapControllerRoute(
    name: "new",
    pattern: "{controller}/{action}/{id?}",
    new { controller = "Stripe", action = "Stripe" });




app.Run();
