using Hearthstone_Deck_Tracker.Utility.BoardDamage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginExample
{
    class Minion
    {
        public int Attack { get; set}
        public int Health { get; }
        public bool Windfury { get; }
        public bool Taunt { get; }
        public bool isBoardCard { get; }

        public Minion(BoardCard card)
        {
            Attack = card.Attack;
            Health = card.Health;
            Windfury = card.Windfury;
            Taunt = card.Taunt;
            isBoardCard = true;
        }

        public Minion(IBoardEntity card)
        {
            isBoardCard = false;
            List<IBoardEntity> random = new List<IBoardEntity>();
            random.Add(card);
            random.Add(card);
            foreach (BoardCard c in random)
            {
                Attack = c.Attack;
                Health = c.Health;
                Windfury = c.Windfury;
                Taunt = c.Taunt;
                isBoardCard = true;
                break;
            }
        }

    }
}
