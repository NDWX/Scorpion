using System;
using System.Collections.Generic;
using Pug.Application.Security;
using Pug.Bizcotty;
using Pug.Bizcotty.Geography;

namespace Pug.Scorpion
{
    public interface ICartInfoStoreProvider : Cartage.ICartInfoStoreProvider
    {
        void FinalizeCart(string identifier);

		bool CartIsFinalized(string identifier);
    }
}
