using FluentAssertions;
using RateLimiter.Core.Algorithms;
using Xunit;
using Xunit.Abstractions;

namespace RateLimiter.UnitTests.TokenBucketRateLimiter;

public class TokenBucketClientLimiterTests
{
	private readonly ITestOutputHelper _testOutputHelper;

	public TokenBucketClientLimiterTests(ITestOutputHelper testOutputHelper)
	{
		_testOutputHelper = testOutputHelper;
	}

	[Theory]
	[InlineData(5)]
	[InlineData(10)]
	[InlineData(100)]
	[InlineData(1)]
	public void When_IncreaseRateMoreThanTokensCount_Expect_False(int tokensCount)
	{
		// arrange
		var bucketClient = new TokenBucketClientLimiter(tokensCount);
		var increaseResults = new bool[tokensCount];

		// act
		for (var i = 0; i < tokensCount; i++)
		{
			increaseResults[i] = bucketClient.TryIncreaseRate();
		}

		var failedIncrease = bucketClient.TryIncreaseRate();

		// assert
		increaseResults.All(x => x).Should().BeTrue();
		failedIncrease.Should().BeFalse();
	}

	[Theory]
	[InlineData(5)]
	[InlineData(10)]
	[InlineData(100)]
	[InlineData(1)]
	public void When_DecreaseRateThenIncreaseMoreThanTokensCount_Expect_False(int tokensCount)
	{
		//arrange 
		var bucketClient = new TokenBucketClientLimiter(tokensCount);
		var increaseResults = new bool[tokensCount];

		//act
		for (var i = 0; i < tokensCount; i++)
		{
			bucketClient.DecreaseRate();
		}

		// act
		for (var i = 0; i < tokensCount; i++)
		{
			increaseResults[i] = bucketClient.TryIncreaseRate();
		}

		var failedIncrease = bucketClient.TryIncreaseRate();

		// assert
		increaseResults.All(x => x).Should().BeTrue();
		failedIncrease.Should().BeFalse();
	}

	[Theory]
	[InlineData(1)]
	[InlineData(5)]
	[InlineData(10)]
	[InlineData(100)]
	[InlineData(1000)]
	public void When_ManyThreadsAreIncreasingRate_Expect_SuccessfulIncreasesAreEqualToTokensCount(int tokensCount)
	{
		//arrange 
		var bucketClient = new TokenBucketClientLimiter(tokensCount);
		var successfulIncreases = 0;

		var threads = Enumerable.Range(1, 10).Select(x => new Thread(() =>
		{
			for (var i = 0; i < tokensCount; i++)
			{
				if (!bucketClient.TryIncreaseRate())
				{
					continue;
				}

				Interlocked.Increment(ref successfulIncreases);
				_testOutputHelper.WriteLine($"thread {x} successfully increased rate");
			}
		})).ToList();

		// act
		foreach (var thread in threads)
		{
			thread.Start();
		}

		// assert
		successfulIncreases.Should().Be(tokensCount);
	}
}