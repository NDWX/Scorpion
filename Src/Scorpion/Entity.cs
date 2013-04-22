using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using Pug.Application.Data;

using Pug.Application.Security;
using Pug.Bizcotty;
using Pug.Bizcotty.Geography;

namespace Pug.Scorpion
{
    public abstract class Entity
    {
		IApplicationData<IScorpionDataProvider> dataProviderFactory;
		ISecurityManager securityManager;

		protected Entity(IApplicationData<IScorpionDataProvider> dataProviderFactory, ISecurityManager securityManager)
        {
            this.dataProviderFactory = dataProviderFactory;
			this.securityManager = securityManager;
        }

		protected IApplicationData<IScorpionDataProvider> DataProviderFactory
        {
            get
            {
                return dataProviderFactory;
            }
        }

		protected ISecurityManager SecurityManager
		{
			get
			{
				return securityManager;
			}
		}

		public abstract void Refresh();
	}
}
