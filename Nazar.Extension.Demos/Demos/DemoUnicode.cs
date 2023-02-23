﻿using StereoKit;
using StereoKit.Framework;

namespace Nazar.Extension.Demos.Demos;

class DemoUnicode : IStepper
{
    Model atlasModel;
    Model clipboard = Model.FromFile("Clipboard.glb", Shader.UI);

    Pose  clipboardPose  = new Pose(0.8f,-0.15f,-0.2f, Quat.LookDir(-1,0,1));
    Pose  unicodeWinPose = new Pose(0.5f,0,-0.5f, Quat.LookDir(-1,0,1));

    public bool Enabled => throw new System.NotImplementedException();
    string unicodeText = "";

    public bool Initialize()
    {
        atlasModel = new Model(Mesh.Quad, TextStyle.Default.Material);
        atlasModel.RootNode.LocalTransform = Matrix.T(0,0,-0.01f);
        return true;
    }

    public void Shutdown()
    {
    }

    public void Step()
    {
        UI.HandleBegin("", ref clipboardPose, clipboard.Bounds);
        clipboard.Draw(Matrix.Identity);
        UI.LayoutArea(V.XY0(12, 15) * U.cm, V.XY(24, 30) * U.cm);
        UI.Label("Font Atlas");
        UI.HSeparator();
        UI.Model(atlasModel);
        UI.HandleEnd();

        UI.WindowBegin(" Unicode Samples", ref unicodeWinPose);
        UI.Label("Just type some Unicode here:");
        UI.Input("UnicodeText", ref unicodeText);
        UI.HSeparator();
        UI.Text("古池や\n蛙飛び込む\n水の音\n- Matsuo Basho");
        UI.HSeparator();
        UI.Text("Съешь же ещё этих мягких французских булок да выпей чаю. Широкая электрификация южных губерний даст мощный толчок подъёму сельского хозяйства. В чащах юга жил бы цитрус? Да, но фальшивый экземпляр!");
        UI.HSeparator();
        UI.Text("Windows only symbols:\n");
        UI.WindowEnd();

        Vec3 at = V.XYZ(0.65f, -.15f, -.35f);
		
    }
}