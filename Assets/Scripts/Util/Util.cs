using System;

public class Util
{
    public static void RequireNonNull(object o, string name = null)
    {
        name ??= o.ToString();
        Assert(o != null, name + " == null");
    }

    public static void RequireEquals(object a, object b, string nameA = null, string nameB = null)
    {
        nameA ??= a.ToString();
        nameB ??= b.ToString();
        Assert(a.Equals(b), nameA + " != " + nameB);
    }

    public static void RequireDifferent(
        object a,
        object b,
        string nameA = null,
        string nameB = null
    )
    {
        nameA ??= a.ToString();
        nameB ??= b.ToString();
        Assert(!a.Equals(b), nameA + " == " + nameB);
    }

    public static void RequireBetweenZeroToOne(float value, string name = null)
    {
        name ??= value.ToString();
        Assert(value >= 0f, name + " < 0f");
        Assert(value <= 1f, name + " > 1f");
    }

    public static void Assert(bool condition, string message = "Assertion failed")
    {
        if (!condition)
        {
            throw new Exception(message);
        }
    }
}
