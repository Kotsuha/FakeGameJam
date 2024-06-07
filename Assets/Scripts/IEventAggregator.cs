public interface IEventAggregator
{
    string ID { get; }
    EventType Type { get; }
}
