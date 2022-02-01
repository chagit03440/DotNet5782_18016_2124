using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlApi;

namespace BlApi
{
    
    public static class BlFactory
    {
        public static IBL GetBl() => BL.BLObject.Instance;
    }
}

