using StereoKit;
using StereoKit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CicoLaboratory.Content
{
    internal class ControllerLocomotion : IStepper
    {
        public bool Enabled => throw new NotImplementedException();

        public bool Initialize()
        {
            return true;
        }

        public void Shutdown()
        {
            throw new NotImplementedException();
        }

        static private Vec3 worldControllerLocomotionDirection;
        static public Pose stagePose = Pose.Identity;

        // Used to move the player in a certain direction
        // The direction vector includes the speed as its length
        static public void GlidePlayer(Vec3 locomotionDirection)
        {
            float minMetersPerSecond = 1f;
            float maxMetersPerSecond = 10;
            float movementRangeMin = .05f;
            float movementRangeMax = .1f;

            float speed = SKMath.Lerp(minMetersPerSecond, maxMetersPerSecond, MathF.Min(movementRangeMax, locomotionDirection.Length - movementRangeMin) / movementRangeMax);
            stagePose.position += locomotionDirection * Time.Stepf * speed;
            UpdateStagePose(stagePose);
        }

        static public void UpdateStagePose(Pose pose)
        {
            stagePose = pose;
            Renderer.CameraRoot = stagePose.ToMatrix();
        }

        private float newControllerRotation;

        // The player may not be stood at the stage centre
        // so we need to rotate around the head position to reduce nausea
        public void RotatePlayer(Quat angle)
        {
            Hierarchy.Push(Matrix.T(Input.Head.position));
            stagePose = Hierarchy.ToLocal(stagePose);
            Hierarchy.Push(Matrix.R(angle));
            stagePose = Hierarchy.ToWorld(stagePose);
            Hierarchy.Pop();
            Hierarchy.Pop();
            UpdateStagePose(stagePose);
        }
        public void Step()
        {
            if (worldControllerLocomotionDirection.Length > .05f)
                GlidePlayer(worldControllerLocomotionDirection);

            float absRotation = Math.Abs(newControllerRotation);
            float rotationRangeMin = .3f;
            if (absRotation > rotationRangeMin)
            {
                float speed = Math.Abs(newControllerRotation) - rotationRangeMin;
                speed = 180 * Math.Min(.8f, speed) * Math.Sign(newControllerRotation) * Time.Stepf;
                RotatePlayer(Quat.FromAngles(V.XYZ(0, speed, 0)));

            }
            // Analogue stick operations
            Vec2 moveStick = Input.Controller(Handed.Left).stick;
            Vec2 rotateStick = Input.Controller(Handed.Right).stick;


            Vec3 indicator = Input.Hand(Handed.Left).palm.position + Input.Hand(Handed.Left).palm.Right * .1f;
            Hierarchy.Push(Input.Controller(Handed.Left).aim.ToMatrix());
            worldControllerLocomotionDirection = Hierarchy.ToWorldDirection(V.XYZ(0, 0, moveStick.y * -1));
            worldControllerLocomotionDirection += Vec3.Cross(Vec3.Up, (Input.Controller(Handed.Left).aim.Forward * V.XYZ(1, 0, 1)).Normalized) * moveStick.x;
            worldControllerLocomotionDirection *= .25f;
            Hierarchy.Pop();

            // Show a direction indicator above the hand
            if (worldControllerLocomotionDirection.Length > .05f)
            {

                Lines.Add(new LinePoint[]
                {
                    new LinePoint(indicator,Color.White.ToLinear(),.02f),
                    new LinePoint(indicator + worldControllerLocomotionDirection.Normalized * .03f , Color.Black.ToLinear(), .0001f)
                });
            }
            newControllerRotation = Input.Controller(Handed.Right).stick.x;

        }
    }
}
