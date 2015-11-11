using System.Collections.Generic;


namespace NetDotNet.API.HTMLComponents
{
    public abstract class NonterminatingComponent : HTMLComponent
    {
        protected List<HTMLComponent> subComponents;
    }
}
