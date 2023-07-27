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
using domain.ArticleAdventure.Repositories;
using service.ArticleAdventure.Services.UserManagement.Contracts;
using service.ArticleAdventure.Services.UserManagement;
using common.ArticleAdventure.Helpers;
using domain.ArticleAdventure.IdentityEntities;
using Microsoft.AspNetCore.Identity;
using common.ArticleAdventure.IdentityConfiguration;
using domain.ArticleAdventure.Entities;
using System.Data;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.SetBasePath(builder.Environment.ContentRootPath);
builder.Configuration.AddJsonFile("appsettings.json");
builder.Configuration.AddEnvironmentVariables();

common.ArticleAdventure.Helpers.ConfigurationManager.SetAppSettingsProperties(builder.Configuration);
common.ArticleAdventure.Helpers.ConfigurationManager.SetAppEnvironmentRootPath(builder.Environment.ContentRootPath);

builder.Services.AddDbContext<ArticleAdventureDataContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString(ConnectionStringNames.DbConnectionString)));

builder.Services.AddDbContext<BusinessContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString(ConnectionStringNames.DbConnectionString)));

builder.Services
               .AddIdentity<User, IdentityRole>(options =>
               {
                   options.SignIn.RequireConfirmedAccount = false;

                   options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._ @+";
                   options.Password.RequireDigit = true;
                   options.Password.RequiredLength = 8;
                   options.Password.RequireLowercase = true;
                   options.Password.RequireUppercase = true;
                   options.Password.RequireNonAlphanumeric = false;
                   options.Password.RequiredUniqueChars = 0;
               })
               .AddEntityFrameworkStores<BusinessContext>()
               .AddDefaultTokenProviders();
ConfigureJwtAuthService(builder.Services);

builder.Services.AddCors(options =>
{
    options.AddPolicy("Allowed", builder =>
    {
        builder
            .WithOrigins("https://localhost:7261/")
            .AllowAnyHeader()
            .AllowCredentials()
            .AllowAnyMethod();
    });
});

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
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizePage("/");
});
builder.Services.AddScoped<IResponseFactory, ResponseFactory>();

builder.Services.AddTransient<IBlogRepositoryFactory, BlogRepositoryFactory>();

builder.Services.AddTransient<IIdentityRepository, IdentityRepository>();
builder.Services.AddTransient<IIdentityRepositoriesFactory, IdentityRepositoriesFactory>();


builder.Services.AddScoped<IRequestTokenService, RequestTokenService>();
builder.Services.AddScoped<IUserProfileService, UserProfileService>();
builder.Services.AddScoped<IBlogService, BlogService>();
builder.Services.AddTransient<IDbConnectionFactory, DbConnectionFactory>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings  
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

    options.LoginPath = "/Authentication/Login";  //set the login page.  
    //options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.SlidingExpiration = true;
});

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var connectionFactory = app.Services.GetRequiredService<IDbConnectionFactory>();
    var userManagement = (UserManager<User>)scope.ServiceProvider.GetRequiredService(typeof(UserManager<User>));
    var roleManager = (RoleManager<IdentityRole>)scope.ServiceProvider.GetRequiredService(typeof(RoleManager<IdentityRole>));
    var identityRepositoriesFactory = (IIdentityRepositoriesFactory)scope.ServiceProvider.GetRequiredService(typeof(IIdentityRepositoriesFactory));

    InitializeDefaultIdentityDataIfNotExists(userManagement, roleManager, connectionFactory, identityRepositoriesFactory);
}
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
app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
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
app.MapControllerRoute(
    name: "login",
    pattern: "{controller}/{action}/{id?}",
    new { controller = "Authentication", action = "Authentication" });
app.Run();


void ConfigureJwtAuthService(IServiceCollection services)
{
    SymmetricSecurityKey signingKey = AuthOptions.GetSymmetricSecurityKey();

    TokenValidationParameters tokenValidationParameters = new TokenValidationParameters
    {
        // The signing key must match!  
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = signingKey,

        // Validate the JWT Issuer (iss) claim  
        ValidateIssuer = true,
        ValidIssuer = AuthOptions.ISSUER,

        // Validate the JWT Audience (aud) claim  
        ValidateAudience = true,
        ValidAudience = AuthOptions.AUDIENCE_LOCAL,

        // Validate the token expiry  
        ValidateLifetime = true,

        ClockSkew = TimeSpan.Zero
    };

    services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
       
    }).AddJwtBearer(options => { options.TokenValidationParameters = tokenValidationParameters; })
    .AddCookie(options => { 
        options.LoginPath = new PathString("/Authentication/Login");
    });
}

void InitializeDefaultIdentityDataIfNotExists(
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            IDbConnectionFactory connectionFactory,
            IIdentityRepositoriesFactory identityRepositoriesFactory)
{
    try
    {
        //Creating pre-defined roles
        if (!roleManager.Roles.Any(r => r.Name == IdentityRoles.User))
        {
            roleManager.CreateAsync(new IdentityRole
            {
                Name = IdentityRoles.User
            }).Wait();
        }

        if (!roleManager.Roles.Any(r => r.Name == IdentityRoles.Administrator))
        {
            roleManager.CreateAsync(new IdentityRole
            {
                Name = IdentityRoles.Administrator
            }).Wait();
        }

        if (userManager.Users.Any(u => u.DisplayName == "Administrator")) return;

        UserProfile profile = new UserProfile
        {
            GrantAdministrativePermissions = true,
            Email = "admin@mail.com",
            UserName = "Administrator"
        };

        using (IDbConnection connection = connectionFactory.NewSqlConnection())
        {
            IUserProfileRepository userProfileRepository =
                identityRepositoriesFactory
                    .NewUserProfileRepository(connection);

            profile.Id = userProfileRepository.Add(profile);

            profile = userProfileRepository.Get(profile.Id);
        }

        User user = new User
        {
            DisplayName = "Administrator",
            UserName = "admin@mail.com",
            Email = "admin@mail.com",
            NetId = profile.NetUid
        };

        IdentityResult identityResult = userManager.CreateAsync(user, "Grimm_jow92").Result;

        if (!identityResult.Succeeded) return;

        userManager.AddToRoleAsync(user, IdentityRoles.Administrator).Wait();

        userManager.AddClaimAsync(user, new Claim("NetId", profile.NetUid.ToString())).Wait();
    }
    catch (Exception)
    {
        //Ignored
    }
}