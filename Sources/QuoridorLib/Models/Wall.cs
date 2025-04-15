using System.Numerics;

namespace QuoridorLib.Models
{
    public class Wall : Position
    {
        private Position endPosition;

        public Wall(int startX, int startY, int endX, int endY) : base(startX, startY)
        {
            endPosition = new Position(endX, endY);
        }

        public bool IsPlaceable()
        {
            bool isHorizontal = (GetPositionY() == endPosition.GetPositionY() && 
                               Math.Abs(endPosition.GetPositionX() - GetPositionX()) == 1);
            bool isVertical = (GetPositionX() == endPosition.GetPositionX() && 
                             Math.Abs(endPosition.GetPositionY() - GetPositionY()) == 1);

            if (!isHorizontal && !isVertical)
                return false;

            if (GetPositionX() < 0 || GetPositionX() > 8 || GetPositionY() < 0 || GetPositionY() > 8 ||
                endPosition.GetPositionX() < 0 || endPosition.GetPositionX() > 8 || 
                endPosition.GetPositionY() < 0 || endPosition.GetPositionY() > 8)
                return false;

            if (GetPositionX() == endPosition.GetPositionX() && GetPositionY() == endPosition.GetPositionY())
                return false;

            return true;
        }

        public Position GetEndPosition()
        {
            return endPosition;
        }

        public void SetEndPosition(int x, int y)
        {
            endPosition.SetPosition(x, y);
        }
    }
}

        


