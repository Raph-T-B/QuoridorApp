using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuoridorLib.Models
{
    public class WallCouple
    {
        readonly Wall Wall1;
        readonly Wall Wall2;
        readonly string Orientation;

        public WallCouple(Wall wall1, Wall wall2, string orientation)
        {
            Wall1 = wall1;
            Wall2 = wall2;
            Orientation = orientation;
        }

        public Wall GetWall1()
        {
             return Wall1;
             
        }
        public Wall GetWall2()
        {
            return Wall2;

        }
        public string GetOrientation()
        {
            return Orientation;

        }
    }
}

