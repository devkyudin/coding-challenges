using RateLimiter.Core.Contracts;

namespace RateLimiter.Core.Algorithms;

public class TokenBucketClientLimiter : IClientRateLimiter
{
	private readonly object _lock;
	private readonly int _maxTokensPerBucket;
	private int _currentTokens;

	public TokenBucketClientLimiter(int maxTokensPerBucket)
	{
		_lock = new object();
		_maxTokensPerBucket = maxTokensPerBucket;
	}

	public bool TryIncreaseRate()
	{
		lock (_lock)
		{
			if (_currentTokens >= _maxTokensPerBucket)
			{
				return false;
			}

			Interlocked.Increment(ref _currentTokens);
			return true;
		}
	}

	public void DecreaseRate()
	{
		lock (_lock)
		{
			if (_currentTokens <= 0)
			{
				return;
			}

			Interlocked.Decrement(ref _currentTokens);
		}
	}
}