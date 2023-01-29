using System;

public struct GridPostition: IEquatable<GridPostition>
{
    public int x;
    public int z;

    public GridPostition(int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    public override bool Equals(object obj)
    {
        return obj is GridPostition gridPostition &&
               x == gridPostition.x &&
               z == gridPostition.z;
    }

    public bool Equals(GridPostition other)
    {
        return this == other;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(x,z);
    }

    public override string ToString()
    {
        return $"{x} , {z}";
    }

    public static bool operator == (GridPostition a, GridPostition b)
    {
        return a.x == b.x && a.z == b.z;
    }

    public static bool operator != (GridPostition a , GridPostition b)
    {
        return !(a==b);
    }

    public static GridPostition operator +(GridPostition a, GridPostition b)
    {
        return new GridPostition(a.x + b.x,a.z+ b.z);
    }

    public static GridPostition operator -(GridPostition a, GridPostition b)
    {
        return new GridPostition(a.x - b.x, a.z - b.z);
    }
}
