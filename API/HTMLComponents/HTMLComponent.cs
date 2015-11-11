using NetDotNet.API.HTMLComponents.Attributes;

namespace NetDotNet.API.HTMLComponents
{
    public abstract class HTMLComponent
    {
        protected AttributesAbstract attributes;

        public abstract string ToRaw();
    }
}
