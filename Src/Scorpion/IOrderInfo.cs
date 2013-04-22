using System;

using Pug.Bizcotty;
using Pug.Bizcotty.Geography;

namespace Pug.Scorpion
{
	public interface IOrderInfo
	{
		Address BillingAddress { get; }

		PersonName BillingContactPerson { get; }

		Address BuyerAddress { get; }

		PersonName BuyerContactPerson { get; }

		string BuyerName { get; }

		string BuyerNote { get; }

		string Identifier { get; }

		DateTime LastModificationTimestamp { get; }

		string LastModificationUser { get; }

		decimal OrderPriceTotal { get; }

		string PayerName { get; }

		int NumberOfPaymentsReceived
		{
			get;
		}

		decimal TotalPaymentsReceived
		{
			get;
		}

		DateTime LastPaymentReceivedTimestamp
		{
			get;
		}

		DateTime RegistrationTimestamp { get; }

		string RegistrationUser { get; }

		Address ShippingAddress { get; }

		PersonName ShippingContactPerson { get; }

		decimal ShippingCost { get; }

		string Status { get; }

		DateTime LastFulfillmentProgress { get; }

		string LastProgressingFulfillmentProcess { get; }
	}
}
