using BenchmarkDotNet.Running;
using RateLimiter.Benchmarks.Tests;

namespace RateLimiter.Benchmarks;

public static class Program
{
	// https://benchmarkdotnet.org/
	public static void Main(string[] args)
	{
		BenchmarkRunner.Run<RateLimiterTests>();
	}
}