namespace RateLimiter.Core.Contracts;

public interface IRequestIdentifierResolver
{
	public ClientIdentifier CreateIdentifier<T>(Func<T, ClientIdentifier> resolver);
}