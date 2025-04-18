using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuoridorLib.Models
{
    public class Player
    {
        readonly string Name;
        private int Victories;

        public Player(string name)
        {
            this.Name = name;
        }
        private int VictoriesCount()
        {
            Victories++;
            return Victories;
        }
        
        
    }
}
