using System.Numerics;

namespace QuoridorLib.Models
{
    public class Wall
    {
        private Position FirstPosition;
        private Position SecondPosition;

        public Wall(int firstX, int firstY, int secondX, int secondY)  
        {
            FirstPosition = new Position(firstX, firstY);
            SecondPosition = new Position(secondX, secondY);
        }

        public Wall(Position firstposition, Position secondPosition) 
        {
            FirstPosition = new Position(firstposition);
            SecondPosition = new Position(secondPosition);
        }

        public Position GetSecondPosition()
        {
            return SecondPosition.GetPosition();
        }

        public void SetSecondPosition(int x, int y)
        {
            SecondPosition.SetPosition(x, y);
        }
        public void SetSecondPosition(Position position)
        {
            SecondPosition.SetPosition(position.GetPositionX(),position.GetPositionY());
        }
    }
}

        


