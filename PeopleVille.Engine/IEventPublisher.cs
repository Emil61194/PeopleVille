namespace PeopleVille.Engine;

public interface IEventPublisher
{
    Task PublishEventAsync(object eventData);
}