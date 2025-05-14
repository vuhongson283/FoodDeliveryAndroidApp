using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using PRM_API.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Newtonsoft.Json;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Đọc chuỗi kết nối từ appsettings.json
var connectionString = builder.Configuration.GetConnectionString("MyCnn");

// Thêm DbContext vào DI container
builder.Services.AddDbContext<FoodDeliveryContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(2);
});
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(7191); // Cho phép HTTP
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ThisIsAVeryStrongSecretKey12345!")), // 🔥 Thay bằng key thực tế
            ValidateIssuer = false, // Nếu có Issuer, đặt thành true
            ValidateAudience = false, // Nếu có Audience, đặt thành true
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization(); // Quan trọng ⚠️


// Cấu hình Swagger để hỗ trợ Bearer Token
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Nhập token vào đây, không cần gõ 'Bearer ' (VD: eyJhbGciOiJIUzI1NiIsInR...)",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,  // Sửa từ ApiKey thành Http
        Scheme = "bearer",  // Đảm bảo viết thường
        BearerFormat = "JWT" // Định dạng token
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
