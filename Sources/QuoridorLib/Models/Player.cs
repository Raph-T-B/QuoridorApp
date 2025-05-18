using System;

namespace QuoridorLib.Models
{
    public class Player
    {
        private readonly string name;
        private uint victories;

        public Player(string name)
        {
            this.name = name;
            this.victories = 0;
        }

        public string Name
        {
            get { return name; }
        }

        public uint Victories
        {
            get { return victories; }
        }

        public void AddVictory()
        {
            victories++;
        }
    }
}