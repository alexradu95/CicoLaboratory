using StereoKit;
using StereoKit.Framework;

namespace Nazar.Extension.Demos.Demos
{
    internal class DemoAnimations : IStepper
    {
        Model _srcModel;
        Model _anim1;
        Model _anim2;

        public bool Enabled => throw new System.NotImplementedException();

        public bool Initialize()
        {
            _srcModel = Model.FromFile("Cosmonaut.glb");
            _anim1 = _srcModel.Copy();
            _anim1.PlayAnim("Idle", AnimMode.Manual);
            _anim2 = _anim1.Copy();
            _anim2.PlayAnim("Jump", AnimMode.Manual);

            return true;
        }

        public void Shutdown()
        {
        }

        public void Step()
        {
            _anim1.AnimCompletion = Time.Totalf;
            _anim2.AnimCompletion = Time.Totalf;

            _srcModel.Draw(Matrix.TR(-1, -1.3f, -1, Quat.LookDir(-Vec3.Forward)));
            _anim1.Draw(Matrix.TR(0, -1.3f, -1, Quat.LookDir(-Vec3.Forward)));
            _anim2.Draw(Matrix.TR(1, -1.3f, -1, Quat.LookDir(-Vec3.Forward)));

        }

    }
}
