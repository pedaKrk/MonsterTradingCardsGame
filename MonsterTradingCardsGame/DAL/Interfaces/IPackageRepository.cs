using MonsterTradingCardsGame.Models;

namespace MonsterTradingCardsGame.DAL.Interfaces
{
    internal interface IPackageRepository
    {
        void CreatePackage(Package package);
        Package? AcquirePackage();
        void DeletePackage(Guid cardId);
    }
}
