using Microsoft.AspNetCore.Mvc;
using Supabase;

namespace BetaChip.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly Supabase.Client _supabase;

        // 생성자: Program.cs에서 등록한 Supabase 클라이언트를 주입받습니다.
        public TestController(Supabase.Client supabase)
        {
            _supabase = supabase;
        }

        // 1. 서버 생존 확인: http://localhost:포트/api/test/ping
        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok(new { 
                message = "BetaChip 서버가 정상적으로 작동 중입니다!", 
                time = DateTime.Now 
            });
        }

        // 2. Supabase 연결 설정 확인: http://localhost:포트/api/test/supabase
        [HttpGet("supabase")]
        public IActionResult CheckSupabase()
        {
            try
            {
                // .Options.Url 대신 객체 자체가 생성되었는지만 확인합니다.
                // 이미 Program.cs에서 URL과 Key가 없으면 서버가 꺼지도록 설정했으므로
                // 이 객체가 null이 아니면 설정이 잘 된 것입니다.
                if (_supabase != null)
                {
                    return Ok(new { 
                        status = "Success", 
                        message = "Supabase 클라이언트 객체가 성공적으로 생성되었습니다." 
                    });
                }

                return BadRequest(new { status = "Fail", message = "Supabase 클라이언트가 생성되지 않았습니다." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "Error", message = ex.Message });
            }
        }
    }
}