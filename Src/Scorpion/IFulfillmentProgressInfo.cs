using System;

namespace Pug.Scorpion
{
	public interface IFulfillmentProgressInfo
	{
		string Comment { get; }
		DateTime ExpectedCompletionTimestamp { get; }
		string Identifier { get; }
		DateTime RegistrationTimestamp { get; }
		string RegistrationUser { get; }
		string Status { get; }
		DateTime Timestamp { get; }
	}
}
