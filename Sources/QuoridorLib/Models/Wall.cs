using System.Numerics;

namespace QuoridorLib.Models
{
    class Wall
    {
        int x1;
        int y1;
        int x2;
        int y2;
        List<Wall> PosW = new List<Wall>(x1,y1,x2,y2);
        public Wall(int a, int b,int c,int d)
        {
            x1 = a;
            y1 = b;
            x2 = c;
            y2 = d;
        }

        //public void Placing(int x1, int y1, int x2, int y2)
        //{ 
        //    if (x1 == x2 && Math.Abs(y2 - y1) == 2)
        //    {
        //        List<Wall> list = new List<Wall>(x1,y1,x2,y2);
        //    }
        //}
public IsWallPlaceable(){
        
        }
    }
}
