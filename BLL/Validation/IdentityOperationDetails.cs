using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Validation
{
    public class IdentityOperationDetails
    {
        public IdentityOperationDetails(bool succedeed, string message, string prop)
        {
            Succedeed = succedeed;
            Message = message;
            Property = prop;
        }
        public bool Succedeed { get; private set; }
        public string Message { get; private set; }
        public string Property { get; private set; }

    }
}
