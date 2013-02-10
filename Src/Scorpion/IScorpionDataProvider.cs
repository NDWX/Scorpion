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

		void InsertContactMethod(string order, ContactMethod contactMethod);

		void SetOrderAttribute(string order, string name, string value);

		Order._Info GetOrder(string identifier);


		bool PaymentExists(string identifier);

		void InsertPayment(string identifier, string order, DateTime timestamp, string method, string transcactionIdentifier,
		string transactionType, string status, string statusShortMessage, string statusLongMessage, string paymentType, string currency, decimal amount, decimal fee, decimal finalAmount, decimal taxAmount, decimal exchangeRate, string receiptIdentifier);

		void InsertPaymentAttribute(string identifier, string name, string value);

		Payment._Info GetPayment(string identifier);

		Payment._Info GetPayment(string identifier, string order);


		OrderFulfillmentProcess._Info GetOrderFulfillmentProcess(string identifier);

		OrderFulfillmentProcess._Info GetOrderFulfillmentProcess(string identifier, string order);
	}
}
