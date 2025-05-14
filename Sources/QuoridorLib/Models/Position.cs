namespace QuoridorLib.Models
{
    public class Position 
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Position(int x, int y) 
        {
            X = x;
            Y = y;
        }

        public Position(Position position)
        {
            X = position.X;
            Y = position.Y;
        }

        public void SetPosition(int x, int y) 
        {
            X = x;
            Y = y;
        }

        public Position GetPosition() => this;

        public int GetPositionX() => X;

        public int GetPositionY() => Y;

        public override bool Equals(object? obj)
        {
            if (obj is Position other)
            {
                return X == other.X && Y == other.Y;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
    }
}