using System;
using System.Collections.Generic;

using System.Transactions;

using Pug.Application.Data;
using Pug.Application.Security;

using Pug.Bizcotty;
using Pug.Bizcotty.Geography;

namespace Pug.Scorpion
{
	public class SynchronizationContext
	{
		object orderIdentifierSync = new object();
		object paymentIdentifierSync = new object();
		object fulfillmentProcessIdentifierSync = new object();

		public object OrderIdentifierSync
		{
			get
			{
				return orderIdentifierSync;
			}
		}
		public object PaymentIdentifierSync
		{
			get
			{
				return paymentIdentifierSync;
			}
		}
		public object FulfillmentProcessIdentifierSync
		{
			get
			{
				return fulfillmentProcessIdentifierSync;
			}
		}
	}

	public class Scorpion<Pi, Pp> : Sisca.ISisca<Pi, Pp>
		where Pi : Sisca.IProductInfo
		where Pp : Sisca.IProductInfoProvider<Pi>
	{
		IApplicationData<ICartInfoStoreProvider> cartStoreProviderFactory;
		Cartage.ICartage cartage;
		Pp productInfoProvider;
		Sisca.ISisca<Pi, Pp> sisca;

		IScorpionDataProviderFactory dataProviderFactory;
		
		SynchronizationContext synchronizationContext = new SynchronizationContext();

		public Scorpion(IApplicationData<ICartInfoStoreProvider> cartStoreProviderFactory, Pp productInfoProvider, IScorpionDataProviderFactory dataProviderFactory, ISecurityManager securityManager)
		{
			this.cartStoreProviderFactory = cartStoreProviderFactory;
			this.dataProviderFactory = dataProviderFactory;
			this.productInfoProvider = productInfoProvider;

			cartage = new Cartage.Cartage<ICartInfoStoreProvider>(cartStoreProviderFactory, securityManager);

			sisca = new Sisca.Sisca<ICartInfoStoreProvider, Pi, Pp>(cartage, productInfoProvider);
		}

		string GetNewIdentifier(object sync)
		{
			byte[] binarySeed;

			lock (sync)
			{
				binarySeed = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
			}

			string newIdentifier = Pug.Base32.From(binarySeed).Replace("=", "");

			return newIdentifier;
		}

		string GetNewOrderIdentifier()
		{
			return GetNewIdentifier(synchronizationContext.OrderIdentifierSync);
		}

		#region ICartage<IShoppingCart<Pi,Pp>,ICartInfo,ICartLine<Pi>,ICartLineInfo<Pi>,ICartLineAttributeInfo,ICartSummary> Members

		public bool CartExists(string identifier)
		{
			throw new System.NotImplementedException();
		}

		public void DeleteCart(string identifier)
		{
			throw new System.NotImplementedException();
		}

		public Sisca.IShoppingCart<Pi, Pp> GetCart(string identifier)
		{
			throw new System.NotImplementedException();
		}

		public System.Collections.Generic.ICollection<Cartage.ICartInfo> GetCarts(Range<System.DateTime> creationPeriod, Range<System.DateTime> modificationPeriod)
		{
			throw new System.NotImplementedException();
		}

		public Sisca.IShoppingCart<Pi, Pp> RegisterCart(string identifier)
		{
			throw new System.NotImplementedException();
		}

		public Sisca.IShoppingCart<Pi, Pp> RegisterCart()
		{
			throw new System.NotImplementedException();
		}

		#endregion
	
		public void CreateOrder (
			ref string identifier, string cart, string buyerName,
			Address buyerAddress, PersonName buyerContactPerson,
			string payerName, Address billingAddress, PersonName billingContactPerson,
			decimal orderPriceTotal, decimal shippingCost, string buyerNote,
			string shippingName, Address shippingAddress, PersonName shippingContactPerson, 
			ICollection<ContactMethod> contactMethods,
			IDictionary<string, string> attributes)
		{
			try
			{
				using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required))
				{
					//RegisterOrder(identifier, cart, buyerName, buyerAddress, buyerContactPerson, payerName, billingAddress, billingContactPerson, orderPriceTotal, shippingCost, buyerNote, shippingName, shippingAddress, shippingContactPerson, contactMethods, attributes);
					
					IScorpionDataProvider dataStore = dataProviderFactory.GetSession();

					if (string.IsNullOrEmpty(identifier))
						identifier = GetNewOrderIdentifier();
					else
						if (dataStore.OrderExists(identifier))
							throw new OrderExists();

					ICartInfoStoreProvider cartInfoStore = cartStoreProviderFactory.GetSession();

					if (cartInfoStore.CartIsFinalized(cart))
					{
						throw new CartFinalized();
					}

					cartInfoStore.FinalizeCart(cart);

					dataStore.InsertOrder(identifier, cart, buyerName, buyerAddress, buyerContactPerson, payerName, billingAddress, billingContactPerson, orderPriceTotal, shippingCost, buyerNote, shippingName, shippingAddress, shippingContactPerson);

					foreach (ContactMethod contactMethod in contactMethods)
						dataStore.InsertContactMethod(identifier, contactMethod);

					foreach (KeyValuePair<string, string> attribute in attributes)
						dataStore.SetOrderAttribute(identifier, attribute.Key, attribute.Value);

					transactionScope.Complete();
				}
			}
			catch
			{
				throw;
			}
		}

		IEnumerable<Cartage.ICartInfo> GetOrders(IEnumerable<string> status, Range<DateTime> creationPeriod, IEnumerable<string> creationUser, Range<DateTime> lastModificationPeriod, IEnumerable<string> lastModificationUser)
		{
			return null;
		}

        public Order GetOrder(string identifier)
        {
			IScorpionDataProvider dataSession = null;
			Order._Info orderInfo;
			Order order;

			try
			{
				dataSession = dataProviderFactory.GetSession();
				orderInfo = dataSession.GetOrder(identifier);

				if( orderInfo == null)
					throw new OrderNotFound();

				order = new Order(orderInfo, dataProviderFactory);

				dataSession.Dispose();
				dataSession = null;
			}
			catch
			{
				throw;
			}
			finally
			{
				if( dataSession != null)
					dataSession.Dispose();
			}

            return order;
        }

		void RegisterPayment(ref string identifier, string order, DateTime timestamp, string method, string transactionIdentifier, string transactionType, string status, string statusShortMessage, string statusLongMessage, string paymentType, string currency, decimal amount, decimal fee, decimal finalAmount, decimal taxAmount, decimal exchangeRate, string receiptIdentifier, IDictionary<string, string> attributes)
		{
			Order _order = GetOrder(order);

			_order.RegisterPayment(ref identifier, timestamp, method, transactionIdentifier, transactionType, status, statusShortMessage, statusLongMessage, paymentType, currency, amount, fee, finalAmount, taxAmount, exchangeRate, receiptIdentifier, attributes);
		}

		public IEnumerable<Payment._Info> GetPayments(string order, Range<DateTime> period, IEnumerable<string> methods, IEnumerable<string> paymentTypes, IEnumerable<string> statuses, IEnumerable<string> currencies, IEnumerable<string> exchangeRates, Range<DateTime> registrationPeriod)
		{
			throw new NotImplementedException();
		}

		public Payment GetPayment(string identifier)
		{
			IScorpionDataProvider dataSession = null;
			Payment._Info paymentInfo;
			Payment payment;

			try
			{
				dataSession = dataProviderFactory.GetSession();
				paymentInfo = dataSession.GetPayment(identifier);
				payment = new Payment(paymentInfo, dataProviderFactory);

				dataSession.Dispose();
				dataSession = null;
			}
			catch
			{
				throw;
			}
			finally
			{
				if (dataSession != null)
					dataSession.Dispose();
			}

			return payment;
		}

		public OrderFulfillmentProcess._Info[] GetFulfillmentProcesses(string order = "", Range<DateTime> lastFulfillmentProcessRegistrationPeriod = null, Range<DateTime> lastFulfillmentProgressPeriod = null, IEnumerable<string> currentFulfillmentProgresssStatus = null, IEnumerable<string> currentFulfillmentProgressAssignee = null, Range<DateTime> expectedFulfillmentProcessCompletionTimestamp = null)
		{
			throw new NotImplementedException();
		}

		public OrderFulfillmentProcess GetFulfillmentProcess(string identifier)
		{
			IScorpionDataProvider dataSession = null;
			OrderFulfillmentProcess._Info fulfillmentProcessInfo;
			OrderFulfillmentProcess fulfillmentProcess;

			try
			{
				dataSession = dataProviderFactory.GetSession();
				fulfillmentProcessInfo = dataSession.GetOrderFulfillmentProcess(identifier);
				fulfillmentProcess = new OrderFulfillmentProcess(fulfillmentProcessInfo, dataProviderFactory);

				dataSession.Dispose();
				dataSession = null;
			}
			catch
			{
				throw;
			}
			finally
			{
				if (dataSession != null)
					dataSession.Dispose();
			}

			return fulfillmentProcess;
		}

		#region IDisposable Members

		public void Dispose()
		{
			throw new System.NotImplementedException();
		}

		#endregion
	}
}
