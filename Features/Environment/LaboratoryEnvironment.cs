using StereoKit;
using StereoKit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace CicoLaboratory.Content.Environment
{
    // Laboratory environment from JumbliVR repository
    internal class LaboratoryEnvironment : IStepper
    {
        Model model;
        const int cacheCount = 1000;
        List<Pose> poseCache = new List<Pose>(cacheCount);
        public bool Enabled => throw new NotImplementedException();
        public bool Initialize()
        {
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

            DrawFloor();
            Text.Add("North", Matrix.TRS(Vec3.Forward * 5f, Quat.FromAngles(0, 180, 0), 3), Color.Black);
            Text.Add("South", Matrix.TRS(Vec3.Forward * -5f, Quat.FromAngles(0, 0, 0), 3), Color.Black);
            Text.Add("East", Matrix.TRS(Vec3.Right * 5f, Quat.FromAngles(0, 90, 0), 3), Color.Black);
            Text.Add("West", Matrix.TRS(Vec3.Right * -5f, Quat.FromAngles(0, 270, 0), 3), Color.Black);
            Text.Add("Home", Matrix.TRS(Vec3.Up * -1.6f, Quat.FromAngles(90, 180, 0), 3), Color.Black);


            poseCache.RemoveAt(poseCache.Count - 1);
            poseCache.Insert(0, curr);
        }

        private void DrawFloor()
        {
            for (int x = 0; x < 100; x++)
            {
                for (int z = 0; z < 100; z++)
                {
                    Vec3 grid = new Vec3(x, 0, z);
                    Pose pose = poseCache[(int)((cacheCount - 1))];
                    pose.position += grid;
                    Mesh.Cube.Draw(Material.Default, pose.ToMatrix(0.9f));
                }
            }
        }
    }
}
