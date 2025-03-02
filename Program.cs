// Web uygulaması için bir yapılandırıcı (builder) oluşturur
using api.Data;
using api.Interfaces;
using api.Repository;
using Microsoft.EntityFrameworkCore;

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


 // burasi degisti buranin aciklamasini eklemiz lazim 

app.MapControllers();

/*
 * Uygulamayı çalıştırır ve gelen istekleri dinlemeye başlar.
 * Buraya kadar tanımlanan middleware'ler ve yapılandırmalar devreye girer.
 */


app.Run();
