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
        ImageFrameCollection framesCollection;
        Sprite logoSprite;
        int currentFrameNumber = 0;

        public bool Initialize() {

            framesCollection = GetGifFramesAsync("https://upload.wikimedia.org/wikipedia/commons/2/2c/Rotating_earth_%28large%29.gif").Result;

            return true;
        }

        public void Shutdown()
        {
            throw new NotImplementedException();
        }

        public void Step()
        {
            UI.WindowBegin("Window", ref windowPose, new Vec2(20, 0) * U.cm, UIWin.Body);
            AnimateGif();
            UI.WindowEnd();
        }

        private void AnimateGif()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                if(logoSprite == null) {
                    logoSprite = Sprite.FromTex(Tex.FromMemory(ms.GetBuffer()));
                }

                Image currentFrame = framesCollection.CloneFrame(currentFrameNumber);
                currentFrame.Save(ms, new GifEncoder());
                logoSprite.Draw(Matrix.Identity, Color32.BlackTransparent);
                
            }

            if (currentFrameNumber == framesCollection.Count - 1)
            {
                currentFrameNumber = 0;
            } else
            {
                currentFrameNumber++;
            }
        }

        public async Task<ImageFrameCollection> GetGifFramesAsync(string imageUrl)
        {
            using var httpClient = new HttpClient();
            using var response = await httpClient.GetAsync(imageUrl);

            if (response.IsSuccessStatusCode)
            {
                using var stream = await response.Content.ReadAsStreamAsync();
                return Image.Load(stream).Frames;
            }
            else
            {
                // Handle unsuccessful response
                return null;
            }
        }

    }
}
