using System;

namespace Pug.Scorpion
{
	public interface IPaymentInfo
	{
		decimal Amount { get; }
		string Currency { get; }
		decimal ExchangeRate { get; }
		decimal Fee { get; }
		decimal FinalAmount { get; }
		string Identifier { get; }
		string Method { get; }
		string Order { get; }
		string PaymentType { get; }
		string ReceiptIdentifier { get; }
		string Status { get; }
		string StatusLongMessage { get; }
		string StatusShortMessage { get; }
		decimal TaxAmount { get; }
		DateTime Timestamp { get; set; }
		string TransactionIdentifier { get; }
		string TransactionType { get; }
	}
}
