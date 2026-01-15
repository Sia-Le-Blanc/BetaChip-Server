using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Supabase;
using BetaChip.Api.Services;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        // 환경 변수(또는 local.settings.json)에서 설정 읽기
        var supabaseUrl = Environment.GetEnvironmentVariable("SupabaseUrl") 
            ?? throw new InvalidOperationException("SupabaseUrl 설정이 없습니다.");
        var supabaseKey = Environment.GetEnvironmentVariable("SupabaseKey") 
            ?? throw new InvalidOperationException("SupabaseKey 설정이 없습니다.");

        // Supabase 클라이언트 등록
        services.AddScoped(_ => new Supabase.Client(supabaseUrl, supabaseKey, new SupabaseOptions
        {
            AutoRefreshToken = true,
            AutoConnectRealtime = true
        }));

        // 비즈니스 로직 서비스 등록
        services.AddScoped<SubscriptionService>();
    })
    .Build();

host.Run();