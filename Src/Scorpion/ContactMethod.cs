using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Pug.Bizcotty;
using Pug.Bizcotty.Geography;

namespace Pug.Scorpion
{
	public class ContactMethod
	{
		string purpose, name, type, destination;

		public ContactMethod(string purpose, string name, string type, string destination)
		{
			this.purpose = purpose;
			this.type = type;
			this.destination = destination;
		}

		public string Purpose
		{
			get { return purpose; }
			protected set { purpose = value; }
		}

		public string Name
		{
			get { return name; }
			protected set { name = value; }
		}

		public string Type
		{
			get { return type; }
			protected set { type = value; }
		}

		public string Destination
		{
			get { return destination; }
			protected set { destination = value; }
		}
	}
}
