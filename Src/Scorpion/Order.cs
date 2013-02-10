using System;
using System.Collections.Generic;

using System.Runtime.Serialization;

using Pug.Bizcotty;
using Pug.Bizcotty.Geography;


namespace Pug.Scorpion
{
	[DataContract]
	public class Order : Entity
	{
		[DataContract]
		public class _Info
		{
			string identifier;
			string buyerName;//, buyerEmailAddress, buyerContactNumber;
			Address buyerAddress;
			PersonName buyerContactPerson;

			string payerName;
			Address billingAddress;
			PersonName billingContactPerson;

			decimal orderPriceTotal, shippingCost;
			string buyerNote;
			PersonName shippingContactPerson;
			Address shippingAddress;
			//string shippingContactNumber;

			string status;

			string registrationUser, lastModificationUser;
			DateTime registrationTimestamp, lastModificationTimestamp;

			public _Info(string identifier, string buyerName, Address buyerAddress, PersonName buyerContactPerson, decimal orderPriceTotal, decimal shippingCost, string buyerNote, string payerName, Address billingAddress, PersonName billingContactPerson, PersonName shippingContactPerson, Address shippingAddress, DateTime registrationTimestamp, string registrationUser, string status, DateTime lastModificationTimestamp, string lastModificationUser)
			{
				this.identifier = identifier;
				this.buyerContactPerson = buyerContactPerson;
				this.buyerName = buyerName;
				this.buyerAddress = buyerAddress;
				this.buyerNote = buyerNote;
				this.orderPriceTotal = orderPriceTotal;
				this.payerName = payerName;
				this.billingAddress = billingAddress;
				this.billingContactPerson = billingContactPerson;
				this.shippingCost = shippingCost;
				this.shippingContactPerson = shippingContactPerson;
				this.shippingAddress = shippingAddress;

				this.registrationUser = registrationUser;
				this.registrationTimestamp = registrationTimestamp;
				this.lastModificationUser = lastModificationUser;
				this.lastModificationTimestamp = lastModificationTimestamp;
			}

			public _Info(string identifier, string buyerName, Address buyerAddress, PersonName buyerContactPerson, decimal orderPriceTotal, decimal shippingCost, string buyerNote, string payerName, Address billingAddress, PersonName billingContactPerson, PersonName shippingContactPerson, Address shippingAddress)
				: this(identifier, buyerName, buyerAddress, buyerContactPerson, orderPriceTotal, shippingCost, buyerNote, payerName, billingAddress, billingContactPerson, shippingContactPerson, shippingAddress, DateTime.MaxValue, string.Empty, "NEW", DateTime.MaxValue, string.Empty)
			{
			}

			[DataMember]
			public string Identifier
			{
				get { return identifier; }
				protected set { identifier = value; }
			}

			[DataMember]
			public string BuyerName
			{
				get
				{
					return buyerName;
				}
				protected set { buyerName = value; }
			}

			[DataMember]
			public PersonName BuyerContactPerson
			{
				get
				{
					return buyerContactPerson;
				}
				protected set { buyerContactPerson = value; }
			}

			[DataMember]
			public Address BuyerAddress
			{
				get
				{
					return buyerAddress;
				}
				protected set { buyerAddress = value; }
			}

			[DataMember]
			public string PayerName
			{
				get { return payerName; }
				set { payerName = value; }
			}

			[DataMember]
			public Address BillingAddress
			{
				get { return billingAddress; }
				set { billingAddress = value; }
			}

			[DataMember]
			public PersonName BillingContactPerson
			{
				get { return billingContactPerson; }
				set { billingContactPerson = value; }
			}

			[DataMember]
			public decimal OrderPriceTotal
			{
				get { return orderPriceTotal; }
				protected set { orderPriceTotal = value; }
			}

			[DataMember]
			public decimal ShippingCost
			{
				get
				{
					return shippingCost;
				}
				protected set { shippingCost = value; }
			}

			[DataMember]
			public string BuyerNote
			{
				get
				{
					return buyerNote;
				}
				protected set { buyerNote = value; }
			}

			[DataMember]
			public PersonName ShippingContactPerson
			{
				get
				{
					return shippingContactPerson;
				}
				protected set { shippingContactPerson = value; }
			}

			[DataMember]
			public Address ShippingAddress
			{
				get
				{
					return shippingAddress;
				}
				protected set { shippingAddress = value; }
			}

			[DataMember]
			public string Status
			{
				get { return status; }
				protected set { status = value; }
			}

			[DataMember]
			public string RegistrationUser
			{
				get { return registrationUser; }
				protected set { registrationUser = value; }
			}

			[DataMember]
			public string LastModificationUser
			{
				get { return lastModificationUser; }
				protected set { lastModificationUser = value; }
			}

			[DataMember]
			public DateTime RegistrationTimestamp
			{
				get { return registrationTimestamp; }
				protected set { registrationTimestamp = value; }
			}

			[DataMember]
			public DateTime LastModificationTimestamp
			{
				get { return lastModificationTimestamp; }
				protected set { lastModificationTimestamp = value; }
			}
		}

		//[DataContract]
		//public class ItemLine
		//{
		//    string identifier, productIdentifier, productCategory, productName;
		//    decimal quantity, unitPrice;

