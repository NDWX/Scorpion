using System;
using System.Collections.Generic;
using Pug.Application.Security;
using Pug.Bizcotty;
using Pug.Bizcotty.Geography;
using Pug.Application.Data;

namespace Pug.Scorpion
{
	public class EntityAttribute : Pug.Scorpion.IEntityAttribute
	{
		string name, value, lastModificationUser;
		DateTime lastModificationTimestamp;

		public string Name
		{
			get { return name; }
			protected set { name = value; }
		}

		public string Value
		{
			get { return this.value; }
			protected set { this.value = value; }
		}

		public string LastModificationUser
		{
			get { return lastModificationUser; }
			protected set { lastModificationUser = value; }
		}

		public DateTime LastModificationTimestamp
		{
			get { return lastModificationTimestamp; }
			protected set { lastModificationTimestamp = value; }
		}
	}
}
