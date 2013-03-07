using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Pug.Scorpion
{
	public interface IPayment
	{
		IEnumerable<EntityAttribute> GetAttributes();

		IPaymentInfo Info { get; set; }

		string this[string name] { get; set; }

		void Refresh();

		void Void(string comment);
	}
}
