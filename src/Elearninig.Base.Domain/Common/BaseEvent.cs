using MediatR;

namespace Elearninig.Base.Domain.Common;
// learn more https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/domain-events-design-implementation
public abstract class BaseEvent : INotification
{
    public DateTimeOffset DateOccurred { get; set; }
    public BaseEvent()
    {
        DateOccurred = DateTimeOffset.UtcNow;
    }
}
