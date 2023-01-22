using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste.Mod.ChineseNewYear2023Helper.POMR.Entities.Entities {
    [Tracked]
    [CustomEntity("ChineseNewYear2023Helper/POMRSese")]
    public class POMRSese : Entity {
        private static Random random = new Random();
        private MTexture texture;

        private void useLocalImage() {
            texture = GFX.Gui["ChineseNewYear2023/optimize_2/sese/" + random.Next(1, 3).ToString()];
        }

        public POMRSese(EntityData data, Vector2 offset) : base(data.Position + offset) {
            Tag |= TagsExt.SubHUD;
            this.Depth = 10000000;
            Task.Run(() => {
                HttpWebRequest request = HttpWebRequest.CreateHttp("https://iw233.cn/api.php?sort=random&type=json");
                HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                if (Convert.ToInt32(response.StatusCode) != 200) {
                    useLocalImage();
                } else {
                    try {
                        string responseContent = "";
                        StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                        responseContent = reader.ReadToEnd().ToString();
                        Regex r = new Regex(@"(https.+\.jpg)");
                        string imageUrl = r.Match(responseContent).Value.Replace("\\/", "/").Replace("/large/", "/middle/");
                        ChineseNewYear2023HelperModule.info("match: " + imageUrl);
                        
                        WebClient client = new WebClient();
                        string path = "Mods/Cache/ChineseNewYear2023Helper";
                        Directory.CreateDirectory(path);
                        client.DownloadFile(imageUrl, path + "/download.png");
                        texture?.Unload();
                        GC.Collect();
                        texture = new MTexture(VirtualContent.CreateTexture(Directory.GetCurrentDirectory() + "/Mods/Cache/ChineseNewYear2023Helper/download.png"));
                        
                    } catch (Exception e) {
                        ChineseNewYear2023HelperModule.info(e.Message + "\n" + e.StackTrace);
                        useLocalImage();
                    }
                }
            });
        }

        private static float getScale() {
            return POMRController.Level.Zoom * ((320f - POMRController.Level.ScreenPadding * 2f) / 320f);
        }

        public override void Render() {
            base.Render();
            if (texture == null) return;

            Level level = SceneAs<Level>();
            if (level == null) {
                return;
            }

            Vector2 textureSize = new(texture.Width, texture.Height);
            float textureScale = (180 / Math.Max(textureSize.X, textureSize.Y)) * getScale() * 5f;

            texture.DrawCentered(
                level.WorldToScreen(Position),
                Color.White * 0.8f,
                Vector2.One * textureScale
            );

        }
    }
}