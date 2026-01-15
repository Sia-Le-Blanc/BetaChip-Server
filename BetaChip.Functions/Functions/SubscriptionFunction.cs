using System.Net;
using BetaChip.Functions.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace BetaChip.Functions
{
    public class SubscriptionFunction
    {
        private readonly SubscriptionService _subscriptionService;
        private readonly ILogger _logger;

        public SubscriptionFunction(SubscriptionService subscriptionService, ILoggerFactory loggerFactory)
        {
            _subscriptionService = subscriptionService;
            _logger = loggerFactory.CreateLogger<SubscriptionFunction>();
        }

        // GET https://<앱이름>.azurewebsites.net/api/subscription/{id}
        [Function("GetSubscription")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "subscription/{id}")] HttpRequestData req,
            string id)
        {
            _logger.LogInformation($"구독 정보 조회 요청: {id}");

            var subscription = await _subscriptionService.GetSubscriptionAsync(id);

            if (subscription == null)
            {
                var response = req.CreateResponse(HttpStatusCode.NotFound);
                await response.WriteAsJsonAsync(new { message = "해당 유저의 구독 정보를 찾을 수 없습니다." });
                return response;
            }

            var okResponse = req.CreateResponse(HttpStatusCode.OK);
            // JSON 직렬화 에러 방지를 위해 필요한 데이터만 담은 객체 반환
            await okResponse.WriteAsJsonAsync(new
            {
                id = subscription.Id,
                email = subscription.Email,
                tier = subscription.Tier,
                hwid = subscription.Hwid,
                expiresAt = subscription.ExpiresAt
            });

            return okResponse;
        }
    }
}