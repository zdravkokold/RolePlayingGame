using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;

namespace RPG.Characters
{
    public abstract class Character
    {
        protected Character(int strenght, int agility, int intelligence, int range, char symbol)
        {
            Strenght = strenght;
            Agility = agility;
            Intelligence = intelligence;
            Range = range;
            Symbol = symbol;
            RemainingPoints = 3;
        }
        public int Id { get; set; }
        public int Strenght { get; protected set; }
        public int Intelligence { get; protected set; }
        public int Agility { get; protected set; }
        public int Range { get; protected set; }
        public char Symbol { get; protected set; }
        public int RemainingPoints { get; private set; }
        public int Health { get; set; }
        public int Mana { get; set; }
        public int Damage { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public DateTime TimeOfCreation { get; set; }


        public void Setup()
        {
            this.Health = this.Strenght * 5;
            this.Mana = this.Intelligence * 3;
            this.Damage = this.Agility * 2;
        }

        public bool AddPoints(int strengthPoints, int agilityPoints, int intelligencePoints)
        {
            if (strengthPoints + agilityPoints + intelligencePoints <= RemainingPoints)
            {
                Strenght += strengthPoints;
                Agility += agilityPoints;
                Intelligence += intelligencePoints;

                RemainingPoints -= (strengthPoints + agilityPoints + intelligencePoints);

                return true;
            }
            return false; 
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;
        }

        public bool IsDead => Health <= 0;
    }
}
