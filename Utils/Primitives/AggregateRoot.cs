using CSharpFunctionalExtensions;

namespace Primitives
{
    public abstract class Aggregate : Entity<Guid>, AggregateRoot
    {
        private readonly List<DomainEvent> _domainEvents = new();

        protected Aggregate(Guid id) : base(id)
        {
        }

        protected Aggregate()
        {
        }

        public IReadOnlyCollection<DomainEvent> GetDomainEvents() => _domainEvents.ToList();

        public void ClearDomainEvents() => _domainEvents.Clear();

        protected void RaiseDomainEvent(DomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }
    }

    public interface AggregateRoot
    {
    }
}