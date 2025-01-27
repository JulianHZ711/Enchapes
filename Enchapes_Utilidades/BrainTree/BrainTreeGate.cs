using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Braintree;
using Microsoft.Extensions.Options;

namespace Enchapes_Utilidades.BrainTree
{
    public class BrainTreeGate : IBrainTreeGate
    {

        public BrainTreeSettings _options { get; set; }
        public IBraintreeGateway brainTreeGateway { get; set; }

        public BrainTreeGate(IOptions<BrainTreeSettings> options)
        {
            _options = options.Value;
        }

        public IBraintreeGateway CreateGateway()
        {
            return new BraintreeGateway(_options.Environment, _options.MerchantId, _options.PublicKey, _options.PrivateKey);

        }

        public IBraintreeGateway GetGateway()
        {
            if (brainTreeGateway == null)
            {
                brainTreeGateway = CreateGateway();
            }
            return brainTreeGateway;
        }
    }
}
