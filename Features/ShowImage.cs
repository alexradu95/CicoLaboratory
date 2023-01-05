using StereoKit;
using StereoKit.Framework;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace CicoLaboratory.Features
{
    internal class ShowImage : IStepper
    {
        public bool Enabled => throw new NotImplementedException();
        Sprite sprite;
        byte[] imageBytes;
        Pose windowPose = new Pose(-.4f, 0, 0, Quat.LookDir(1, 0, 1));

        public bool Initialize() {

            imageBytes = GetImageAsByteArrayAsync("https://upload.wikimedia.org/wikipedia/commons/thumb/b/b6/Image_created_with_a_mobile_phone.png/1200px-Image_created_with_a_mobile_phone.png").Result;
            sprite = Sprite.FromTex(Tex.FromMemory(imageBytes));

            return true;
        }

        public void Shutdown()
        {
            throw new NotImplementedException();
        }

        public void Step()
        {
            UI.WindowBegin("Window", ref windowPose, new Vec2(20, 0) * U.cm, UIWin.Body);
            UI.Image(sprite, new Vec2(22, 0) * U.cm);
            UI.WindowEnd();
        }


        public async Task<byte[]> GetImageAsByteArrayAsync(string imageUrl)
        {
            byte[] imageBytes;

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(imageUrl))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        imageBytes = await response.Content.ReadAsByteArrayAsync();
                        return imageBytes;
                    }
                    else
                    {
                        // Handle unsuccessful response
                        return null;
                    }
                }
            }
        }
    }
}
