namespace Pandora.Interactions.UI
{
    public class Control : ControlElement
    {
        public delegate void ControlEventDelegate(Control control);

        public Control()
        { }

        public new Control Parent { get { return (Control)base.Parent; } }

        protected internal int ControlLevel
        {
            get
            {
                if (Parent is Control control)
                {
                    return Parent.ControlLevel + 1;
                }
                else
                {
                    return 0;
                }
            }
        }

        internal override void InternalOnLoad(SceneHandler handler)
        {
            base.InternalOnLoad(handler);
        }

        protected override void Destroy(bool disposing)
        { }
    }
}
