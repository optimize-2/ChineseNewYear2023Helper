using System;
using System.Collections.Generic;
using Celeste.Mod.ChineseNewYear2023Helper.POMR.Entities.Placables;
using Celeste.Mod.ChineseNewYear2023Helper.POMR.Entities.Triggers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Monocle;

namespace Celeste.Mod.ChineseNewYear2023Helper.POMR.Entities.Renderer {
    [Tracked]
    public class POMRRenderer : Entity {
        public POMRRenderer() : base(Vector2.Zero) {
            Tag |= TagsExt.SubHUD;
            Tag |= Tags.FrozenUpdate;
            this.Depth = -10000000;
        }

        private static string message;
        private static float messageDisappearTime;

        public Vector2 Tracking = Vector2.Zero;

        private static MTexture[] numbers;

        public static SoundSource soundSource;

        public float Alpha = 1f;
        protected float time = 0f;
        protected bool popupShown = false;
        protected float popupTime = 100f;
        protected bool timeRateSet = false;

        public HashSet<string> TimeRateSkip = new();
        public bool ForceSetTimeRate;

        public float Angle = 0f;
        
        public int Selected = -1;
        protected int PrevSelected;
        protected float selectedTime = 0f;

        public MTexture BG = GFX.Gui["ChineseNewYear2023/optimize_2/bg"];
        public MTexture Line = GFX.Gui["ChineseNewYear2023/optimize_2/line"];
        public MTexture Indicator = GFX.Gui["ChineseNewYear2023/optimize_2/indicator"];

        public Color TextSelectColorA = Calc.HexToColor("84FF54");
        public Color TextSelectColorB = Calc.HexToColor("FCFF59");

        private static float getScale() {
            return POMRController.Level.Zoom * ((320f - POMRController.Level.ScreenPadding * 2f) / 320f);
        }

