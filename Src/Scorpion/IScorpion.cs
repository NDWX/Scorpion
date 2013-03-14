using System;
using System.Collections;
using System.Collections.Generic;

using Pug.Bizcotty;
using Pug.Bizcotty.Geography;

using Pug.Cartage;

using Pug.Sisca;

namespace Pug.Scorpion
{
	public interface IScorpion<Pi, Pp> : IDisposable //Sisca.ISisca<Pi, Pp>, 
		where Pi : IProductInfo
		where Pp : IProductInfoProvider<Pi>
	{
	
		void CreateOrder(ref string identifier, string cart, string buyerName, Address buyerAddress, PersonName buyerContactPerson, string payerName, Address billingAddress, PersonName billingContactPerson, decimal orderPriceTotal, decimal shippingCost, string buyerNote, string shippingName, Address shippingAddress, PersonName shippingContactPerson, ICollection<ContactMethod> contactMethods, IDictionary<string, string> attributes);

		void CreateOrder(ref string identifier, string cart, string buyerName, Address buyerAddress, PersonName buyerContactPerson, string payerName, Address billingAddress, PersonName billingContactPerson, decimal orderPriceTotal, decimal shippingCost, string buyerNote, string shippingName, Address shippingAddress, PersonName shippingContactPerson, ICollection<ContactMethod> contactMethods, IDictionary<string, string> attributes, ref string paymentIdentifier, DateTime paymentTimestamp, string paymentMethod, string paymentTransactionIdentifier, string paymentTransactionType, string paymentStatus, string paymentStatusShortMessage, string paymentStatusLongMessage, string paymentPaymentType, string paymentCurrency, decimal paymentAmount, decimal paymentFee, decimal paymentFinalAmount, decimal paymentTaxAmount, decimal paymentExchangeRate, string paymentReceiptIdentifier, IDictionary<string, string> paymentAttributes);

		IEnumerable<ICartInfo> GetOrders(string status, Range<DateTime> creationPeriod, string creationUser, Range<DateTime> lastModificationPeriod, string lastModificationUser);

		IOrder GetOrder(string identifier);

		void RegisterPayment(ref string identifier, string order, DateTime timestamp, string method, string transactionIdentifier, string transactionType, string status, string statusShortMessage, string statusLongMessage, string paymentType, string currency, decimal amount, decimal fee, decimal finalAmount, decimal taxAmount, decimal exchangeRate, string receiptIdentifier, IDictionary<string, string> attributes);

		IEnumerable<IPaymentInfo> GetPayments(string order, Range<DateTime> period, string method, string paymentType, string status, string currency, string exchangeRate, Range<DateTime> registrationPeriod);

		IPayment GetPayment(string identifier);

		IEnumerable<IFulfillmentProcessInfo> GetFulfillmentProcesses(string order, Range<DateTime> lastFulfillmentProcessRegistrationPeriod, Range<DateTime> lastFulfillmentProgressPeriod, string currentFulfillmentProgresssStatus, string currentFulfillmentProgressAssignee, Range<DateTime> expectedFulfillmentProcessCompletionTimestamp);

		IFulfillmentProcess GetFulfillmentProcess(string identifier);
	}
}
