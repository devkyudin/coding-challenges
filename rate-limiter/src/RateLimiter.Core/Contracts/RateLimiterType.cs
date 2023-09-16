namespace RateLimiter.Core.Contracts;

public enum RateLimiterType
{
	TokenBucket,
	FixedWindowCounter,
	SlidingWindowLog,
	SlidingWindowCounter
}