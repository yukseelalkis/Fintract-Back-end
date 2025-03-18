// Web uygulaması için bir yapılandırıcı (builder) oluşturur
using api.Data;
using api.Interfaces;
using api.models;
using api.Repository;
using api.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

/*
 * Servisleri Dependency Injection (DI) konteynerine ekler.
 * Bu, ASP.NET Core'un hizmetleri yönetmesine ve bağımlılıkları otomatik olarak enjekte etmesine yardımcı olur.
 */

 //Controllerrimizi ekliyoruz

 builder.Services.AddControllers();

// API endpoint'lerini keşfetmek için kullanılır. Swagger gibi araçların API'yi tanımasını sağlar.
builder.Services.AddEndpointsApiExplorer();

// Swagger/OpenAPI desteği ekleniyor. Bu sayede API dokümantasyonu otomatik olarak oluşturulabilir.
builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });

    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,  // Token'ın nereye ekleneceğini belirtiyor (Header içinde olacak)
        Description = "Please enter a valid token",  // Swagger'da gözükecek açıklama
        Name = "Authorization",  // Header'da kullanılacak isim (Genellikle "Authorization" olur)
        Type = SecuritySchemeType.Http,  // Güvenlik türü HTTP olacak
        BearerFormat = "JWT",  // Bearer token formatı JWT olacak
        Scheme = "Bearer"  // Kullanılacak kimlik doğrulama şeması Bearer
    });

    option.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[] {}
        }
    });
});



/// **Veritabanı Bağlantısı Ekleniyor**
/// 
/// - `ApplicationDBContext`, Entity Framework Core (EF Core) kullanılarak DI sistemine ekleniyor.
/// - `UseSqlServer()`, SQL Server veritabanı kullanılarak bağlanılacağını belirtiyor.
/// - `GetConnectionString("DefaultConnection")`, bağlantı dizesini **appsettings.json** içinden alır.

builder.Services.AddDbContext<ApplicationDBContex>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//Bu kod, Dependency Injection (DI) mekanizmasında IStockRepository arayüzünü, StockRepository sınıfı ile eşleştirerek bağımlılık yönetimini sağlar.
//ASP.NET Core'da bağımlılıkları kaydetmek için 3 farklı yöntem vardır:
//1️⃣ AddScoped<> → Her HTTP isteği için tek bir nesne oluşturur.
//2️⃣ AddTransient<> → Her kullanımda yeni bir nesne oluşturur.
//3️⃣ AddSingleton<> → Uygulama süresince tek bir nesne oluşturur.
builder.Services.AddScoped<IStockRepository , StockRepository>();
builder.Services.AddScoped<ICommentRepository , CommentRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IPortfolioRepository , PortfolioRepository>();
builder.Services.AddScoped<IFMPService ,FMPService >();
builder.Services.AddHttpClient<IFMPService,FMPService>();


builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    });

builder.Services.AddIdentity<AppUsers, IdentityRole>(options =>{
    //Şifrede en az bir rakam (0-9) bulunmalı.
    options.Password.RequireDigit= true;
    //Şifrede en az bir küçük harf (a-z) olmalı.
    options.Password.RequireLowercase = true;
    //Şifrede en az bir büyük harf (A-Z) olmalı.
    options.Password.RequireUppercase= true;
    //Şifrede en az bir özel karakter (!@#$% vs.) bulunmalı.
    options.Password.RequireNonAlphanumeric= true;
    //Şifre en az 12 karakter uzunluğunda olmalı.
    options.Password.RequiredLength = 8;

})
.AddEntityFrameworkStores<ApplicationDBContex>();   

//

builder.Services.AddAuthentication(options =>{
    options.DefaultAuthenticateScheme=
    options.DefaultChallengeScheme=
    options.DefaultForbidScheme=
    options.DefaultScheme=
    options.DefaultSignInScheme=
    options.DefaultSignOutScheme=JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(
    options=>{
        options.TokenValidationParameters = new TokenValidationParameters{
            ValidateIssuer= true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidateAudience= true,
            ValidAudience=  builder.Configuration["JWT:Audience"],
            ValidateIssuerSigningKey= true,
            IssuerSigningKey = new SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"])
            ),
        };
    });

// Uygulama nesnesini oluşturur (Dependency Injection, Middleware'ler ve diğer yapılandırmaları hazır hale getirir).
var app = builder.Build();

/*
 * HTTP İstek hattını (Middleware Pipeline) yapılandırıyoruz.
 * Middleware'ler, gelen HTTP isteklerini işler ve yanıtlar üretir.
 */

// Eğer uygulama geliştirme ortamında çalışıyorsa (Development Mode) aşağıdaki ayarları uygula.
if (app.Environment.IsDevelopment())
{
    // Swagger API dokümantasyonunu etkinleştir.
    app.UseSwagger();
 
    // Swagger UI'yi etkinleştirerek geliştiricilerin API'yi test etmesine olanak tanır.
    app.UseSwaggerUI();

}
// HTTP'den HTTPS'e yönlendirme yaparak güvenliği artırır.
app.UseHttpsRedirection();

// aciklamalar
app.UseAuthentication();
app.UseAuthorization();
 // burasi degisti buranin aciklamasini eklemiz lazim 

app.MapControllers();

/*
 * Uygulamayı çalıştırır ve gelen istekleri dinlemeye başlar.
 * Buraya kadar tanımlanan middleware'ler ve yapılandırmalar devreye girer.
 */


app.Run();
