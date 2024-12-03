
public static class Glossary
{
    public enum Tag
    {
        Reboundable, LeftHand
    }

    public static string GetTag(Tag tag)
    {
        switch(tag)
        {
            case Tag.Reboundable: return "Reboundable";
            case Tag.LeftHand: return "LeftHand";
        }

        return "";
    }
}