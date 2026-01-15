using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Supabase;
using BetaChip.Functions.Services; // 수정된 서비스 참조

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        var supabaseUrl = Environment.GetEnvironmentVariable("SupabaseUrl") 
            ?? throw new InvalidOperationException("SupabaseUrl 설정이 없습니다.");
        var supabaseKey = Environment.GetEnvironmentVariable("SupabaseKey") 
            ?? throw new InvalidOperationException("SupabaseKey 설정이 없습니다.");

        services.AddScoped(_ => new Supabase.Client(supabaseUrl, supabaseKey, new SupabaseOptions
        {
            AutoRefreshToken = true,
            AutoConnectRealtime = true
        }));

        services.AddScoped<SubscriptionService>();
    })
    .Build();

host.Run();