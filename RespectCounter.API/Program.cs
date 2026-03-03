using System.Text;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;

using RespectCounter.Application.Shared;
using RespectCounter.Application.Shared.Behaviors;
using RespectCounter.Application.Shared.Contracts;
using RespectCounter.Infrastructure;
using RespectCounter.Infrastructure.Repositories;
using RespectCounter.Infrastructure.Services;
using RespectCounter.Infrastructure.Identity;
using RespectCounter.API.Middleware;

using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "DevPolicy", policy  =>
    {
        policy.WithOrigins("http://localhost:3000", "https://localhost:3000");
    });
});

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always;
    options.Secure = CookieSecurePolicy.Always;
});

builder.Services.AddControllers(); // .AddJsonOptions(o => o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles); //to prevent reference cycle error

if (builder.Configuration["DB"] == "InMemory")
{
    builder.Services.AddDbContext<RespectDbContext>(options => options.UseInMemoryDatabase("RespectCounterDB"));
    Console.WriteLine("Initialization info: the server is going to use an in-memory database.");
}
else
{
    builder.Services.AddDbContext<RespectDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
}   

builder.Services.AddIdentity<CustomIdentityUser, IdentityRole<Guid>>().AddEntityFrameworkStores<RespectDbContext>();

builder.Services.AddScoped<IDatabaseInitializer, DatabaseInitializer>(provider =>
{
    var context = provider.GetRequiredService<RespectDbContext>();
    var imageService = provider.GetRequiredService<IImageService>();
    var logger = provider.GetRequiredService<ILogger<DatabaseInitializer>>();
    var env = provider.GetRequiredService<IWebHostEnvironment>();

    var solutionRoot = Path.GetDirectoryName(env.ContentRootPath) ?? ""; 
    var seedAssetsPath = Path.Combine(solutionRoot, "RespectCounter.Infrastructure", "SeedData", "images");

    return new DatabaseInitializer(context, logger, imageService, seedAssetsPath);
});
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IReadOnlyRepository, ReadOnlyRepository>();
builder.Services.AddScoped<IActivityRepository, ActivityRepository>();
builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<IReactionRepository, ReactionRepository>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddScoped<IImageService, LocalImageService>(provider =>
{
    var env = provider.GetRequiredService<IWebHostEnvironment>();
    return new LocalImageService(env.WebRootPath);
});
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(ApplicationLayer).Assembly);
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

var jwtSettingsSection = builder.Configuration.GetSection("Jwt");
var jwtSettings = jwtSettingsSection.Get<JwtSettings>();
builder.Services.Configure<JwtSettings>(jwtSettingsSection);

builder.Services.AddAuthentication(options => 
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}
).AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings?.Issuer ?? throw new Exception("Issuer cannot be empty"),
            ValidAudience = jwtSettings?.Audience ?? throw new Exception("Audience cannot be empty"),
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings.AccessTokenSecret ?? throw new Exception("AccessTokenSecret cannot be empty"))
            ),
        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                context.Token = context.Request.Cookies["AccessToken"];
                return Task.CompletedTask;
            }
        };
    }
);

builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

var app = builder.Build();

// Auto executing migrations
if (builder.Configuration["DB"] != "InMemory")
{
    using var scope = app.Services.CreateScope();
    var initializer = scope.ServiceProvider.GetRequiredService<IDatabaseInitializer>();
    await initializer.InitializeAsync();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

// HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles(); 
app.UseCookiePolicy();
app.UseHttpsRedirection();
app.UseCors("DevPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
