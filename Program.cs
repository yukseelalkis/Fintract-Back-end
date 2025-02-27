// Web uygulaması için bir yapılandırıcı (builder) oluşturur
var builder = WebApplication.CreateBuilder(args);

/*
 * Servisleri Dependency Injection (DI) konteynerine ekler.
 * Bu, ASP.NET Core'un hizmetleri yönetmesine ve bağımlılıkları otomatik olarak enjekte etmesine yardımcı olur.
 */

// API endpoint'lerini keşfetmek için kullanılır. Swagger gibi araçların API'yi tanımasını sağlar.
builder.Services.AddEndpointsApiExplorer();

// Swagger/OpenAPI desteği ekleniyor. Bu sayede API dokümantasyonu otomatik olarak oluşturulabilir.
builder.Services.AddSwaggerGen();

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

/*
 * Uygulamayı çalıştırır ve gelen istekleri dinlemeye başlar.
 * Buraya kadar tanımlanan middleware'ler ve yapılandırmalar devreye girer.
 */
app.Run();
