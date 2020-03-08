namespace Pandora.Engine
{
    public class RuntimeFrameEventArgs
    {
        public RuntimeFrameEventArgs()
        { }
        
        public float Milliseconds { get; internal set; }

        public float Secounds { get; internal set; }
    }
}