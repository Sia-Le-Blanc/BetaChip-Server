using BetaChip.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace BetaChip.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubscriptionController : ControllerBase
    {
        private readonly SubscriptionService _subscriptionService;

        public SubscriptionController(SubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSubscription(string id)
        {
            var subscription = await _subscriptionService.GetSubscriptionAsync(id);

            if (subscription == null)
            {
                return NotFound(new { message = "해당 유저의 구독 정보를 찾을 수 없습니다." });
            }

            // ★ 수정된 부분: BaseModel의 내부 정보를 제외하고 필요한 필드만 새 객체로 만들어 반환합니다.
            return Ok(new {
                id = subscription.Id,
                email = subscription.Email,
                tier = subscription.Tier,
                hwid = subscription.Hwid,
                expiresAt = subscription.ExpiresAt
            });
        }
    }
}