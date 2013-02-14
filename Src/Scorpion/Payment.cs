using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using Pug.Application.Security;

namespace Pug.Scorpion
{
	[DataContract]
	public class Payment : Entity
	{
		[DataContract]
		public class _Info
		{
			string identifier, order;
			DateTime timestamp;
			string method;
			string status, statusShortMessage, statusLongMessage;
			string paymentType, transactionIdentifier, transactionType, currency;
			decimal amount, fee, finalAmount, taxAmount, exchangeRate;
			string receiptIdentifier;

			public _Info(string identifier, string order, DateTime timestamp, string method, string transactionIdentifier, string transactionType, string status, string statusShortMessage, string statusLongMessage, string paymentType, string currency, decimal amount, decimal fee, decimal finalAmount, decimal taxAmount, decimal exchangeRate, string receiptIdentifier, IDictionary<string, string> attributes)
			{
				this.identifier = identifier;
				this.order = order;
				this.timestamp = timestamp;
				this.method = method;
				this.status = status;
				this.statusShortMessage = statusShortMessage;
				this.statusLongMessage = statusLongMessage;
				this.paymentType = paymentType;
				this.transactionIdentifier = transactionIdentifier;
				this.transactionType = transactionType;
				this.currency = currency;
				this.amount = amount;
				this.fee = fee;
				this.finalAmount = finalAmount;
				this.taxAmount = taxAmount;
				this.exchangeRate = exchangeRate;
				this.receiptIdentifier = receiptIdentifier;
			}

			public string Identifier
			{
				get
				{
					return identifier;
				}
				protected set
				{
					identifier = value;
				}
			}

			[DataMember]
			public string Order
			{
				get { return order; }
				protected set { order = value; }
			}

			[DataMember]
			public DateTime Timestamp
			{
				get { return timestamp; }
				set { timestamp = value; }
			}

			[DataMember]
			public string TransactionIdentifier
			{
				get { return transactionIdentifier; }
				protected set { transactionIdentifier = value; }
			}

			[DataMember]
			public string TransactionType
			{
				get { return transactionType; }
				protected set { transactionType = value; }
			}

			[DataMember]
			public string Method
			{
				get { return method; }
				protected set { method = value; }
			}

			[DataMember]
			public string Status
			{
				get { return status; }
				protected set { status = value; }
			}

			[DataMember]
			public string StatusShortMessage
			{
				get { return statusShortMessage; }
				protected set { statusShortMessage = value; }
			}

			[DataMember]
			public string StatusLongMessage
			{
				get { return statusLongMessage; }
				protected set { statusLongMessage = value; }
			}

			[DataMember]
			public string PaymentType
			{
				get { return paymentType; }
				protected set { paymentType = value; }
			}

			[DataMember]
			public string Currency
			{
				get { return currency; }
				protected set { currency = value; }
			}

			[DataMember]
			public decimal ExchangeRate
			{
				get { return exchangeRate; }
				protected set { exchangeRate = value; }
			}

			[DataMember]
			public decimal Amount
			{
				get { return amount; }
				protected set { amount = value; }
			}

			[DataMember]
			public decimal Fee
			{
				get { return fee; }
				protected set { fee = value; }
			}

			[DataMember]
			public decimal FinalAmount
			{
				get { return finalAmount; }
				protected set { finalAmount = value; }
			}

			[DataMember]
			public decimal TaxAmount
			{
				get { return taxAmount; }
				protected set { taxAmount = value; }
			}

			[DataMember]
			public string ReceiptIdentifier
			{
				get { return receiptIdentifier; }
				protected set { receiptIdentifier = value; }
			}
		}
		_Info info;

		public Payment(_Info info, IScorpionDataProviderFactory dataProviderFactory, ISecurityManager securityManager)
			: base(dataProviderFactory, securityManager)
		{
			this.info = info;
		}

		[DataMember]
		public _Info Info
		{
		  get { return info; }
		  set { info = value; }
		}

		public string this[string name]
		{
			get
			{
				return null;
			}
			set
			{

			}
		}

		public void Void(string comment)
		{
			IScorpionDataProvider dataSession = null;

			try
			{
				dataSession = DataProviderFactory.GetSession();

				dataSession.SetOrderStatus(info.Identifier, PaymentStatus.Void, comment);
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

		public override void Refresh()
		{
			throw new NotImplementedException();
		}
	}
}
