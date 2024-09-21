// See https://aka.ms/new-console-template for more information
using MonsterTradingCardsGame.Models;


User UserA = new User("Player1", "123");

UserA.BuyPack();
UserA.BuyPack();

Console.WriteLine(UserA.ToString());

