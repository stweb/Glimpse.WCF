using System;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Glimpse.Wcf
{
    // http://msdn.microsoft.com/en-us/library/aa717047%28v=vs.110%29.aspx

    public class GlimpseBehaviorExtensionElement : BehaviorExtensionElement
    {
        public override Type BehaviorType
        {
            get
            {
                return typeof(GlimpseEndpointBehavior);
            }
        }

        protected override object CreateBehavior()
        {
            return new GlimpseEndpointBehavior();
        }
    }

    public class GlimpseEndpointBehavior : IEndpointBehavior
    {
        public void Validate(ServiceEndpoint endpoint)
        {
        }

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {            
            //clientRuntime.MessageInspectors.Add(new GlimpseInspector());
        }
    }
}