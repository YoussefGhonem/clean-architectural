namespace Elearninig.Base.Domain.Common;
// learn more https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/implement-value-objects
public abstract class ValueObject
{
    // his method is responsible for returning the individual components (properties or fields) of the value object that participate in equality comparison.
    protected abstract IEnumerable<object> GetEqualityComponents();
    protected static bool EqualOperator(ValueObject left, ValueObject right)
    {
        // method is a protected static method that compares two ValueObject instances for equality.
        // It handles cases where either one of the objects is null or if both objects are non-null.
        // It calls the Equals method to perform the actual equality comparison.
        if (ReferenceEquals(left, null) ^ ReferenceEquals(right, null))
        {
            return false;
        }
        return ReferenceEquals(left, right) || left.Equals(right);
    }

    protected static bool NotEqualOperator(ValueObject left, ValueObject right)
    {
        // method is a protected static method that returns the negation of the EqualOperator
        return !(EqualOperator(left, right));
    }

    // helper methods
    public override bool Equals(object obj)
    {
        if (obj == null || obj.GetType() != GetType())
        {
            return false;
        }

        var other = (ValueObject)obj;

        return this.GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    // GetHashCode method defined in the base System.Object class.
    // The GetHashCode method is a standard method in C# that returns a hash code value for an object.
    // used for storing objects in hash-based collections like dictionaries and hash sets
    // The overridden 'GetHashCode' method calculates and returns a hash code based on the equality components returned by the GetEqualityComponents method.
    public override int GetHashCode()
    {
        // It uses GetEqualityComponents to retrieve the components,
        // and then applies a bitwise XOR operation to combine the hash codes of each component.
        return GetEqualityComponents()
            .Select(x => x != null ? x.GetHashCode() : 0)
            .Aggregate((x, y) => x ^ y);
    }
}
