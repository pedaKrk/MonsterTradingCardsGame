using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Models
{
    internal class Card
    {
        private readonly string _id;

        private readonly double _damage;

        private readonly string _name;

        private readonly Element _element;

        private readonly CardType _cardType;

        public Card(string id, string name, double damage)
        {
            _id = id;
            _name = name;
            _damage = damage;
            _element = DetermineElement(name);
            _cardType = DetermineCardType(name);
        }

        public string Id { get { return _id; } }
        public double Damage { get { return _damage; } }
        public string Name { get { return _name; } }
        public Element Element { get { return _element; } }


        private Element DetermineElement(string name)
        {
            //Pass Type as variable in req
            if (name.Contains("Fire"))
                return Element.Fire;
            if (name.Contains("Water"))
                return Element.Water;

            return Element.Normal;
        }
        private CardType DetermineCardType(string name)
        {
            //Pass Type as variable in req
            return name.Contains("Spell")
                ? CardType.SpellCard
                : CardType.MonsterCard;
        }

        public override string ToString()
        {
            return $"[{_cardType} Card] Id: {_id}, Name: {Name}, Damage: {Damage}, Element: {Element}";
        }

    }
}
