namespace NorthWind.Entities.Abstractions;

using System.Linq.Expressions;

public abstract class Specification<T>
{
    public abstract Expression<Func<T, bool>> ConditionExpression { get; }

    public bool IsSatisfiedBy(T entityInstance)
    {
        Func<T, bool> ExpressionDelegate = ConditionExpression.Compile();
        return ExpressionDelegate(entityInstance);
    }
}