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
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient<IAuthService, AuthService>();
builder.Services.AddHttpClient<IArticleService, ArticleService>();
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

//app.MapControllerRoute(
//    name: "images",
//    pattern: "{controller}/{action}/{id?}",
//    new { controller = "Images", action = "Images" });

//app.MapControllerRoute(
//    name: "new",
//    pattern: "{controller}/{action}/{id?}",
//    new { controller = "New", action = "New" });

//app.MapControllerRoute(
//    name: "tags",
//    pattern: "{controller}/{action}/{id?}",
//    new { controller = "Tags", action = "Tags" });


app.Run();
