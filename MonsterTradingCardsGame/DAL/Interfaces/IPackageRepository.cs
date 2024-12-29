using MonsterTradingCardsGame.Models;

namespace MonsterTradingCardsGame.DAL.Interfaces
{
    internal interface IPackageRepository
    {
        void CreatePackage(List<Card> cards);
        List<Card> GetPackage();
        void DeletePackage(string packageId);
    }
}
