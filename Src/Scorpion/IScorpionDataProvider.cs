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
							string shippingName, Address shippingAddress, PersonName shippingContactPerson);

		IEnumerable<Cartage.ICartInfo> GetOrders(string status, Range<DateTime> creationPeriod, string creationUser, Range<DateTime> lastModificationPeriod, string lastModificationUser);

		Order._Info GetOrder(string identifier);

		void InsertContactMethod(string order, string purpose, string name, string type, string destination);

		void SetOrderStatus(string identifier, string status, string comment);

		void SetOrderAttribute(string order, string name, string value);

		EntityAttribute GetOrderAttribute(string order, string name);

		IEnumerable<EntityAttribute> GetOrderAttributes(string order);


		IEnumerable<ContactMethod> GetContactMethods(string purpose, string type);

		void UpdateContactMethod(string order, string purpose, string name, string type, string identifier);

		void DeleteContactMethod(string order, string purpose, string name);


		bool PaymentExists(string identifier);

		void InsertPayment(string identifier, string order, DateTime timestamp, string method, string transcactionIdentifier,
							string transactionType, string status, string statusShortMessage, string statusLongMessage, string paymentType, 
							string currency, decimal amount, decimal fee, decimal finalAmount, decimal taxAmount, decimal exchangeRate, string receiptIdentifier);

		void InsertPaymentAttribute(string identifier, string name, string value);

		IEnumerable<Payment._Info> GetPayments(string order, Range<DateTime> period, string method, string paymentType, string status, string currency, Range<DateTime> registrationPeriod);

		Payment._Info GetPayment(string identifier);

		Payment._Info GetPayment(string identifier, string order);

		void SetPaymentStatus(string identifier, string status, string comment);


		void InsertFulfillmentProcess(string identifier, string asignee, string comment, DateTime timestamp, string status, DateTime expectedStatusCompletionTimestamp, DateTime expectedCompletionTimestamp);

		void InsertFulfillmentProcessAttribute(string identifier, string name, string value);
		
		OrderFulfillmentProcess._Info[] GetFulfillmentProcesses(Range<DateTime> lastFulfillmentProcessRegistrationPeriod, Range<DateTime> lastFulfillmentProgressPeriod, string currentFulfillmentProgresssStatus, string currentFulfillmentProgressAssignee, Range<DateTime> expectedFulfillmentProcessCompletionTimestamp);

		OrderFulfillmentProcess._Info GetOrderFulfillmentProcess(string identifier);

		OrderFulfillmentProcess._Info GetOrderFulfillmentProcess(string identifier, string order);

		IEnumerable<EntityAttribute> GetOrderFulfillmentAttributes(string process);

		void InsertOrderFulfillmentProgress(string process, string identifier, DateTime timestamp, string status, string assignee, string comment, DateTime completionTimestamp, string registrationUser);

		void InsertOrderFUlfillmentProgressAttribute(string process, string identifier, string name, string value, string user);

		void SetOrderFulfillmentStatus(string identifier, string status, string comment, string user);

		IEnumerable<OrderFulfillmentProcess._Progress._Info> GetOrderFulfillmentProgresses(string process);

		OrderFulfillmentProcess._Progress._Info GetOrderFulfillmentProgress(string process, string identifier);

		IEnumerable<EntityAttribute> GetOrderFulfillmentProgressAttributes(string process, string identifier);
	}
}
