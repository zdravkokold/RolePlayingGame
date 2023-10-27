using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace RPG.Characters
{
    public class Monster : Character
    {
        public Monster(int positionX, int positionY) : base(0, 0, 0, 1, '◙')
        {
            Strenght = new Random().Next(1, 4);
            Agility = new Random().Next(1, 4);
            Intelligence = new Random().Next(1, 4);
            PositionX = positionX;
            PositionY = positionY;
        }
    }
}
