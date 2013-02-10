using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Pug.Scorpion
{
	[DataContract]
	public class OrderFulfillmentProcess : Entity
	{
		[DataContract]
		public class _Info
		{
			string order, identifier, status, registrationUser;

			[DataMember]
			DateTime startTimestamp, expectedCompletionTimestamp, registrationTimestamp;

			public _Info(string identifier, DateTime startTimestamp, string status, DateTime expectedCompletionTimestamp, DateTime registrationTimestamp, string registrationUser)
			{
				this.order = order;
				this.identifier = identifier;
				this.startTimestamp = startTimestamp;
				this.status = status;
				this.expectedCompletionTimestamp = expectedCompletionTimestamp;
				this.registrationTimestamp = registrationTimestamp;
				this.registrationUser = registrationUser;
			}

			[DataMember]
			public string Status
			{
				get { return status; }
				protected set { status = value; }
			}

			[DataMember]
			public string RegistrationUser
			{
				get { return registrationUser; }
				protected set { registrationUser = value; }
			}

			[DataMember]
			public string Identifier
			{
				get { return identifier; }
				protected set { identifier = value; }
			}

			[DataMember]
			public string Order
			{
				get { return order; }
				protected set { order = value; }
			}

			[DataMember]
			public DateTime StartTimestamp
			{
			  get { return startTimestamp; }
			  protected set { startTimestamp = value; }
			}

			[DataMember]
			public DateTime ExpectedCompletionTimestamp
			{
			  get { return expectedCompletionTimestamp; }
			  protected set { expectedCompletionTimestamp = value; }
			}

			[DataMember]
			public DateTime RegistrationTimestamp
			{
			  get { return registrationTimestamp; }
			  protected set { registrationTimestamp = value; }
			}
		}

		[DataContract]
		public class _Progress
		{
			[DataContract]
			public class _Info
			{
				DateTime timestamp, expectedCompletionTimestamp, registrationTimestamp;
				string identifier, comment, status, registrationUser;
				
				[DataMember]
				public string Identifier
				{
					get
					{
						return this.identifier;
					}
					protected set
					{
						this.identifier = identifier;
					}
				}

				[DataMember]
				public DateTime Timestamp
				{
					get { return timestamp; }
					protected set { timestamp = value; }
				}

				[DataMember]
				public DateTime ExpectedCompletionTimestamp
				{
					get { return expectedCompletionTimestamp; }
					protected set { expectedCompletionTimestamp = value; }
				}

				[DataMember]
				public DateTime RegistrationTimestamp
				{
					get { return registrationTimestamp; }
					protected set { registrationTimestamp = value; }
				}

				[DataMember]
				public string Comment
				{
					get { return comment; }
					protected set { comment = value; }
				}

				[DataMember]
				public string Status
				{
					get { return status; }
					protected set { status = value; }
				}

				[DataMember]
				public string RegistrationUser
				{
					get { return registrationUser; }
					protected set { registrationUser = value; }
				}
			}

			_Info info;
			IDictionary<string, string> attributes;

			public _Progress(_Info info, IDictionary<string, string> attributes)
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
				get { return attributes; }
				protected set { attributes = value; }
			}
		}

		_Info info;
		IEnumerable<_Progress._Info> progress;

		public OrderFulfillmentProcess(_Info info, IScorpionDataProviderFactory dataProviderFactory)
			: base(dataProviderFactory)
		{
			this.info = info;
		}
		
		public void RegisterProgress(ref string identifier, string assignee, IDictionary<string, string> attributes, string comment, DateTime timestamp, string status, DateTime expectedCompletionTimestamp)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<_Progress._Info> GetProgresses()
		{
			return null;
		}

		public _Progress GetProgress(string identifier)
		{
			return null;
		}

		public void Cancel(IDictionary<string, string> attributes, string comment, DateTime timestamp)
		{
			throw new NotImplementedException();
		}

		public void Finalize( IDictionary<string, string> attributes, string comment, DateTime timestamp)
		{
			throw new NotImplementedException();
		}

		[DataMember]
		public _Info Info
		{
			get { return info; }
			protected set { info = value; }
		}

		[DataMember]
		public IEnumerable<_Progress._Info> Progress
		{
			get { return progress; }
			protected set { progress = value; }
		}
	}
}
