using MonsterTradingCardsGame.Models;

namespace MonsterTradingCardsGame.DAL.Interfaces
{
    public interface ITradingDealRepository
    {
        bool TradingDealExists(Guid tradingDealId);
        void AddTradingDeal(TradingDeal tradingDeal);
        TradingDeal? GetTradingDeal(Guid tradingDealId);
        List<TradingDeal> GetAllTradingDeals();
        void DeleteTradingDeal(Guid tradingDealId);
    }
}
