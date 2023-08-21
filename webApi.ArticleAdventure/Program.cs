using common.ArticleAdventure.Helpers;
using common.ArticleAdventure.IdentityConfiguration;
using common.ArticleAdventure.ResponceBuilder;
using common.ArticleAdventure.ResponceBuilder.Contracts;
using database.ArticleAdventure;
using domain.ArticleAdventure.DbConnectionFactory;
using domain.ArticleAdventure.DbConnectionFactory.Contracts;
using domain.ArticleAdventure.Entities;
using domain.ArticleAdventure.IdentityEntities;
using domain.ArticleAdventure.Repositories;
using domain.ArticleAdventure.Repositories.Blog;
using domain.ArticleAdventure.Repositories.Blog.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using service.ArticleAdventure.MailSenderServices.Contracts;
using service.ArticleAdventure.MailSenderServices;
using service.ArticleAdventure.Services.Blog;
using service.ArticleAdventure.Services.Blog.Contracts;
using service.ArticleAdventure.Services.UserManagement;
using service.ArticleAdventure.Services.UserManagement.Contracts;
using System.Data;
using System.Security.Claims;
using service.ArticleAdventure.Services.Tag.Contracts;
using service.ArticleAdventure.Services.Tag;
using domain.ArticleAdventure.Repositories.Identity.Contracts;
using domain.ArticleAdventure.Repositories.Identity;
using domain.ArticleAdventure.Repositories.Tag.Contracts;
using domain.ArticleAdventure.Repositories.Tag;
using Stripe;
using service.ArticleAdventure.Services.Stripe.Contracts;
using service.ArticleAdventure.Services.Stripe;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.SetBasePath(builder.Environment.ContentRootPath);
builder.Configuration.AddJsonFile("appsettings.json");
builder.Configuration.AddEnvironmentVariables();

common.ArticleAdventure.Helpers.ConfigurationManager.SetAppSettingsProperties(builder.Configuration);
common.ArticleAdventure.Helpers.ConfigurationManager.SetAppEnvironmentRootPath(builder.Environment.ContentRootPath);
StripeConfiguration.ApiKey = builder.Configuration.GetValue<string>("StripeSettings:SecretKey");

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
            .WithOrigins("https://localhost:7192/")
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

builder.Services.AddScoped<IResponseFactory, ResponseFactory>();

builder.Services
    .AddScoped<CustomerService>()
    .AddScoped<ChargeService>()
    .AddScoped<TokenService>()
    .AddScoped<IStripeService, StripeService>();
builder.Services.AddTransient<IArticleRepositoryFactory, ArticleRepositoryFactory>();
builder.Services.AddTransient<IMainArticleRepositoryFactory, MainArticleRepositoryFactory>();
builder.Services.AddTransient<ITagRepositoryFactory, TagRepositoryFactory>();
builder.Services.AddTransient<IMainArticleTagsFactory, MainArticleTagsFactory>();

builder.Services.AddScoped<IMailSenderFactory, MailSenderFactory>();

builder.Services.AddTransient<IIdentityRepository, IdentityRepository>();
builder.Services.AddTransient<IIdentityRepositoriesFactory, IdentityRepositoriesFactory>();


builder.Services.AddScoped<IRequestTokenService, RequestTokenService>();
builder.Services.AddScoped<IUserProfileService, UserProfileService>();
builder.Services.AddScoped<IArticleService, ArticleService>();
builder.Services.AddScoped<IMainArticleService, MainArticleService>();
builder.Services.AddScoped<ITagService, TagService>();

builder.Services.AddTransient<IDbConnectionFactory, DbConnectionFactory>();
builder.Services.AddEndpointsApiExplorer();
var app = builder.Build();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("Allowed");

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

app.UseHttpsRedirection();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
using (var scope = app.Services.CreateScope())
{
    var connectionFactory = app.Services.GetRequiredService<IDbConnectionFactory>();
    var userManagement = (UserManager<User>)scope.ServiceProvider.GetRequiredService(typeof(UserManager<User>));
    var roleManager = (RoleManager<IdentityRole>)scope.ServiceProvider.GetRequiredService(typeof(RoleManager<IdentityRole>));
    var identityRepositoriesFactory = (IIdentityRepositoriesFactory)scope.ServiceProvider.GetRequiredService(typeof(IIdentityRepositoriesFactory));

    InitializeDefaultIdentityDataIfNotExists(userManagement, roleManager, connectionFactory, identityRepositoriesFactory);
}
// Configure the HTTP request pipeline.




//app.MapRazorPages();
//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller}/{action}/{id?}",
//    new { controller = "Home", action = "Index" });

//app.MapControllerRoute(
//    name: "all",
//    pattern: "{controller}/{action}/{id?}",
//    new { controller = "All", action = "All" });

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
//app.MapControllerRoute(
//    name: "login",
//    pattern: "{controller}/{action}/{id?}",
//    new { controller = "Authentication", action = "Authentication" });
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
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // If using HTTPS
        options.LoginPath = new PathString("/Authentication/Login");
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
        options.AccessDeniedPath = "/Home/AccessDenied";
        options.Cookie.Name = "ApplicationCookie";
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
