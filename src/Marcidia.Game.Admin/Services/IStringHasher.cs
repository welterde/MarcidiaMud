using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace Marcidia.Game.Admin.Services
{
    interface IStringHasher
    {
        string Hash(string value);
    }
}
