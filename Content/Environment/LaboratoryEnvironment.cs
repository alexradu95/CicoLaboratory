using StereoKit;
using StereoKit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CicoLaboratory.Content.Environment
{
    // Laboratory environment from JumbliVR repository
    internal class LaboratoryEnvironment : IStepper
    {
        public bool Enabled => throw new NotImplementedException();
        Model apartmentModel;
        Random r = new Random();
        Dictionary<Vec4, Color> env = new Dictionary<Vec4, Color>();

        public bool Initialize()
        {
            BuildEnvironment();
            return true;
        }

        public void Shutdown()
        {
            throw new NotImplementedException();
        }

        public void Step()
        {
            var apartmentTransform = Matrix.S(new Vec3(0.01f, 0.01f, 0.01f));
            apartmentModel.Draw(apartmentTransform);
            foreach (KeyValuePair<Vec4, Color> kvp in env)
                Mesh.Sphere.Draw(Material.Default, Matrix.TS(kvp.Key.XYZ, SKMath.Lerp(.1f, .5f, kvp.Key.w)), kvp.Value);

            Text.Add("North", Matrix.TRS(Vec3.Forward * 5f, Quat.FromAngles(0, 180, 0), 3), Color.Black);
            Text.Add("South", Matrix.TRS(Vec3.Forward * -5f, Quat.FromAngles(0, 0, 0), 3), Color.Black);
            Text.Add("East", Matrix.TRS(Vec3.Right * 5f, Quat.FromAngles(0, 90, 0), 3), Color.Black);
            Text.Add("West", Matrix.TRS(Vec3.Right * -5f, Quat.FromAngles(0, 270, 0), 3), Color.Black);
            Text.Add("Home", Matrix.TRS(Vec3.Up * -1.6f, Quat.FromAngles(90, 180, 0), 3), Color.Black);
        }

        void BuildEnvironment()
        {
            apartmentModel = Model.FromFile("Apartment/apartment.obj");
            int count = 100;
            int envRange = 5;
            while (count > 0)
            {
                Vec4 v = new Vec4(r.NextInt64(envRange * -1, envRange), r.NextInt64(envRange * -1, envRange), r.NextInt64(envRange * -1, envRange), r.NextSingle());
                if (env.ContainsKey(v) == false)
                {
                    env.Add(v, Color.HSV(r.NextSingle(), .5f, .5f));
                    count--;
                }
            }
        }
    }
}
