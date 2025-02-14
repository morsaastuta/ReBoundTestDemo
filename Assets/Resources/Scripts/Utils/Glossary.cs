using UnityEngine;

public static class Glossary
{
    #region GAME MODES

    public enum GameMode
    {
        Hands, Controllers, Desktop
    }

    #endregion

    #region TAGS

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
    public static bool CompareTag(Collider collider, Tag tag)
    {
        if (collider.CompareTag(GetTag(tag))) return true;
        else return false;
    }

    #endregion

    #region LAYERS

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
    public static bool CompareLayer(Collider collider, Layer layer)
    {
        if (LayerMask.LayerToName(collider.gameObject.layer).Equals(GetLayer(layer))) return true;
        else return false;
    }

    #endregion

    #region COLORS

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

    #endregion
}