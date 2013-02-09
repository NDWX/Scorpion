using System;
using System.Collections.Generic;

using Pug.Application.Security;

using Pug.Bizcotty;
using Pug.Bizcotty.Geography;

namespace Pug.Scorpion
{
	public interface ICartInfoStoreProvider : Cartage.ICartInfoStoreProvider
	{
		void FinalizeCart(string identifier);
	}

	public interface ICartIntoStoreProviderFactory : Cartage.ICartInfoStoreProviderFactory
	{
	}
	
	public class Scorpion<Pi, Pp> : Sisca.ISisca<Pi, Pp>
		where Pi : Sisca.IProductInfo
		where Pp : Sisca.IProductInfoProvider<Pi>
	{
		Cartage.ICartage cartage;
        Pp productInfoProvider;
		Sisca.ISisca<Pi, Pp> sisca;

		public Scorpion(ICartIntoStoreProviderFactory cartStoreProviderFactory, Pp productInfoProvider, ISecurityManager securityManager)
		{
			this.productInfoProvider = productInfoProvider;
			cartage = new Cartage.Cartage(cartStoreProviderFactory, securityManager);

			sisca = new Sisca.Sisca<Pi, Pp>(cartage, productInfoProvider);
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
			PersonName shippingContactPerson, Address shippingAddress,
			IDictionary<string, string> buyerContactMethods,
			IDictionary<string, string> billingContactMethods,
			IDictionary<string, string> shippingContactMethods,
			IDictionary<string, string> attributes)
		{
		}

		public void CreateOrder(
			ref string identifier, string cart, string buyerName,
			Address buyerAddress, PersonName buyerContactPerson,
			string payerName, Address billingAddress, PersonName billingContactPerson,
			decimal orderPriceTotal, decimal shippingCost, string buyerNote,
			PersonName shippingContactPerson, Address shippingAddress,
			IDictionary<string, string> buyerContactMethods,
			IDictionary<string, string> billingContactMethods,
			IDictionary<string, string> shippingContactMethods,
			IDictionary<string, string> attributes,
			Payment payment)
		{
		}

		IEnumerable<Cartage.ICartInfo> GetOrders(IEnumerable<string> status, Range<DateTime> creationPeriod, IEnumerable<string> creationUser, Range<DateTime> lastModificationPeriod, IEnumerable<string> lastModificationUser)
		{
			return null;
		}

		public IEnumerable<Payment._Info> GetPayments(string order, Range<DateTime> period, IEnumerable<string> methods, IEnumerable<string> paymentTypes, IEnumerable<string> statuses, IEnumerable<string> currencies, IEnumerable<string> exchangeRates, Range<DateTime> registrationPeriod)
		{
			throw new NotImplementedException();
		}

		public Payment GetPayment(string method, string transactionIdentifier)
		{
			throw new NotImplementedException();
		}

		public OrderFulfillmentProcess._Info[] GetFulfillmentProcesses(string order = "", Range<DateTime> lastFulfillmentProcessRegistrationPeriod = null, Range<DateTime> lastFulfillmentProgressPeriod = null, IEnumerable<string> currentFulfillmentProgresssStatus = null, IEnumerable<string> currentFulfillmentProgressAssignee = null, Range<DateTime> expectedFulfillmentProcessCompletionTimestamp = null)
		{
			throw new NotImplementedException();
		}

		public OrderFulfillmentProcess GetFulfillmentProcess(string identifier)
		{
			throw new NotImplementedException();
		}

		#region IDisposable Members

		public void Dispose()
		{
			throw new System.NotImplementedException();
		}

		#endregion
	}
}
