using System;
using System.Collections.Generic;
using StereoKit;
using StereoKit.Framework;

namespace nazar.core.Features.Demos
{
    class DemoManyObjects : IStepper
    {
        Model model;
        const int cacheCount = 1000;
        List<Pose> poseCache = new List<Pose>(cacheCount);
        public bool Enabled => throw new NotImplementedException();
        public bool Initialize()
        {

            model = Model.FromFile("DamagedHelmet.gltf");

            for (int i = 0; i < cacheCount; i++)
            {
                poseCache.Add(Pose.Identity);
            }
            return true;
        }

        public void Shutdown() { }

        public void Step()
        {
            Pose curr = poseCache[0];
            UI.Handle("Model", ref curr, model.Bounds * 0.1f);

            for (int y = 0; y < 20; y++)
            {
                for (int x = 0; x < 20; x++)
                {
                    Vec3 grid = new Vec3(x - 2f, 0, y - 4) * 0.5f;
                    float dist = Math.Min(1, grid.Length / 10.0f);
                    Pose pose = poseCache[(int)(dist * (cacheCount - 1))];
                    pose.position += grid;
                    model.Draw(pose.ToMatrix(0.1f));
                }
            }

            poseCache.RemoveAt(poseCache.Count - 1);
            poseCache.Insert(0, curr);
        }
    }
}
