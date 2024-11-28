
public static class Glossary
{
    public enum Tag
    {
        Reboundable
    }

    public static string GetTag(Tag tag)
    {
        switch(tag)
        {
            case Tag.Reboundable: return "Reboundable";
        }

        return "";
    }
}