        public override void Render() {
            base.Render();

            if (!POMRPlacementFeatureTrigger.Enabled && !POMRPlacementFeatureTrigger.InGame) return;

            if (messageDisappearTime > 0) messageDisappearTime -= Engine.DeltaTime;

            float H = Engine.Graphics.PreferredBackBufferHeight;
            float W = Engine.Graphics.PreferredBackBufferWidth;
            float scale = Calc.Min(H / 9f, W / 16f);
            float scaledH = scale * 9;
            float scaledW = scale * 16;
            float deltaH = H / 2 - scaledH / 2;
            float deltaW = W / 2 - scaledW / 2;
            MouseState mouseState = MInput.Mouse.CurrentState;
            Vector2 mousePosition = new Vector2(mouseState.X, mouseState.Y);
            mousePosition.X -= deltaW;
            mousePosition.Y -= deltaH;
            float viewScale = (float) Engine.ViewWidth / Engine.Width;
            Vector2 mouseWorldPosition = Calc.Floor(POMRController.Level.ScreenToWorld(mousePosition / viewScale));
            Camera cam = POMRController.Level.Camera;

            Player player = Scene.Tracker.GetEntity<Player>();
            if (player != null) {
                string content = Dialog.Get("ChineseNewYear2023Helper_OPTIMIZE2_REMAINING_MONEY") + ": " + POMRController.Money.ToString();
                if (POMRPlacementFeatureTrigger.InGame) content += " (In game)";
                else if (messageDisappearTime > 0) content += $" ({ message })";
                int totalWidth = content.Length * 4 - 1;
                Color outlineColor = Color.Black;
                float yoffset = cam.Top - cam.Bottom;
                Vector2 size = ActiveFont.Measure(content) * getScale();
                Vector2 basePosition = new Vector2(Engine.Width / 2, Engine.Height);
                ActiveFont.DrawOutline(
                    content,
                    basePosition,
                    new Vector2(0.5f, 1f),
                    Vector2.One * getScale(),
                    Color.White, 2f,
                    outlineColor
                );
            }

            if (!POMRPlacementFeatureTrigger.Enabled) return;
            
            bool shown = (mouseState.RightButton == ButtonState.Pressed);
            Vector2 yaw = (mouseWorldPosition - Tracking).SafeNormalize();
            Angle = yaw.Angle();
            
            if (shown) {
                float start = (-0.5f / POMRController.List.Count) * 2f * (float) Math.PI;
                if (2f * (float) Math.PI + start < Angle) {
                    Angle -= 2f * (float) Math.PI;
                }
                for (int i = 0; i < POMRController.List.Count; i++) {
                    float min = ((i - 0.5f) / POMRController.List.Count) * 2f * (float) Math.PI;
                    float max = ((i + 0.5f) / POMRController.List.Count) * 2f * (float) Math.PI;
                    if ((min <= Angle && Angle <= max) ||
                        (min <= (Angle + 2f * (float) Math.PI) && (Angle + 2f * (float) Math.PI) <= max) ||
                        (min <= (Angle - 2f * (float) Math.PI) && (Angle - 2f * (float) Math.PI) <= max)) {
                        Selected = i;
                        break;
                    }
                }
            } else {
                Tracking = mouseWorldPosition;
                if (Selected == -1) return;
                POMRController.List[Selected].Select();
                Selected = -1;
                return;
            }
            time += Engine.RawDeltaTime;

            selectedTime += Engine.RawDeltaTime;
            if (PrevSelected != Selected) {
                selectedTime = 0f;
                PrevSelected = Selected;
            }

            float popupAlpha;
            float popupScale;

            popupTime += Engine.RawDeltaTime;
            if (shown && !popupShown) {
                popupTime = 0f;
            } else if ((shown && popupTime > 1f) ||
                (!shown && popupTime < 1f)) {
                popupTime = 1f;
            }
            popupShown = shown;

            if (popupTime < 0.1f) {
                float t = popupTime / 0.1f;
                // Pop in.
                popupAlpha = Ease.CubeOut(t);
                popupScale = Ease.ElasticOut(t);

            } else if (popupTime < 1f) {
                // Stay.
                popupAlpha = 1f;
                popupScale = 1f;

            } else {
                float t = (popupTime - 1f) / 0.2f;
                // Fade out.
                popupAlpha = 1f - Ease.CubeIn(t);
                popupScale = 1f - 0.2f * Ease.CubeIn(t);
            }

            float alpha = Alpha * popupAlpha;
            if (Tracking == null) {
                return;
            }

            Level level = SceneAs<Level>();
            if (level == null) {
                return;
            }

            popupScale *= getScale() * 2f;

            Vector2 pos = Tracking;
            pos.Y -= 8f;

            pos = level.WorldToScreen(pos);

            float radius = BG.Width * 0.5f * 0.75f * popupScale;

            pos = pos.Clamp(
                0f + radius, 0f + radius,
                1920f - radius, 1080f - radius
            );

            BG.DrawCentered(
                pos,
                Color.White * alpha * alpha * alpha,
                Vector2.One * popupScale
            );

            Indicator.DrawCentered(
                pos,
                Color.White * alpha * alpha * alpha,
                Vector2.One * popupScale,
                Angle
            );

            float selectedScale = 1.2f + 0.4f * Calc.Clamp(Ease.CubeOut(selectedTime / 0.1f), 0f, 1f) + (float) Math.Sin(time * 1.8f) * 0.1f;

            for (int i = 0; i < POMRController.List.Count; i++) {
                Line.DrawCentered(
                    pos,
                    Color.White * alpha * alpha * alpha,
                    Vector2.One * popupScale,
                    ((i + 0.5f) / POMRController.List.Count) * 2f * (float) Math.PI
                );

                PlacableItem item = POMRController.List[i];

                float a = (i / (float) POMRController.List.Count) * 2f * (float) Math.PI;
                Vector2 itemPos = pos + new Vector2(
                    (float) Math.Cos(a),
                    (float) Math.Sin(a)
                ) * radius;

                MTexture icon = item.Texture;
                if (icon == null)
                    continue;

                Vector2 iconSize = new(icon.Width, icon.Height);
                float iconScale = (180 / Math.Max(iconSize.X, iconSize.Y)) * 0.24f * popupScale;

                icon.DrawCentered(
                    itemPos,
                    Color.White * (Selected == i ? (Calc.BetweenInterval(selectedTime, 0.1f) ? 0.9f : 1f) : 0.7f) * alpha,
                    Vector2.One * (Selected == i ? selectedScale : 1f) * iconScale
                );

                string itemText;
                itemText = item.Cost.ToString() + " - " + item.Count.ToString();

                Vector2 textSize = ActiveFont.Measure(itemText);
                float textScale = (180 / Math.Max(textSize.X, textSize.Y)) * 0.24f * popupScale;

                ActiveFont.DrawOutline(
                    itemText,
                    itemPos + new Vector2(0, 10f),
                    new(0.5f, 0.5f),
                    Vector2.One * (Selected == i ? selectedScale : 1f) * textScale,
                    (Selected == i ? (Calc.BetweenInterval(selectedTime, 0.1f) ? TextSelectColorA : TextSelectColorB) : Color.LightSlateGray) * alpha,
                    2f,
                    Color.Black * alpha * alpha * alpha
                );
            }
        }

        public static void Message(string msg) {
            message = msg;
            messageDisappearTime = 1f;
        } 
    }
}