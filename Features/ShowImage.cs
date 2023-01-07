using StereoKit;
using StereoKit.Framework;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using ImageMagick;
using System.Linq;

namespace CicoLaboratory.Features
{
    internal class ShowImage : IStepper
    {
        public bool Enabled => true;
        List<Tex> gifFramesTextures = new();

        Pose windowPose = new Pose(-.4f, 0, 0, Quat.LookDir(1, 0, 1));
        int delayTime = 0;
        int currentFrameNumber = 0;

        public bool Initialize()
        {

            var loadImage = RetrieveImageFromWeb("https://i0.wp.com/www.printmag.com/wp-content/uploads/2021/02/4cbe8d_f1ed2800a49649848102c68fc5a66e53mv2.gif?fit=476%2C280&ssl=1").Result;
            ExtractGifsFrame(loadImage);

            return true;
        }

        private void ExtractGifsFrame(byte[] loadImage)
        {

            List<byte[]> frames = new List<byte[]>();

            using (var image = new MagickImageCollection())
            {
                image.Read(loadImage);
                gifFramesTextures = image.Select(imageFrame =>
                {

                    if (delayTime == 0)
                    {
                        delayTime = imageFrame.AnimationDelay;
                    }
                    using (var frameStream = new MemoryStream())
                    {
                        imageFrame.Format = MagickFormat.Png;
                        imageFrame.Write(frameStream);
                        return frameStream.ToArray();
                    }
                }).Select(byteArray => Tex.FromMemory(byteArray)).ToList();
            }
        }

        public void Shutdown()
        {
            //donothing
        }

        public void Step()
        {
            UI.WindowBegin("Window", ref windowPose, new Vec2(20, 0) * U.cm, UIWin.Normal);


            if (gifFramesTextures.Count > 0)
            {
                GoToNextFrame();

                UI.Image(Sprite.FromTex(gifFramesTextures[currentFrameNumber]), new Vec2(20, 0) * U.cm);
            }


            UI.WindowEnd();
        }

        private void GoToNextFrame()
        {
            var timeTotal = Time.Total * 100;
            var calc = timeTotal % delayTime;
            if ((int) calc == 1 || (int) calc == 0) 
            {
                if (currentFrameNumber == gifFramesTextures.Count - 1)
                {
                    currentFrameNumber = 0;
                }
                else
                {
                    currentFrameNumber++;
                }
            }
        }

        private async Task<byte[]> RetrieveImageFromWeb(string imageUrl)
        {
            using var httpClient = new HttpClient();
            using var response = await httpClient.GetAsync(imageUrl);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsByteArrayAsync();
            }

            return null; //should return a placeholder
        }

    }
}
