using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hannon.FreightEngine.Models;


namespace Hannon.FreightEngine
{
    interface IFreightEngine
    {
        Address ValidateAddress(Address address);
    }
}
