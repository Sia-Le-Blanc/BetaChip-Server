using BetaChip.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace BetaChip.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubscriptionController : ControllerBase
    {
        private readonly SubscriptionService _subscriptionService;

        // 생성자: 서버가 시작될 때 만들어둔 SubscriptionService를 가져옵니다.
        public SubscriptionController(SubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        // 유저 ID로 구독 정보를 조회하는 API
        // 예: GET http://localhost:5020/api/subscription/4e222613-7a83-4063-b717-d7e06bed0122
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSubscription(string id)
        {
            var subscription = await _subscriptionService.GetSubscriptionAsync(id);

            if (subscription == null)
            {
                return NotFound(new { message = "해당 유저의 구독 정보를 찾을 수 없습니다." });
            }

            return Ok(subscription);
        }
    }
}