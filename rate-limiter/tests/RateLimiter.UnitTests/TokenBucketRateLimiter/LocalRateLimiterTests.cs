using FluentAssertions;
using RateLimiter.Core;
using RateLimiter.Core.Contracts;
using Xunit;

namespace RateLimiter.UnitTests.TokenBucketRateLimiter;

public class LocalRateLimiterTests
{
	[Theory]
	[InlineData(1)]
	[InlineData(10)]
	[InlineData(100)]
	public async Task When_OneClientExceedsRateLimit_Expect_False(int tokensCount)
	{
		// assert
		var limiter = new LocalRateLimiter(tokensCount);
		var clientIdentifier = new ClientIdentifier(Guid.NewGuid());
		var successfulIncreaseResults = new bool[tokensCount];
		// act
		for (var i = 0; i < tokensCount; i++)
		{
			successfulIncreaseResults[i] = await limiter.TryIncreaseRate(clientIdentifier);
		}

		var failedIncreaseResult = await limiter.TryIncreaseRate(clientIdentifier);

		// assert
		successfulIncreaseResults.All(x => x).Should().BeTrue();
		failedIncreaseResult.Should().BeFalse();
	}
}