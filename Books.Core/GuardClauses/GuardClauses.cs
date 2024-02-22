using System.Collections;

namespace Books.Core.GuardClauses
{
    public static class GuardClauses
    {
        public static bool IsNull<T>(T value) => value is null;

        public static bool IsNotNull<T>(T value) => value is not null;

        public static (bool IsValid, string ErrorMessage) StringLengthIsValid(int minLength, int maxLength, string errorMessage, string value) =>
            value.Length >= minLength && value.Length <= maxLength ? (true, string.Empty) : (false, errorMessage);

        public static (bool IsValid, string ErrorMessage) IdIsGreaterThanZero(int id, string errorMessage) =>
            id > 0 ? (true, string.Empty) : (false, errorMessage);

        public static (bool IsValid, string ErrorMessage) IdIsZero(int id, string errorMessage) =>
            id == 0 ? (true, string.Empty) : (false, errorMessage);

        public static (bool IsValid, string ErrorMessage) IdIsNullOrGreaterThanZero(int? id, string errorMessage) =>
            !id.HasValue || id.Value > 0 ? (true, string.Empty) : (false, errorMessage);

        public static (bool IsValid, string ErrorMessage) CollectionHasItems(IEnumerable<object> collection, string errorMessage) =>
            collection.Any() ? (true, string.Empty) : (false, errorMessage);

        public static (bool IsValid, string ErrorMessage) IsType<T>(object value, string errorMessage) =>
            value.GetType() == typeof(T) ? (true, string.Empty) : (false, errorMessage);
    }
}
