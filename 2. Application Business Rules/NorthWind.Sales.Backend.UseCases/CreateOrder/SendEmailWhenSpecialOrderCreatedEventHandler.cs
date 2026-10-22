namespace NorthWind.Sales.Backend.UseCases.CreateOrder;

using NorthWind.Entities.Interfaces;
using NorthWind.Events.Entities.Interfaces;
using NorthWind.Sales.Backend.UseCases.Resources;

internal class SendEmailWhenSpecialOrderCreatedEventHandler(
    IMailService mailService)
    : IDomainEventHandler<SpecialOrderCreatedEvent>
{
    public Task Handle(SpecialOrderCreatedEvent createdOrder)
    {
        return mailService.SendMailToAdministrator(
            CreateOrderMessages.SendEmailSubject,
            string.Format(CreateOrderMessages.SendEmailBodyTemplate,
                createdOrder.OrderId,
                createdOrder.ProductsCount));
    }
}