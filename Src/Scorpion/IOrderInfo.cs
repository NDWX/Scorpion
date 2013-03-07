using System;
namespace Pug.Scorpion
{
	public interface IOrderInfo
	{
		Pug.Bizcotty.Geography.Address BillingAddress { get; set; }
		Pug.Bizcotty.PersonName BillingContactPerson { get; set; }
		Pug.Bizcotty.Geography.Address BuyerAddress { get; }
		Pug.Bizcotty.PersonName BuyerContactPerson { get; }
		string BuyerName { get; }
		string BuyerNote { get; }
		string Identifier { get; }
		DateTime LastModificationTimestamp { get; }
		string LastModificationUser { get; }
		decimal OrderPriceTotal { get; }
		string PayerName { get; set; }
		DateTime RegistrationTimestamp { get; }
		string RegistrationUser { get; }
		Pug.Bizcotty.Geography.Address ShippingAddress { get; }
		Pug.Bizcotty.PersonName ShippingContactPerson { get; }
		decimal ShippingCost { get; }
		string Status { get; }
	}
}
