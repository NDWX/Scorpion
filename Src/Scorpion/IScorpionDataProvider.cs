using System;
using System.Collections.Generic;
using Pug.Application.Security;
using Pug.Bizcotty;
using Pug.Bizcotty.Geography;

using Pug.Application.Data;

namespace Pug.Scorpion
{
	public interface IScorpionDataProvider : IApplicationDataSession
	{
		bool OrderExists(string identifier);

		void InsertOrder(string identifier, string cart,
							string buyerName, Address buyerAddress, PersonName buyerContactPerson, 
							string payerName, Address billingAddress, PersonName billingContactPerson,
							decimal orderPriceTotal, decimal shippingCost, string buyerNote,
							string shippingName, Address shippingAddress, PersonName shippingContactPerson, string user);

		IEnumerable<Cartage.ICartInfo> GetOrders(string status, Range<DateTime> creationPeriod, string creationUser, Range<DateTime> lastModificationPeriod, string lastModificationUser);

		Order._Info GetOrder(string identifier);

		void InsertContactMethod(string order, string purpose, string name, string type, string destination, string user);

		void SetOrderStatus(string identifier, string status, string comment, string user);

		void SetOrderAttribute(string order, string name, string value, string user);

		EntityAttribute GetOrderAttribute(string order, string name);

		IEnumerable<EntityAttribute> GetOrderAttributes(string order);


		IEnumerable<ContactMethod> GetContactMethods(string purpose, string type);

		void UpdateContactMethod(string order, string purpose, string name, string type, string identifier, string user);

		void DeleteContactMethod(string order, string purpose, string name, string user);


		bool PaymentExists(string identifier);

		void InsertPayment(string identifier, string order, DateTime timestamp, string method, string transcactionIdentifier,
							string transactionType, string status, string statusShortMessage, string statusLongMessage, string paymentType, 
							string currency, decimal amount, decimal fee, decimal finalAmount, decimal taxAmount, decimal exchangeRate, string receiptIdentifier, string user);

		IEnumerable<EntityAttribute> GetPaymentAttributes(string identifier);

		void SetPaymentAttribute(string identifier, string name, string value, string user);

		EntityAttribute GetPaymentAttribute(string identifier, string name);

		IEnumerable<Payment._Info> GetPayments(string order, Range<DateTime> period, string method, string paymentType, string status, string currency, Range<DateTime> registrationPeriod);

		Payment._Info GetPayment(string identifier);

		Payment._Info GetPayment(string identifier, string order);

		void SetPaymentStatus(string identifier, string status, string comment, string user);


		void InsertFulfillmentProcess(string identifier, string asignee, string comment, DateTime timestamp, string status, DateTime expectedStatusCompletionTimestamp, DateTime expectedCompletionTimestamp, string user);

		void SetFulfillmentProcessAttribute(string identifier, string name, string value, string user);
		
		OrderFulfillmentProcess._Info[] GetFulfillmentProcesses(Range<DateTime> lastFulfillmentProcessRegistrationPeriod, Range<DateTime> lastFulfillmentProgressPeriod, string currentFulfillmentProgresssStatus, string currentFulfillmentProgressAssignee, Range<DateTime> expectedFulfillmentProcessCompletionTimestamp);

		OrderFulfillmentProcess._Info GetFulfillmentProcess(string identifier);

		OrderFulfillmentProcess._Info GetFulfillmentProcess(string identifier, string order);

		IEnumerable<EntityAttribute> GetFulfillmentProcessAttributes(string process);

		EntityAttribute GetFulfillmentProcessAttribute(string process, string name);

		void InsertFulfillmentProgress(string process, string identifier, DateTime timestamp, string status, string assignee, string comment, DateTime completionTimestamp, string registrationUser);

		void InsertFulfillmentProgressAttribute(string process, string identifier, string name, string value, string user);

		void SetFulfillmentStatus(string identifier, string status, string comment, string user);

		IEnumerable<OrderFulfillmentProcess._Progress._Info> GetFulfillmentProgresses(string process);

		OrderFulfillmentProcess._Progress._Info GetFulfillmentProgress(string process, string identifier);

		IEnumerable<EntityAttribute> GetFulfillmentProgressAttributes(string process, string identifier);
	}
}
