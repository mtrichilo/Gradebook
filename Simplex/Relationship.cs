namespace Simplex
{
    public enum Relationship
    {
        Equal = 0,
        LessThanOrEqual = 1,
        GreaterThanOrEqual = -1
    }

    public static class RelationshipExtension
    {
        public static int GetValue(this Relationship relationship)
        {
            return (int)relationship;
        }
    }
}
