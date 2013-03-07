using System;

namespace Pug.Scorpion
{
	public interface IFulfillmentProcessInfo
	{
		DateTime ExpectedCompletionTimestamp { get; }
		string Identifier { get; }
		string Order { get; }
		DateTime RegistrationTimestamp { get; }
		string RegistrationUser { get; }
		DateTime StartTimestamp { get; }
		string Status { get; }
	}
}
