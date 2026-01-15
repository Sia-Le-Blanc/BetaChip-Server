using Supabase;
using BetaChip.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// [핵심] 클라우드타입 환경 설정: 포트 번호를 환경 변수에서 읽어오고 0.0.0.0으로 바인딩합니다.
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

// 서비스 등록
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Supabase 클라이언트 등록 (환경 변수에서 보안 정보를 읽어옵니다.)
builder.Services.AddScoped(_ => 
{
    var url = builder.Configuration["SupabaseUrl"];
    var key = builder.Configuration["SupabaseKey"];
    
    if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(key))
    {
        throw new InvalidOperationException("SupabaseUrl 또는 SupabaseKey가 설정되지 않았습니다. 클라우드타입 환경 변수를 확인하세요.");
    }

    return new Supabase.Client(url, key, new SupabaseOptions
    {
        AutoRefreshToken = true,
        AutoConnectRealtime = true
    });
});

builder.Services.AddScoped<SubscriptionService>();

var app = builder.Build();

// Swagger 설정 (필요 시)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// [주의] 클라우드타입은 자체적으로 HTTPS를 지원하므로, 코드 내 리다이렉션은 비활성화합니다.
// app.UseHttpsRedirection(); 

app.UseAuthorization();
app.MapControllers();

app.Run();