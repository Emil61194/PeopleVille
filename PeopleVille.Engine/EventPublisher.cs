namespace PeopleVille.Engine;

public class EventPublisher(IEventPublisher eventPublisher)
{
    // CALL THIS FOR ALL DATA U WANT TO SEND TO CLIENT
    public Task PublishEvent(object eventData)
    {
        return eventPublisher.PublishEventAsync(eventData);
    }
}