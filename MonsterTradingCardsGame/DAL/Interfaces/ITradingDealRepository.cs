using MonsterTradingCardsGame.Models;

namespace MonsterTradingCardsGame.DAL.Interfaces
{
    internal interface ITradingDealRepository
    {
        bool TradingDealExists(Guid tradingDealId);
        void AddTradingDeal(TradingDeal tradingDeal);
        TradingDeal? GetTradingDeal(Guid tradingDealId);
        List<TradingDeal> GetAllTradingDeals();
        void DeleteTradingDeal(Guid tradingDealId);
    }
}
