using System;

namespace Pug.Scorpion
{
	public interface IEntityAttribute
	{
		DateTime LastModificationTimestamp { get; }
		string LastModificationUser { get; }
		string Name { get; }
		string Value { get; }
	}
}
