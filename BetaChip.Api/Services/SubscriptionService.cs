using BetaChip.Api.Models;
using Supabase;

namespace BetaChip.Api.Services
{
    public class SubscriptionService
    {
        private readonly Supabase.Client _supabase;

        public SubscriptionService(Supabase.Client supabase)
        {
            _supabase = supabase;
        }

        // 유저 고유 ID(UUID)를 받아 구독 정보를 가져오는 함수
        public async Task<Subscription?> GetSubscriptionAsync(string userId)
        {
            // DB의 subscriptions 테이블에서 id가 일치하는 첫 번째 데이터를 가져옵니다.
            var response = await _supabase
                .From<Subscription>()
                .Where(x => x.Id == userId)
                .Get();

            return response.Model;
        }

        // 유저의 하드웨어 ID(HWID)를 업데이트하는 함수 (중복 로그인 방지용)
        public async Task UpdateHwidAsync(string userId, string hwid)
        {
            var subscription = await GetSubscriptionAsync(userId);
            if (subscription != null)
            {
                subscription.Hwid = hwid;
                await subscription.Update<Subscription>();
            }
        }
    }
}