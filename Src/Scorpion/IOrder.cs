using System;
using System.Collections;
using System.Collections.Generic;

using Pug.Bizcotty;
using Pug.Bizcotty.Geography;

namespace Pug.Scorpion
{
	public interface IOrder
	{
		IOrderInfo Info { get; }

		string this[string attribute] { get; set; }

		Order.Summary GetSummary();

		System.Collections.Generic.IEnumerable<EntityAttribute> GetAttributes();

		void AddContactMethod(string purpose, string name, string type, string destination);

		System.Collections.Generic.IEnumerable<ContactMethod> GetContactMethods(string purpose, string type);

		void RegisterPayment(ref string identifier, DateTime timestamp, string method, string transactionIdentifier, string transactionType, string status, string statusShortMessage, string statusLongMessage, string paymentType, string currency, decimal amount, decimal fee, decimal finalAmount, decimal taxAmount, decimal exchangeRate, string receiptIdentifier, System.Collections.Generic.IDictionary<string, string> attributes);

		System.Collections.Generic.IEnumerable<IPaymentInfo> GetPayments(Pug.Range<DateTime> period, string method, string paymentType, string status, string currency, Pug.Range<DateTime> registrationPeriod);

		IPayment GetPayment(string identifier);

		void VoidPayment(string identifier, string comment);

		void UpdateContactMethod(string purpose, string name, string type, string destination);

		void DeleteContactMethod(string purpose, string name);

		void RegisterFulfillmentProcess(ref string identifier, string asignee, System.Collections.Generic.IDictionary<string, string> attributes, string comment, DateTime timestamp, string status, DateTime expectedStatusCompletionTimestamp, DateTime expectedCompletionTimestamp);

		IEnumerable<IFulfillmentProcessInfo> GetFulfillmentProcesses(Pug.Range<DateTime> lastFulfillmentProcessRegistrationPeriod, Pug.Range<DateTime> lastFulfillmentProgressPeriod, string currentFulfillmentProgresssStatus, string currentFulfillmentProgressAssignee, Pug.Range<DateTime> expectedFulfillmentProcessCompletionTimestamp);

		IFulfillmentProcess GetFulfillmentProcess(string identifier);

		void CancelFulfillmentProcess(string identifier, System.Collections.Generic.IDictionary<string, string> attributes, string comment, DateTime timestamp);

		void FinalizeFulfillmentProcess(string identifier, System.Collections.Generic.IDictionary<string, string> attributes, string comment, DateTime timestamp);

		void Cancel(string comment);

		void Refresh();
	}
}
