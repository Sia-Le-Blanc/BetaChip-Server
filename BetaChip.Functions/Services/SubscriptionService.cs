using BetaChip.Functions.Models; // 수정된 모델 참조
using Supabase;
using System.Threading.Tasks;

namespace BetaChip.Functions.Services // 네임스페이스 통일
{
    public class SubscriptionService
    {
        private readonly Supabase.Client _supabase;

        public SubscriptionService(Supabase.Client supabase)
        {
            _supabase = supabase;
        }

        public async Task<Subscription?> GetSubscriptionAsync(string userId)
        {
            var response = await _supabase
                .From<Subscription>()
                .Where(x => x.Id == userId)
                .Get();

            return response.Model;
        }

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