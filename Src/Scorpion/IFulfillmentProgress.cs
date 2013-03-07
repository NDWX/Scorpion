using System;
namespace Pug.Scorpion
{
	public interface IFulfillmentProgress
	{
		System.Collections.Generic.IEnumerable<Pug.Scorpion.EntityAttribute> Attributes { get; }
		Pug.Scorpion.IFulfillmentProgressInfo Info { get; }
	}
}
