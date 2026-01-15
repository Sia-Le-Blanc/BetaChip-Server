using Supabase;
using BetaChip.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Cloudtype 환경에서 포트 설정 읽기 (기본값 8080)
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

// 서비스 등록
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Supabase 클라이언트 등록
builder.Services.AddScoped(_ => 
{
    var url = builder.Configuration["SupabaseUrl"];
    var key = builder.Configuration["SupabaseKey"];
    
    if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(key))
    {
        throw new InvalidOperationException("Supabase 설정(Url/Key)이 환경 변수에 등록되지 않았습니다.");
    }

    return new Supabase.Client(url, key, new SupabaseOptions
    {
        AutoRefreshToken = true,
        AutoConnectRealtime = true
    });
});

builder.Services.AddScoped<SubscriptionService>();

var app = builder.Build();

// 개발 환경에서만 Swagger 사용
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 중요: Cloudtype에서는 SSL이 외부에서 처리되므로 이 라인은 주석 처리하거나 제거합니다.
// app.UseHttpsRedirection(); 

app.UseAuthorization();
app.MapControllers();

app.Run();