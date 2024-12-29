using MonsterTradingCardsGame.Models;

namespace MonsterTradingCardsGame.DAL.Interfaces
{
    internal interface ITradingDealRepository
    {
        bool TradingDealExists(string tradingDealId);
        void AddTradingDeal(TradingDeal tradingDeal);
        TradingDeal GetTradingDeal(string tradingDealId);
        void DeleteTradingDeal(string tradingDealId);
    }
}
