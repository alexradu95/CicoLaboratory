using StereoKit.Framework;

namespace Framework
{
    public abstract class NodeLifecycle : IStepper
    {
        private bool enabled;

        public bool Enabled => enabled;

        public virtual bool Initialize()
        {
            if (!enabled)
            {
                enabled = true;
                OnEnterTree();
                OnReady();
            }
            return true;
        }

        public abstract void Step();

        public virtual void Shutdown() { }

        protected virtual void OnEnterTree() { }

        protected virtual void OnReady() { }
    }

}
