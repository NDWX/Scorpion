using System;
namespace Pug.Scorpion
{
	public interface IFulfillmentProcess
	{
		IFulfillmentProcessInfo Info { get; }

		string this[string name] { get; set; }

		void Cancel(System.Collections.Generic.IDictionary<string, string> attributes, string comment, DateTime timestamp);

		void Finalize(System.Collections.Generic.IDictionary<string, string> attributes, string comment, DateTime timestamp);

		System.Collections.Generic.IEnumerable<Pug.Scorpion.EntityAttribute> GetAttributes();

		IFulfillmentProgress GetProgress(string identifier);

		System.Collections.Generic.IEnumerable<IFulfillmentProgressInfo> GetProgresses();

		void Refresh();

		void RegisterProgress(ref string identifier, string assignee, System.Collections.Generic.IDictionary<string, string> attributes, string comment, DateTime timestamp, string status, DateTime expectedCompletionTimestamp);
	}
}
