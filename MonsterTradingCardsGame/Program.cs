// See https://aka.ms/new-console-template for more information
using MonsterTradingCardsGame.Models;

Console.WriteLine("Hello, World!");

User UserA = new User("Player1", "123");

UserA.BuyPack();

Console.WriteLine(UserA.ToString());

