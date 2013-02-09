using System.Collections.Generic;


using Pug.Extensions;

namespace Pug.Scorpion
{
	[DataContract]
	public class OrderItemLine
	{
		[DataContract]
		public class _Info
		{
			string order, identifier, productIdentifier, productCategory, productName;
			decimal quantity, unitPrice;

			public _Info(string order, string identifier, string productIdentifier, string productCategory, string productName, decimal quantity, decimal unitPrice)
			{
				this.order = order;
				this.identifier = identifier;
				this.productIdentifier = productIdentifier;
				this.productCategory = productCategory;
				this.productName = productName;
				this.quantity = quantity;
				this.unitPrice = unitPrice;
			}

			[DataMember]
			public string Order
			{
				get { return order; }
				protected set { order = value; }
			}

			[DataMember]
			public string Identifier
			{
				get { return identifier; }
				protected set { identifier = value; }
			}

			[DataMember]
			public string ProductIdentifier
			{
				get { return productIdentifier; }
				protected set { productIdentifier = value; }
			}

			[DataMember]
			public string ProductName
			{
				get { return productName; }
				protected set { productName = value; }
			}

			[DataMember]
			public string ProductCategory
			{
				get { return productCategory; }
				protected set { productCategory = value; }
			}

			[DataMember]
			public decimal Quantity
			{
				get { return quantity; }
				protected set { quantity = value; }
			}

			[DataMember]
			public decimal UnitPrice
			{
				get { return unitPrice; }
				protected set { unitPrice = value; }
			}
		}

		_Info info;
		IDictionary<string, string> attributes;

		public OrderItemLine(_Info info, IDictionary<string, string> attributes)
		{
			this.info = info;
			this.attributes = attributes;
		}

		[DataMember]
		public _Info Info
		{
			get { return info; }
			protected set { info = value; }
		}

		[DataMember]
		public IDictionary<string, string> Attributes
		{
			get { return attributes.ReadOnly(); }
			protected set { attributes = value; }
		}
	}
}
