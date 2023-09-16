namespace RateLimiter.Core.Contracts;

public interface IRateLimiter
{
	Task<bool> TryIncreaseRate(ClientIdentifier clientIdentifier);
	Task TryDecreaseRate(ClientIdentifier clientIdentifier);
}