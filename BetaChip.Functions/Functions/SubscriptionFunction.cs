using System.Net;
using BetaChip.Functions.Services; // 서비스 참조 추가
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