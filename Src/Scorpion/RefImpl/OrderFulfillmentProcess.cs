using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using Pug.Application.Security;

namespace Pug.Scorpion
{
	[DataContract]
	public class OrderFulfillmentProcess : Entity, Pug.Scorpion.IFulfillmentProcess
	{
		[DataContract]
		public class _Info : Pug.Scorpion.IFulfillmentProcessInfo
		{
			string order, identifier, status, registrationUser;

			[DataMember]
			DateTime startTimestamp, expectedCompletionTimestamp, registrationTimestamp;

			public _Info(string identifier, string order, DateTime startTimestamp, string status, DateTime expectedCompletionTimestamp, DateTime completionTimestamp, DateTime registrationTimestamp, string registrationUser)
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
		public class _Progress : Pug.Scorpion.IFulfillmentProgress
		{
			[DataContract]
			public class _Info : Pug.Scorpion.IFulfillmentProgressInfo
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
						this.identifier = value;
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

			IFulfillmentProgressInfo info;
			IEnumerable<EntityAttribute> attributes;

			public _Progress(IFulfillmentProgressInfo info, IEnumerable<EntityAttribute> attributes)
			{
				this.info = info;
				this.attributes = attributes;
			}

			[DataMember]
			public IFulfillmentProgressInfo Info
			{
				get { return info; }
				protected set { info = value; }
			}

			[DataMember]
			public IEnumerable<EntityAttribute> Attributes
			{
				get { return attributes; }
				protected set { attributes = value; }
			}
		}

		IFulfillmentProcessInfo info;

		object progressIdentifierSync = new object();

		public OrderFulfillmentProcess(IFulfillmentProcessInfo info, IScorpionDataProviderFactory dataProviderFactory, ISecurityManager securityManager)
			: base(dataProviderFactory, securityManager)
		{
			this.info = info;
		}

		string GetNewIdentifier(object sync)
		{
			byte[] binarySeed;

			lock (sync)
			{
				binarySeed = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
			}

			string newIdentifier = Pug.Base32.From(binarySeed).Replace("=", "");

			return newIdentifier;
		}

		[DataMember]
		public IFulfillmentProcessInfo Info
		{
			get { return info; }
			protected set { info = value; }
		}

		public IEnumerable<EntityAttribute> GetAttributes()
		{
			IScorpionDataProvider dataSession = null;

			IEnumerable<EntityAttribute> attributes;

			try
			{
				dataSession = DataProviderFactory.GetSession();

				attributes = dataSession.GetFulfillmentProcessAttributes(info.Identifier);
			}
			catch
			{
				throw;
			}
			finally
			{
				if (dataSession != null)
					dataSession.Dispose();
			}

			return attributes;
		}

		public string this[string name]
		{
			get
			{
				IScorpionDataProvider dataSession = null;

				try
				{
					dataSession = DataProviderFactory.GetSession();

					return dataSession.GetFulfillmentProcessAttribute(info.Identifier, name).Value;
				}
				catch
				{
					throw;
				}
				finally
				{
					if (dataSession != null)
						dataSession.Dispose();
				}
			}
			set
			{
				IScorpionDataProvider dataSession = null;

				try
				{
					dataSession = DataProviderFactory.GetSession();

					dataSession.SetFulfillmentProcessAttribute(info.Identifier, name, value, SecurityManager.CurrentUser.Identity.Identifier);
				}
				catch
				{
					throw;
				}
				finally
				{
					if (dataSession != null)
						dataSession.Dispose();
				}
			}
		}
				
		public void RegisterProgress(ref string identifier, string assignee, IDictionary<string, string> attributes, string comment, DateTime timestamp, string status, DateTime expectedCompletionTimestamp)
		{
			IScorpionDataProvider dataSession = null;

			try
			{
				dataSession = DataProviderFactory.GetSession();

				dataSession.BeginTransaction();

				if (string.IsNullOrEmpty(identifier))
					identifier = GetNewIdentifier(progressIdentifierSync);
				else
					if (dataSession.OrderExists(identifier))
						throw new OrderExists();

				dataSession.InsertFulfillmentProgress(info.Identifier, identifier, timestamp, status, assignee, comment, expectedCompletionTimestamp, SecurityManager.CurrentUser.Identity.Identifier);

				foreach(KeyValuePair<string, string> attribute in attributes)
					dataSession.InsertFulfillmentProgressAttribute(info.Identifier, identifier, attribute.Key, attribute.Value, SecurityManager.CurrentUser.Identity.Identifier);

				dataSession.CommitTransaction();

				info = dataSession.GetFulfillmentProcess(info.Identifier);
			}
			catch
			{
				dataSession.RollbackTransaction();

				throw;
			}
			finally
			{
				if (dataSession != null)
					dataSession.Dispose();
			}
		}

