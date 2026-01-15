using Supabase;
using BetaChip.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. 클라우드타입 포트 설정 (매우 중요)
// 클라우드타입은 PORT 환경 변수를 통해 포트를 지정합니다. 기본값은 8080입니다.
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

// 서비스 등록
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 2. Supabase 클라이언트 등록
// appsettings.json 또는 클라우드타입 환경 변수에서 설정을 읽어옵니다.
builder.Services.AddScoped(_ => 
{
    var url = builder.Configuration["SupabaseUrl"];
    var key = builder.Configuration["SupabaseKey"];
    
    if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(key))
    {
        throw new InvalidOperationException("SupabaseUrl 또는 SupabaseKey 설정이 없습니다. 환경 변수를 확인하세요.");
    }

    return new Supabase.Client(url, key, new SupabaseOptions
    {
        AutoRefreshToken = true,
        AutoConnectRealtime = true
    });
});

builder.Services.AddScoped<SubscriptionService>();

var app = builder.Build();

// Swagger 설정 (개발 환경 확인)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 3. HTTPS 리다이렉션 제거 (중요)
// 클라우드타입 자체에서 SSL(HTTPS)을 처리하므로, 코드에서는 중복 리다이렉션을 막기 위해 주석 처리합니다.
// app.UseHttpsRedirection(); 

app.UseAuthorization();
app.MapControllers();

app.Run();