using Glimpse.Core.Extensibility;

namespace Glimpse.Wcf
{
    public class WcfInspector : IInspector
    {
        public void Setup(IInspectorContext context)
        {
            GlimpseWcfClientInspector.Initialize(context.MessageBroker, context.TimerStrategy);
        }
    }
}