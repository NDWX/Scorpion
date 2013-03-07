using System;
using System.Collections.Generic;

using System.Transactions;

using Pug.Application.Data;
using Pug.Application.Security;

using Pug.Bizcotty;
using Pug.Bizcotty.Geography;

namespace Pug.Scorpion
{
	public class Scorpion<Pi, Pp> : IScorpion<Pi,Pp>
		where Pi : Sisca.IProductInfo
		where Pp : Sisca.IProductInfoProvider<Pi>
	{
		ISecurityManager securityManager;

		IApplicationData<ICartInfoStoreProvider> cartStoreProviderFactory;
		Cartage.ICartage cartage;
		Pp productInfoProvider;
		Sisca.ISisca<Pi, Pp> sisca;

		IScorpionDataProviderFactory dataProviderFactory;
		
		SynchronizationContext synchronizationContext = new SynchronizationContext();

		public Scorpion(IApplicationData<ICartInfoStoreProvider> cartStoreProviderFactory, Pp productInfoProvider, IScorpionDataProviderFactory dataProviderFactory, ISecurityManager securityManager)
		{
			this.securityManager = securityManager;
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

					dataStore.InsertOrder(identifier, cart, buyerName, buyerAddress, buyerContactPerson, payerName, billingAddress, billingContactPerson, orderPriceTotal, shippingCost, buyerNote, shippingName, shippingAddress, shippingContactPerson, securityManager.CurrentUser.Identity.Identifier);

					foreach (ContactMethod contactMethod in contactMethods)
						dataStore.InsertContactMethod(identifier, contactMethod.Purpose, contactMethod.Name, contactMethod.Type, contactMethod.Destination, securityManager.CurrentUser.Identity.Identifier);

					foreach (KeyValuePair<string, string> attribute in attributes)
						dataStore.SetOrderAttribute(identifier, attribute.Key, attribute.Value, securityManager.CurrentUser.Identity.Identifier);

					transactionScope.Complete();
				}
			}
			catch
			{
				throw;
			}
		}

		public IEnumerable<Cartage.ICartInfo> GetOrders(string status, Range<DateTime> creationPeriod, string creationUser, Range<DateTime> lastModificationPeriod, string lastModificationUser)
		{
			return null;
		}

        public IOrder GetOrder(string identifier)
        {
			IScorpionDataProvider dataSession = null;
			IOrderInfo orderInfo;
			IOrder order;

			try
			{
				dataSession = dataProviderFactory.GetSession();
				orderInfo = dataSession.GetOrder(identifier);

				if( orderInfo == null)
					throw new OrderNotFound();

				order = new Order(orderInfo, dataProviderFactory, synchronizationContext, securityManager);

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

		public void RegisterPayment(ref string identifier, string order, DateTime timestamp, string method, string transactionIdentifier, string transactionType, string status, string statusShortMessage, string statusLongMessage, string paymentType, string currency, decimal amount, decimal fee, decimal finalAmount, decimal taxAmount, decimal exchangeRate, string receiptIdentifier, IDictionary<string, string> attributes)
		{
			IOrder _order = GetOrder(order);

			_order.RegisterPayment(ref identifier, timestamp, method, transactionIdentifier, transactionType, status, statusShortMessage, statusLongMessage, paymentType, currency, amount, fee, finalAmount, taxAmount, exchangeRate, receiptIdentifier, attributes);
		}

		public IEnumerable<IPaymentInfo> GetPayments(string order, Range<DateTime> period, string method, string paymentType, string status, string currency, string exchangeRate, Range<DateTime> registrationPeriod)
		{
			IOrder _order = GetOrder(order);

			IEnumerable<IPaymentInfo> orderPayments = _order.GetPayments(period, method, paymentType, status, currency, registrationPeriod);

			return orderPayments;
		}

		public IPayment GetPayment(string identifier)
		{
			IScorpionDataProvider dataSession = null;
			IPaymentInfo paymentInfo;
			IPayment payment;

			try
			{
				dataSession = dataProviderFactory.GetSession();
				paymentInfo = dataSession.GetPayment(identifier);
				payment = new Payment(paymentInfo, dataProviderFactory, securityManager);

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

		public IEnumerable<IFulfillmentProcessInfo> GetFulfillmentProcesses(string order, Range<DateTime> lastFulfillmentProcessRegistrationPeriod, Range<DateTime> lastFulfillmentProgressPeriod, string currentFulfillmentProgresssStatus, string currentFulfillmentProgressAssignee, Range<DateTime> expectedFulfillmentProcessCompletionTimestamp)
		{
			IOrder _order = GetOrder(order);

			IEnumerable<IFulfillmentProcessInfo> fulfillmentProcesses = _order.GetFulfillmentProcesses(lastFulfillmentProcessRegistrationPeriod, expectedFulfillmentProcessCompletionTimestamp, currentFulfillmentProgresssStatus, currentFulfillmentProgressAssignee, expectedFulfillmentProcessCompletionTimestamp);

			return fulfillmentProcesses;
		}

		public IFulfillmentProcess GetFulfillmentProcess(string identifier)
		{
			IScorpionDataProvider dataSession = null;
			IFulfillmentProcessInfo fulfillmentProcessInfo;
			IFulfillmentProcess fulfillmentProcess;

			try
			{
				dataSession = dataProviderFactory.GetSession();
				fulfillmentProcessInfo = dataSession.GetFulfillmentProcess(identifier);
				fulfillmentProcess = new OrderFulfillmentProcess(fulfillmentProcessInfo, dataProviderFactory, securityManager);

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
