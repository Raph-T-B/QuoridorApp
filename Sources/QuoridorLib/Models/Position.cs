namespace QuoridorLib.Models
{
    public class Position 
    {
        public int X { get; }
        public int Y { get; }

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

        public static bool operator ==(Position leftP, Position rightP)
        {
            if (ReferenceEquals(leftP, rightP)) return true;
            if (leftP is null || rightP is null) return false;
            return leftP.X == rightP.X && leftP.Y == rightP.Y;
        }

        public static bool operator !=(Position leftP, Position rightP)
        {
            return !(leftP == rightP);
        }

        public override bool Equals(object? obj)
        {
            return obj is Position other && X == other.X && Y == other.Y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
    }
}