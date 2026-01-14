using Supabase;

var builder = WebApplication.CreateBuilder(args);

// 1. Supabase 설정 정보 읽기 (권장하는 방식: Null 검증 추가)
// 설정 파일(appsettings.json)에 값이 없으면 서버 실행 시 즉시 에러를 발생시킵니다.
var supabaseUrl = builder.Configuration["Supabase:Url"] 
    ?? throw new InvalidOperationException("Supabase URL이 설정되지 않았습니다. appsettings.json 파일을 확인하세요.");

var supabaseKey = builder.Configuration["Supabase:Key"] 
    ?? throw new InvalidOperationException("Supabase Key가 설정되지 않았습니다. appsettings.json 파일을 확인하세요.");

// 2. Supabase 클라이언트 등록
builder.Services.AddScoped(_ => new Supabase.Client(supabaseUrl, supabaseKey, new SupabaseOptions
{
    AutoRefreshToken = true,
    AutoConnectRealtime = true
}));

// 3. API 컨트롤러 및 도구 설정
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 4. HTTP 요청 파이프라인 설정
// 개발 환경(Development)에서만 Swagger(API 테스트 화면)를 활성화합니다.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 우리가 만들 로그인/구독 확인 API 컨트롤러들을 연결합니다.
app.MapControllers();

app.Run();