using StereoKit;
using StereoKit.Framework;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using SixLabors.ImageSharp;
using System.IO;

namespace CicoLaboratory.Features
{
    internal class ShowImage : IStepper
    {
        public bool Enabled => true;
        List<Tex> gifFramesTextures = new List<Tex>();
        Pose windowPose = new Pose(-.4f, 0, 0, Quat.LookDir(1, 0, 1));
        int currentFrameNumber = 0;

        public bool Initialize() {

            Task<Image> loadImageTask = RetrieveImageFromWeb("https://i.gifer.com/9zQg.gif");
            loadImageTask.ContinueWith(image =>
            {
                Image result = image.Result;


                while (result.Frames.Count > 0)
                {
                    var exportedFrame = result.Frames.ExportFrame(0);

                    using (MemoryStream ms = new())
                    {
                        exportedFrame.SaveAsJpeg(ms);
                        var texture = Tex.FromMemory(ms.ToArray());
                        gifFramesTextures.Add(texture);
                    }
                }

            });

            return true;
        }

        public void Shutdown()
        {
            throw new NotImplementedException();
        }

        public void Step()
        {
            UI.WindowBegin("Window", ref windowPose, new Vec2(20, 0) * U.cm, UIWin.Normal);


            if(gifFramesTextures.Count > 0)
            {
                if (currentFrameNumber == gifFramesTextures.Count - 1)
                {
                    currentFrameNumber = 0;
                }
                else
                {
                    currentFrameNumber++;
                }

                UI.Image(Sprite.FromTex(gifFramesTextures[currentFrameNumber]), new Vec2(20, 0) * U.cm);
            }


            UI.WindowEnd();
        }

        private async Task<Image> RetrieveImageFromWeb(string imageUrl)
        {
            using var httpClient = new HttpClient();
            using var response = await httpClient.GetAsync(imageUrl);

            if (response.IsSuccessStatusCode)
            {
                var byteArrayContent = await response.Content.ReadAsByteArrayAsync();
                return Image.Load(byteArrayContent);
            }

            return null; //should return a placeholder
        }

    }
}
