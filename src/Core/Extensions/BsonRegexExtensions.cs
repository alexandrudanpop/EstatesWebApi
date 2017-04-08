namespace Core.Extensions
{
    using MongoDB.Bson;

    public static class BsonRegexExtensions
    {
        public static BsonRegularExpression ToCaseInsensitiveRegex(this string source)
        {
            return new BsonRegularExpression("/^" + source.Replace("+", @"\+") + "$/i");
        }
    }
}