namespace eShop.Domain.SharedKernel.Abstractions;

using MediatR;

public interface IDomainEvent : INotification
{
    DateTime OccurredOn { get; }
}
