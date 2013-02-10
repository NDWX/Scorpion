using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Pug.Bizcotty;
using Pug.Bizcotty.Geography;

namespace Pug.Scorpion
{
    public class Entity
    {
        IScorpionDataProviderFactory dataProviderFactory;

        public Entity(IScorpionDataProviderFactory dataProviderFactory)
        {
            this.dataProviderFactory = dataProviderFactory;
        }

        protected IScorpionDataProviderFactory DataProviderFactory
        {
            get
            {
                return dataProviderFactory;
            }
        }
    }
}
