using System.Collections.Concurrent;
using RateLimiter.Core.Algorithms;
using RateLimiter.Core.Contracts;

namespace RateLimiter.Core;

public class LocalRateLimiter : IRateLimiter
{
	private readonly ConcurrentDictionary<ClientIdentifier, IClientRateLimiter> _buckets;
	private readonly int _maxTokensPerBucket;

	public LocalRateLimiter(int maxTokensPerBucket)
	{
		_maxTokensPerBucket = maxTokensPerBucket;
		_buckets = new ConcurrentDictionary<ClientIdentifier, IClientRateLimiter>();
	}

	public Task<bool> TryIncreaseRate(ClientIdentifier clientIdentifier)
	{
		var bucketClient = _buckets.GetOrAdd(clientIdentifier, new TokenBucketClientLimiter(_maxTokensPerBucket));
		return Task.FromResult(bucketClient.TryIncreaseRate());
	}

	public Task TryDecreaseRate(ClientIdentifier clientIdentifier)
	{
		if (_buckets.TryGetValue(clientIdentifier, out var bucketClient))
		{
			bucketClient.DecreaseRate();
		}

		return Task.CompletedTask;
	}
}