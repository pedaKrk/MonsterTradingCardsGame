using MonsterTradingCardsGame.Models;

namespace MonsterTradingCardsGame.DAL.Interfaces
{
    internal interface ITradingDealRepository
    {
        bool TradingDealExists(string tradingDealId);
        void AddTradingDeal(TradingDeal tradingDeal);
        TradingDeal? GetTradingDeal(string tradingDealId);
        List<TradingDeal> GetAllTradingDeals();
        void DeleteTradingDeal(string tradingDealId);
    }
}
