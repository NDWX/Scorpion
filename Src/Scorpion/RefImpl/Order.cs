using System;
using System.Collections.Generic;

using System.Runtime.Serialization;

using System.Transactions;

using Pug.Application.Security;

using Pug.Bizcotty;
using Pug.Bizcotty.Geography;


namespace Pug.Scorpion
{
	[DataContract]
	public class Order : Entity, IOrder
	{
		[DataContract]
		public class _Info : IOrderInfo
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

			int numberOfPaymentsReceived;
			decimal totalPaymentsReceived;
			DateTime lastPaymentReceivedTimestamp;

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
			public int NumberOfPaymentsReceived
			{
				get { return this.numberOfPaymentsReceived; }
			}

			[DataMember]
			public decimal TotalPaymentsReceived
			{
				get { return this.totalPaymentsReceived; }
			}

			[DataMember]
			public DateTime LastPaymentReceivedTimestamp
			{
				get { return this.lastPaymentReceivedTimestamp; }
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

		IOrderInfo info;

		SynchronizationContext synchronizationContext;

		public Order(IOrderInfo info, IScorpionDataProviderFactory dataProviderFactory, SynchronizationContext synchronizationContext, ISecurityManager securityManager)
			: base(dataProviderFactory, securityManager)
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
		public IOrderInfo Info
		{
			get { return info; }
			protected set { info = value; }
		}

		public IEnumerable<ContactMethod> GetContactMethods(string purpose, string type)
		{
			IScorpionDataProvider dataSession = null;

			IEnumerable<ContactMethod> contactMethods = null;

			try
			{
				dataSession = DataProviderFactory.GetSession();

				contactMethods = dataSession.GetContactMethods(purpose, type);
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

			return contactMethods;
		}

		public void AddContactMethod(string purpose, string name, string type, string destination)
		{
			IScorpionDataProvider dataSession = null;

			try
			{
				dataSession = DataProviderFactory.GetSession();

				dataSession.InsertContactMethod(info.Identifier, purpose, name, type, destination, SecurityManager.CurrentUser.Identity.Identifier);
			}
			catch
			{
				throw;
			}
			finally
			{
				if( dataSession != null )
					dataSession.Dispose();
			}
		}

		public void UpdateContactMethod(string purpose, string name, string type, string destination)
		{
			IScorpionDataProvider dataSession = null;

			try
			{
				dataSession = DataProviderFactory.GetSession();

				dataSession.UpdateContactMethod(info.Identifier, purpose, name, type, destination, SecurityManager.CurrentUser.Identity.Identifier);
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
		}

		public void DeleteContactMethod(string purpose, string name)
		{
			IScorpionDataProvider dataSession = null;

			try
			{
				dataSession = DataProviderFactory.GetSession();

				dataSession.DeleteContactMethod(info.Identifier, purpose, name, SecurityManager.CurrentUser.Identity.Identifier);
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
					if (dataStore.PaymentExists(identifier))
						throw new OrderExists();

				dataStore.InsertPayment(identifier, info.Identifier, timestamp, method, transactionIdentifier, transactionType, status, statusShortMessage, statusLongMessage, paymentType, currency, amount, fee, finalAmount, taxAmount, exchangeRate, receiptIdentifier, SecurityManager.CurrentUser.Identity.Identifier);

				foreach( KeyValuePair<string, string> attribute in attributes)
					dataStore.SetPaymentAttribute(identifier, attribute.Key, attribute.Value, SecurityManager.CurrentUser.Identity.Identifier);

				dataStore.CommitTransaction();

				info = dataStore.GetOrder(info.Identifier);
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

		public IEnumerable<IPaymentInfo> GetPayments(Range<DateTime> period, string method, string paymentType, string status, string currency, Range<DateTime> registrationPeriod)
		{
			IScorpionDataProvider dataSession = null;

			IEnumerable<IPaymentInfo> payments = null;

			try
			{
				dataSession = DataProviderFactory.GetSession();

				payments = dataSession.GetPayments(info.Identifier, period, method, paymentType, status, currency, registrationPeriod);
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

			return payments;
			
		}

		public IPayment GetPayment(string identifier)
		{
			IScorpionDataProvider dataSession = null;

			IPaymentInfo paymentInfo = null;

			try
			{
				dataSession = DataProviderFactory.GetSession();

				paymentInfo = dataSession.GetPayment(identifier, info.Identifier);
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

			if( paymentInfo != null )
				return new Payment(paymentInfo, DataProviderFactory, SecurityManager);
			else
				return null;
		}
		
		public void VoidPayment(string identifier, string comment)
		{
			GetPayment(identifier).Void(comment);
		}

		public void Cancel(string comment)
		{
			IScorpionDataProvider dataSession = null;
			
			try
			{
				dataSession = DataProviderFactory.GetSession();

				dataSession.SetOrderStatus(info.Identifier, "CANCELLED", comment, SecurityManager.CurrentUser.Identity.Identifier);
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
		}

		public void RegisterFulfillmentProcess(ref string identifier, string asignee, IDictionary<string, string> attributes, string comment, DateTime timestamp, string status, DateTime expectedStatusCompletionTimestamp, DateTime expectedCompletionTimestamp)
		{
			IScorpionDataProvider dataStore = null;

			using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required))
			{
				try
				{
					//RegisterOrder(identifier, cart, buyerName, buyerAddress, buyerContactPerson, payerName, billingAddress, billingContactPerson, orderPriceTotal, shippingCost, buyerNote, shippingName, shippingAddress, shippingContactPerson, contactMethods, attributes);

					dataStore = DataProviderFactory.GetSession();

					if (string.IsNullOrEmpty(identifier))
						identifier = GetNewFulfillmentProcessIdentifier();
					else
						if (dataStore.OrderExists(identifier))
							throw new OrderExists();
							
					dataStore.InsertFulfillmentProcess(identifier, asignee, comment, timestamp, status, expectedStatusCompletionTimestamp, expectedCompletionTimestamp, SecurityManager.CurrentUser.Identity.Identifier);

					foreach (KeyValuePair<string, string> attribute in attributes)
						dataStore.SetFulfillmentProcessAttribute(identifier, attribute.Key, attribute.Value, SecurityManager.CurrentUser.Identity.Identifier);

					transactionScope.Complete();
				}
				catch
				{
					throw;
				}
				finally
				{
					if( dataStore != null)
						dataStore.Dispose();
				}
			}
		}

		public IEnumerable<IFulfillmentProcessInfo> GetFulfillmentProcesses(Range<DateTime> lastFulfillmentProcessRegistrationPeriod, Range<DateTime> lastFulfillmentProgressPeriod, string currentFulfillmentProgresssStatus, string currentFulfillmentProgressAssignee, Range<DateTime> expectedFulfillmentProcessCompletionTimestamp)
		{
			IScorpionDataProvider dataSession = null;

			try
			{
				dataSession = DataProviderFactory.GetSession();

				return dataSession.GetFulfillmentProcesses(lastFulfillmentProcessRegistrationPeriod, lastFulfillmentProgressPeriod, currentFulfillmentProgresssStatus, currentFulfillmentProgressAssignee, expectedFulfillmentProcessCompletionTimestamp);
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
		}

		public IFulfillmentProcess GetFulfillmentProcess(string identifier)
		{
			IScorpionDataProvider dataSession = null;

			IFulfillmentProcessInfo fulfillmentProcessInfo = null;

			try
			{
				dataSession = DataProviderFactory.GetSession();

				fulfillmentProcessInfo = dataSession.GetFulfillmentProcess(identifier, info.Identifier);
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

			if (fulfillmentProcessInfo != null)
				return new OrderFulfillmentProcess(fulfillmentProcessInfo, DataProviderFactory, SecurityManager);
			else
				return null;
		}

		public void CancelFulfillmentProcess(string identifier, IDictionary<string, string> attributes, string comment, DateTime timestamp)
		{
			GetFulfillmentProcess(identifier).Cancel(attributes, comment, timestamp);
		}

		public void FinalizeFulfillmentProcess(string identifier, IDictionary<string, string> attributes, string comment, DateTime timestamp)
		{
			GetFulfillmentProcess(identifier).Finalize(attributes, comment, timestamp);
		}

		public IEnumerable<EntityAttribute> GetAttributes()
		{
			IScorpionDataProvider dataSession = null;
			
			try
			{
				dataSession = DataProviderFactory.GetSession();

				return dataSession.GetOrderAttributes(info.Identifier);
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
		}

		public string this[string attribute]
		{
			get
			{
				IScorpionDataProvider dataSession = null;

				try
				{
					dataSession = DataProviderFactory.GetSession();

					return dataSession.GetOrderAttribute(info.Identifier, attribute).Value;
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
			}
			set
			{
				IScorpionDataProvider dataSession = null;

				try
				{
					dataSession = DataProviderFactory.GetSession();

					dataSession.SetOrderAttribute(info.Identifier, attribute, value, SecurityManager.CurrentUser.Identity.Identifier);
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
			}
		}

		public override void Refresh()
		{
			IScorpionDataProvider dataSession = null;

			try
			{
				dataSession = DataProviderFactory.GetSession();

				info = dataSession.GetOrder(info.Identifier);
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
		}
	}
}
