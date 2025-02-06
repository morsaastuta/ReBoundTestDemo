using UnityEngine;

public static class Glossary
{
    public enum GameMode
    {
        Hands, Controllers, Desktop
    }

    public enum Tag
    {
        Reboundable, LeftHand, Annulling
    }

    public static string GetTag(Tag tag)
    {
        return tag switch
        {
            Tag.Reboundable => "Reboundable",
            Tag.LeftHand => "LeftHand",
            Tag.Annulling => "Annulling",
            _ => ""
        };
    }

    public enum Layer
    {
        Player, Ball, Scene, Interactable, Auxiliar
    }

    public static string GetLayer(Layer layer)
    {
        return layer switch
        {
            Layer.Player => "Player",
            Layer.Ball => "Ball",
            Layer.Scene => "Scene",
            Layer.Interactable => "Interactable",
            Layer.Auxiliar => "Auxiliar",
            _ => ""
        };
    }

    public enum CustomColor
    {
        Green, Yellow, Orange, Red
    }

    public static Color GetColor(CustomColor color)
    {
        return color switch
        {
            CustomColor.Green => Color.green,
            CustomColor.Yellow => Color.yellow,
            CustomColor.Orange => new Color(1, .55f, .1f),
            CustomColor.Red => Color.red,
            _ => new Color(0, 0, 0)
        };
    }
}