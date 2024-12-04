
using UnityEngine;

public static class Glossary
{
    public enum Tag
    {
        Reboundable, LeftHand
    }

    public enum CustomColor
    {
        Green, Yellow, Orange, Red
    }

    public static string GetTag(Tag tag)
    {
        return tag switch
        {
            Tag.Reboundable => "Reboundable",
            Tag.LeftHand => "LeftHand",
            _ => ""
        };
    }

    public static Color GetColor(CustomColor color)
    {
        return color switch
        {
            CustomColor.Green => new Color(0, 1, 0),
            CustomColor.Yellow => new Color(.75f, 1, 0),
            CustomColor.Orange => new Color(1, .75f, 0),
            CustomColor.Red => new Color(1, 0, 0),
            _ => new Color(0, 0, 0)
        };
    }
}