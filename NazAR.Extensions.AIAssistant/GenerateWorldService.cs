using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using StereoKit.Framework;

namespace Nazar.Extension.AIWorldGenerator
{
    internal class GenerateWorldService : IStepper
    {

        //AI generated game objects
        private readonly int myIdCounter = 0;
        private readonly List<Object> objects = new();

        public bool Enabled => throw new NotImplementedException();

        public bool Initialize()
        {
            return true;
        }

        public void Shutdown()
        {
        }

        public void Step()
        {
            foreach (Object o in objects) o.Draw();
        }

        public void HandleInput(string input)
        {
            JObject JResponce = JObject.Parse(input);
            int id = (int)JResponce.GetValue("id");

            //Remove
            JResponce.TryGetValue("remove", out JToken JRemove);
            JResponce.TryGetValue("delete", out JToken JDelete);
            bool remove = JRemove != null && (bool)JRemove;
            bool delete = JDelete != null && (bool)JDelete;
            if (remove || delete)
            {
                for (int i = 0; i < objects.Count; i++)
                    if (objects[i].myId == id)
                    {
                        int lastIndex = objects.Count - 1;
                        objects[i] = objects[lastIndex];
                        objects.RemoveAt(lastIndex);
                        i--; //new object at current postion
                        break;
                    }
            }
            else //Update or add new object
            {
                bool foundObject = false;
                for (int i = 0; i < objects.Count; i++)
                    if (objects[i].myId == id)
                    {
                        objects[i].UpdateFromJSON(JResponce);
                        foundObject = true;
                        break;
                    }

                if (!foundObject) //Create a new object
                    objects.Add(new Object(id, JResponce));
            }
        }
    }
}