		public IEnumerable<IFulfillmentProgressInfo> GetProgresses()
		{		
			IScorpionDataProvider dataSession = null;

			IEnumerable<IFulfillmentProgressInfo> progresses = null;

			try
			{
				dataSession = DataProviderFactory.GetSession();

				progresses = dataSession.GetFulfillmentProgresses(info.Identifier);
			}
			catch
			{
				throw;
			}
			finally
			{
				if (dataSession != null)
					dataSession.Dispose();
			}
			
			return progresses;
		}

		public IFulfillmentProgress GetProgress(string identifier)
		{
			IScorpionDataProvider dataSession = null;

			IFulfillmentProgress progress = null;

			try
			{
				dataSession = DataProviderFactory.GetSession();

				progress = new _Progress(dataSession.GetFulfillmentProgress(info.Identifier, identifier), dataSession.GetFulfillmentProgressAttributes(info.Identifier, identifier));
			}
			catch
			{
				throw;
			}
			finally
			{
				if (dataSession != null)
					dataSession.Dispose();
			}

			return progress;
		}

		public void Cancel(IDictionary<string, string> attributes, string comment, DateTime timestamp)
		{
			IScorpionDataProvider dataSession = null;

			try
			{
				string identifier = GetNewIdentifier(progressIdentifierSync);

				dataSession = DataProviderFactory.GetSession();

				dataSession.BeginTransaction();

				dataSession.InsertFulfillmentProgress(info.Identifier, identifier, timestamp, "Cancellation", string.Empty, comment, timestamp, SecurityManager.CurrentUser.Identity.Identifier);

				foreach (KeyValuePair<string, string> attribute in attributes)
					dataSession.InsertFulfillmentProgressAttribute(info.Identifier, identifier, attribute.Key, attribute.Value, SecurityManager.CurrentUser.Identity.Identifier);

				dataSession.SetOrderStatus(info.Identifier, "CANCELLED", comment, SecurityManager.CurrentUser.Identity.Identifier);

				dataSession.CommitTransaction();

				info = dataSession.GetFulfillmentProcess(info.Identifier);
			}
			catch
			{
				dataSession.RollbackTransaction();

				throw;
			}
			finally
			{
				if (dataSession != null)
					dataSession.Dispose();
			}
		}

		public void Finalize( IDictionary<string, string> attributes, string comment, DateTime timestamp)
		{
			IScorpionDataProvider dataSession = null;

			try
			{
				string identifier = GetNewIdentifier(progressIdentifierSync);

				dataSession = DataProviderFactory.GetSession();

				dataSession.BeginTransaction();

				dataSession.InsertFulfillmentProgress(info.Identifier, identifier, timestamp, "Finalization", string.Empty, comment, timestamp, SecurityManager.CurrentUser.Identity.Identifier);

				foreach (KeyValuePair<string, string> attribute in attributes)
					dataSession.InsertFulfillmentProgressAttribute(info.Identifier, identifier, attribute.Key, attribute.Value, SecurityManager.CurrentUser.Identity.Identifier);

				dataSession.SetOrderStatus(info.Identifier, "FINALIZED", comment, SecurityManager.CurrentUser.Identity.Identifier);

				dataSession.CommitTransaction();

				info = dataSession.GetFulfillmentProcess(info.Identifier);
			}
			catch
			{
				dataSession.RollbackTransaction();

				throw;
			}
			finally
			{
				if (dataSession != null)
					dataSession.Dispose();
			}
		}

		public override void Refresh()
		{
			IScorpionDataProvider dataSession = null;

			try
			{
				dataSession = DataProviderFactory.GetSession();

				info = dataSession.GetFulfillmentProcess(info.Identifier);
			}
			catch
			{
				throw;
			}
			finally
			{
				if (dataSession != null)
					dataSession.Dispose();
			}
		}
	}
}
