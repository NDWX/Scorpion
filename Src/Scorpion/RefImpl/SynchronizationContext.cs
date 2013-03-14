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
}
