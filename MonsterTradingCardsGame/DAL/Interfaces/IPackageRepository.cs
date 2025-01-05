using MonsterTradingCardsGame.Models;

namespace MonsterTradingCardsGame.DAL.Interfaces
{
    public interface IPackageRepository
    {
        void CreatePackage(Package package);
        Package? AcquirePackage();
        void DeletePackage(Guid cardId);
    }
}
