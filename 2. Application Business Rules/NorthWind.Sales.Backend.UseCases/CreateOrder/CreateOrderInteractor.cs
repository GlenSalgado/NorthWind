namespace NorthWind.Sales.Backend.UseCases.CreateOrder;

using NorthWind.DomainLogs.Entities.Interfaces;
using NorthWind.DomainLogs.Entities.ValueObjects;
using NorthWind.Entities.Guards;
using NorthWind.Entities.Interfaces;
using Microsoft.Extensions.Logging;

internal class CreateOrderInteractor(
    ICreateOrderOutputPort outputPort,
    ICommandsRepository repository,
    IModelValidatorHub<CreateOrderDto> modelValidatorHub,
    IDomainEventHub<SpecialOrderCreatedEvent> domainEventHandler,
    IDomainLogger domainLogger,
    IUserService userService,
    ILogger<CreateOrderInteractor> logger) : ICreateOrderInputPort
{
    public async Task Handle(CreateOrderDto orderDto)
    {
        // Verificar autenticación
        GuardUser.AgainstUnauthenticated(userService);

        await GuardModel.AgainstNotValid(modelValidatorHub, orderDto);

        await TryLogAsync(new DomainLog(
            CreateOrderMessages.StartingPurchaseOrderCreation,
            userService.UserName));

        OrderAggregate Order = OrderAggregate.From(orderDto);

        try
        {
            await repository.CreateOrder(Order);
            await repository.SaveChanges();
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Error while saving sales order {OrderId}", Order.Id);

            string Information = string.Format(
                CreateOrderMessages.OrderCreationCancelledTemplate,
                Order.Id);

            await TryLogAsync(
                new DomainLog(Information, userService.UserName));

            throw;
        }

        // EF Core saves the order, its details, and stock changes in one
        // database transaction. Logging and email use separate resources and
        // must not enlist in a Windows-only distributed transaction.
        await outputPort.Handle(Order);
        await TryLogAsync(new DomainLog(
            string.Format(CreateOrderMessages.PurchaseOrderCreatedTemplate,
                Order.Id),
            userService.UserName));

        if (new SpecialOrderSpecification().IsSatisfiedBy(Order))
        {
            try
            {
                await domainEventHandler.Raise(
                    new SpecialOrderCreatedEvent(
                        Order.Id, Order.OrderDetails.Count));
            }
            catch (Exception exception)
            {
                // The order is already committed; an email failure must not
                // report the completed order as failed and invite a duplicate.
                logger.LogError(exception,
                    "Order {OrderId} was saved, but its notification failed",
                    Order.Id);
            }
        }
    }

    private async Task TryLogAsync(DomainLog log)
    {
        try
        {
            await domainLogger.LogInformation(log);
        }
        catch (Exception exception)
        {
            logger.LogWarning(exception,
                "Could not write a domain log for sales order processing");
        }
    }
}
