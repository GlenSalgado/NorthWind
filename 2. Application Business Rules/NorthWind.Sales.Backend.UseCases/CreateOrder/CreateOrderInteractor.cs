namespace NorthWind.Sales.Backend.UseCases.CreateOrder;

using NorthWind.DomainLogs.Entities.Interfaces;
using NorthWind.DomainLogs.Entities.ValueObjects;
using NorthWind.Entities.Guards;
using NorthWind.Entities.Interfaces;
using NorthWind.Transactions.Entities.Interfaces;

internal class CreateOrderInteractor(
    ICreateOrderOutputPort outputPort,
    ICommandsRepository repository,
    IModelValidatorHub<CreateOrderDto> modelValidatorHub,
    IDomainEventHub<SpecialOrderCreatedEvent> domainEventHandler,
    IDomainLogger domainLogger,
    IDomainTransaction domainTransaction,
    IUserService userService) : ICreateOrderInputPort
{
    public async Task Handle(CreateOrderDto orderDto)
    {
        // Verificar autenticación
        GuardUser.AgainstUnauthenticated(userService);

        await GuardModel.AgainstNotValid(modelValidatorHub, orderDto);

        await domainLogger.LogInformation(new DomainLog(
            CreateOrderMessages.StartingPurchaseOrderCreation,
            userService.UserName));

        OrderAggregate Order = OrderAggregate.From(orderDto);

        try
        {
            // Iniciar la transacción
            domainTransaction.BeginTransaction();

            await repository.CreateOrder(Order);
            await repository.SaveChanges();

            await domainLogger.LogInformation(new DomainLog(
                string.Format(CreateOrderMessages.PurchaseOrderCreatedTemplate,
                    Order.Id),
                userService.UserName));

            await outputPort.Handle(Order);

            if (new SpecialOrderSpecification().IsSatisfiedBy(Order))
            {
                await domainEventHandler.Raise(
                    new SpecialOrderCreatedEvent(
                        Order.Id, Order.OrderDetails.Count));
            }

            // Aceptar la transacción
            domainTransaction.CommitTransaction();
        }
        catch
        {
            // Cancelar la transacción
            domainTransaction.RollbackTransaction();

            string Information = string.Format(
                CreateOrderMessages.OrderCreationCancelledTemplate,
                Order.Id);

            await domainLogger.LogInformation(
                new DomainLog(Information, userService.UserName));

            throw;
        }
    }
}