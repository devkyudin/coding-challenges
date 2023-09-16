namespace RateLimiter.Core.Contracts;

public interface IClientRateLimiter
{
	bool TryIncreaseRate();
	void DecreaseRate();
}