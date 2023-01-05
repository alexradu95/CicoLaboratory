using StereoKit;
using StereoKit.Framework;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using SixLabors.ImageSharp;
using Microsoft.Maui.Controls.Xaml;
using SixLabors.ImageSharp.PixelFormats;
using System.IO;
using SixLabors.ImageSharp.Memory;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Gif;

namespace CicoLaboratory.Features
{
    internal class ShowImage : IStepper
    {
        public bool Enabled => throw new NotImplementedException();
        Pose windowPose = new Pose(-.4f, 0, 0, Quat.LookDir(1, 0, 1));
        List<Byte[]> gifFramesInByteArray;
        Sprite logoSprite;
        int currentFrameNumber = 0;

        public bool Initialize() {

            gifFramesInByteArray = GetGifFramesAsync("https://upload.wikimedia.org/wikipedia/commons/2/2c/Rotating_earth_%28large%29.gif").Result;

            return true;
        }

        public void Shutdown()
        {
            throw new NotImplementedException();
        }

        public void Step()
        {
            UI.WindowBegin("Window", ref windowPose, new Vec2(20, 0) * U.cm, UIWin.Body);
            UI.WindowEnd();
        }


        public async Task<List<byte[]>> GetGifFramesAsync(string imageUrl)
        {
            using var httpClient = new HttpClient();
            using var response = await httpClient.GetAsync(imageUrl);

            var gifFramesArray = new List<Byte[]>();

            if (response.IsSuccessStatusCode)
            {
                using var stream = await response.Content.ReadAsStreamAsync();
                var framesCollection = Image.Load(stream).Frames;

                using (MemoryStream ms = new MemoryStream())
                {
                    for (int i = 0; i < framesCollection.Count; i++)
                    {
                        Image currentFrame = framesCollection.CloneFrame(currentFrameNumber);
                        currentFrame.Save(ms, new GifEncoder());
                        gifFramesArray.Add(ms.ToArray());
                    }
                }

                return gifFramesArray;

            }

            return null;
        }

    }
}