		//    public ItemLine(string identifier, string productIdentifier, string productCategory, string productName, decimal quantity, decimal unitPrice)
		//    {
		//        this.identifier = identifier;
		//        this.productIdentifier = productIdentifier;
		//        this.productCategory = productCategory;
		//        this.productName = productName;
		//    }

		//    [DataMember]
		//    public string Identifier
		//    {
		//        get { return identifier; }
		//        protected set { identifier = value; }
		//    }

		//    [DataMember]
		//    public string ProductIdentifier
		//    {
		//        get { return productIdentifier; }
		//        protected set { productIdentifier = value; }
		//    }

		//    [DataMember]
		//    public string ProductCategory
		//    {
		//        get { return productCategory; }
		//        protected set { productCategory = value; }
		//    }

		//    [DataMember]
		//    public string ProductName
		//    {
		//        get { return productName; }
		//        protected set { productName = value; }
		//    }

		//    [DataMember]
		//    public decimal Quantity
		//    {
		//        get { return quantity; }
		//        protected set { quantity = value; }
		//    }

		//    [DataMember]
		//    public decimal UnitPrice
		//    {
		//        get { return unitPrice; }
		//        protected set { unitPrice = value; }
		//    }
		//}

		_Info info;
		//IEnumerable<OrderFulfillmentProcess._Info> fulfillmentProcesses;

		SynchronizationContext synchronizationContext;

		public Order(_Info info, IScorpionDataProviderFactory dataProviderFactory, SynchronizationContext synchronizationContext)
			: base(dataProviderFactory)
		{
			this.info = info;
			this.synchronizationContext = synchronizationContext;
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

		string GetNewPaymentIdentifier()
		{
			return GetNewIdentifier(synchronizationContext.PaymentIdentifierSync);
		}

		string GetNewFulfillmentProcessIdentifier()
		{
			return GetNewIdentifier(synchronizationContext.FulfillmentProcessIdentifierSync);
		}

		[DataMember]
		public _Info Info
		{
			get { return info; }
			protected set { info = value; }
		}

		public ICollection<ContactMethod> GetContactMethods(string purpose, string type)
		{
			return null;
		}

		public void SetContactMethod(string purpose, string type, string destination)
		{
		}

		public void DeleteContactMethod(string purpose, string type)
		{
		}


		public void RegisterPayment(ref string identifier, DateTime timestamp, string method, string transactionIdentifier, string transactionType, string status, string statusShortMessage, string statusLongMessage, string paymentType, string currency, decimal amount, decimal fee, decimal finalAmount, decimal taxAmount, decimal exchangeRate, string receiptIdentifier, IDictionary<string, string> attributes)
		{
			IScorpionDataProvider dataStore = base.DataProviderFactory.GetSession();

			dataStore.BeginTransaction();

			try
			{
				if (string.IsNullOrEmpty(identifier))
					identifier = GetNewPaymentIdentifier();
				else
					if (dataStore.OrderExists(identifier))
						throw new OrderExists();

				dataStore.InsertPayment(identifier, info.Identifier, timestamp, method, transactionIdentifier, transactionType, status, statusShortMessage, statusLongMessage, paymentType, currency, amount, fee, finalAmount, taxAmount, exchangeRate, receiptIdentifier);

				foreach( KeyValuePair<string, string> attribute in attributes)
					dataStore.InsertPaymentAttribute(identifier, attribute.Key, attribute.Value);

				dataStore.CommitTransaction();
			}
			catch
			{
				dataStore.RollbackTransaction();
				throw;
			}
			finally
			{
				dataStore.Dispose();
			}

		}

		public IEnumerable<Payment._Info> GetPayments(Range<DateTime> period, string method, string paymentType, string status, string currency)
		{
			throw new NotImplementedException();
		}

		public Payment GetPayment(string identifier)
		{
			return null;
		}

		public Payment GetPayment(string method, string transactionIdentifier)
		{
			throw new NotImplementedException();
		}

		public void VoidPayment(string method, string transactionIdentifier, string comment)
		{
			throw new NotImplementedException();
		}

		public void Cancel(string comment)
		{
			throw new NotImplementedException();
		}

		public void RegisterFulfillmentProcess(ref string identifier, string asignee, IDictionary<string, string> attributes, string comment, DateTime timestamp, string status, DateTime expectedStatusCompletionTimestamp, DateTime expectedCompletionTimestamp)
		{
			throw new NotImplementedException();
		}

		public OrderFulfillmentProcess._Info[] GetFulfillmentProcesses(Range<DateTime> lastFulfillmentProcessRegistrationPeriod, Range<DateTime> lastFulfillmentProgressPeriod, string currentFulfillmentProgresssStatus, string currentFulfillmentProgressAssignee, Range<DateTime> expectedFulfillmentProcessCompletionTimestamp)
		{
			throw new NotImplementedException();
		}

		public void CancelFulfillmentProcess(IDictionary<string, string> attributes, string comment, DateTime timestamp)
		{
			throw new NotImplementedException();
		}

		public void FinalizeFulfillmentProcess(IDictionary<string, string> attributes, string comment, DateTime timestamp)
		{
			throw new NotImplementedException();
		}

		public OrderFulfillmentProcess GetFulfillmentProcess(string identifier)
		{
			throw new NotImplementedException();
		}

		public string this[string attribute]
		{
			get
			{
				return null;
			}
			set
			{
			}
		}
	}
}
