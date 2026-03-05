namespace eShop.Domain.SharedKernel.Exceptions;

public abstract class DomainException(string message) : Exception(message) { }
