namespace NorthWind.Sales.Backend.BusinessObjects.Specifications;

using System.Linq.Expressions;
using NorthWind.Entities.Abstractions;

public class SpecialOrderSpecification : Specification<OrderAggregate>
{
    public override Expression<Func<OrderAggregate, bool>> ConditionExpression =>
        order => order.OrderDetails.Count > 3;
}