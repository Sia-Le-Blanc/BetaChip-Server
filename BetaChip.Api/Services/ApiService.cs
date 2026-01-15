using System.Net.Http;
using System.Net.Http.Json;
using MosaicCensorSystem.Models;

namespace MosaicCensorSystem.Services
{
    public class ApiService
    {
        // HttpClient는 매번 새로 만드는 것보다 하나를 계속 쓰는 것이 성능상 좋습니다.
        private static readonly HttpClient _httpClient = new HttpClient 
        { 
            BaseAddress = new Uri("http://localhost:5020/") 
        };

        public async Task<SubscriptionInfo?> GetSubscriptionAsync(string userId)
        {
            try
            {
                // 서버의 api/subscription/{id} 호출
                return await _httpClient.GetFromJsonAsync<SubscriptionInfo>($"api/subscription/{userId}");
            }
            catch (Exception ex)
            {
                // 서버가 꺼져있거나 인터넷이 안 될 때 처리
                Console.WriteLine($"서버 연결 실패: {ex.Message}");
                return null;
            }
        }
    }
}