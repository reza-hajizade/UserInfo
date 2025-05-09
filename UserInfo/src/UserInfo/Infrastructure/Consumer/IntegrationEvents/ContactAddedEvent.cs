namespace UserInfo.Infrastructure.Consumer.IntegrationEvents;

public record ContactAddedEvent(string phoneNumber, int UserId, DateTime OccurredOn);


