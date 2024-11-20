using Dashboard.BLL.Middlewares;
using Dashboard.BLL.Services.AccountService;
using Dashboard.BLL.Services.CategoryService;
using Dashboard.BLL.Services.EmailService;
using Dashboard.BLL.Services.ImageService;
using Dashboard.BLL.Services.JwtService;
using Dashboard.BLL.Services.ProductService;
using Dashboard.BLL.Services.RoleService;
using Dashboard.BLL.Services.TestService;
using Dashboard.BLL.Services.UserService;
using Dashboard.DAL;
using Dashboard.DAL.Initializer;
using Dashboard.DAL.Models.Identity;
using Dashboard.DAL.Repositories.CategoryRepository;
using Dashboard.DAL.Repositories.ProductRepository;
using Dashboard.DAL.Repositories.RoleRepository;
using Dashboard.DAL.Repositories.TestRepository;
using Dashboard.DAL.Repositories.UserRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add database context
builder.Services.AddDbContext<AppDbContext>(options =>
{
    //options.UseNpgsql("name=PostgreSqlAWS");
    options.UseNpgsql("name=PostgreSqlLocal");
    //options.UseNpgsql("name=PostgreSqlElephant");
    //options.UseSqlServer("name=MsSql");
});

// Add CORS
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://34.236.159.53:3000", "http://pd324dashboard.pp.ua", "http://pd324dashboard.pp.ua:3000", "http://localhost:3000")
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                      });
});

// Add identity
builder.Services.AddIdentity<User, Role>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Add jwt service
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            RequireExpirationTime = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["AuthSettings:key"])),
            ValidAudience = builder.Configuration["AuthSettings:audience"],
            ValidIssuer = builder.Configuration["AuthSettings:issuer"],
            ValidateLifetime = true
        };
    });

// Add services to the container.
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<ITestService, TestService>();

// Add repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ITestRepository, TestRepository>();

builder.Services.AddControllers();

// Add automapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Dashboard", Version = "v1" });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter JWT token by template: Bearer {your token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new []{ Settings.AdminRole }
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(MyAllowSpecificOrigins);

app.UseMiddleware<MiddlewareExceptionHandling>();
app.UseMiddleware<MiddlewareJwtTokenExceptionHandling>();

//app.UseStaticFiles("/wwwroot");

app.UseStaticFiles(new StaticFileOptions
{
    // шлях до папки з файлами
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.WebRootPath, "images", "users")),

    // URL адреса до цієї папки
    RequestPath = "/user"
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.SeedData();

app.Run();