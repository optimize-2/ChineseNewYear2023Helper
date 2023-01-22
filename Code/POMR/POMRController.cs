using System;
using System.Collections.Generic;
using Celeste.Mod.ChineseNewYear2023Helper.POMR.Entities.Placables;
using Celeste.Mod.ChineseNewYear2023Helper.POMR.Entities.Renderer;
using Celeste.Mod.ChineseNewYear2023Helper.POMR.Entities.Triggers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Monocle;

namespace Celeste.Mod.ChineseNewYear2023Helper.POMR {
    public class POMRController : GameComponent {
        public static Game Celeste;
        public static Level Level;
        public static Player Player;

        public static Vector2 LastPlay;
        public static HashSet<KeyValuePair<Vector2, Placable>> Placed = new HashSet<KeyValuePair<Vector2, Placable>>();

        public static List<PlacableItem> List = new List<PlacableItem>();

        public static int Money;

        public static Placable Selected;

        private static bool leftButtonReleased = true;

        public POMRController(Game game) : base(game) {
            POMRController.Celeste = game;
            Load();
        }
        
        public static void OnLoadLevel(Level level, Player.IntroTypes intro, bool fromLoader) {
            POMRController.Level = level;
            POMRController.Player = level.Tracker.GetEntity<Player>();
            if (Level.Tracker.GetEntities<POMRPlacementFeatureTrigger>().Count == 0) return;
            PlacableSeeker.SeekerIndex = 0;
            RefreshPlacedList();
            DisablePlacement();
            DisableGame();
            removePlacedEntities();
        }

        public static void RefreshPlacedList() {
            Vector2 triggerPos = Level.Tracker.GetEntity<POMRPlacementFeatureTrigger>().Position;
            if (triggerPos != LastPlay) {
                LastPlay = triggerPos;
                Placed.Clear();
            }
        }

        public void Load() {
            Everest.Events.Level.OnLoadLevel += OnLoadLevel;
        }
        
        public void Unload() {
            Everest.Events.Level.OnLoadLevel -= OnLoadLevel;
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);

            if (!POMRPlacementFeatureTrigger.Enabled) return;

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
            
            Level.Tracker.GetEntities<PlacableEntity>().ForEach(e => {
                if (((PlacableEntity) e).Sprite == null) return;
                ((PlacableEntity) e).Sprite.Color = Color.White * 0.5f;
            });

            Level.CollideAll<PlacableEntity>(mouseWorldPosition).ForEach(e => {
                if (((PlacableEntity) e).Sprite == null) return;
                ((PlacableEntity) e).Sprite.Color = Color.White * 0.8f;
            });

            if (mouseState.LeftButton == ButtonState.Pressed && leftButtonReleased) {
                leftButtonReleased = false;
                
                if (Input.Grab.Check) {
                    Level.CollideAll<PlacableEntity>(mouseWorldPosition).ForEach(e => {
                        foreach (KeyValuePair<Vector2, Placable> pair in Placed) {
                            if (pair.Key == e.Position && pair.Value == e.Type) {
                                Placed.Remove(pair);
                                break;
                            }
                        }
                        e.Recycle();
                    });
                } else {
                    List.ForEach(item => {
                        if (item.Type == Selected) {
                            item.Buy(mouseWorldPosition);
                        }
                    });
                }
            }

            if (mouseState.LeftButton == ButtonState.Released) {
                leftButtonReleased = true;
            }
            
        }

        public static void EnablePlacement() {
            if (POMRPlacementFeatureTrigger.InGame) return;
            // spawnPlacedEntities();
            DisableGame();
            Celeste.IsMouseVisible = true;
            POMRPlacementFeatureTrigger.Enabled = true;
            
            if (Level.Tracker.GetEntities<POMRRenderer>().Count == 0) {
                Level.Add(new POMRRenderer());
            }

            removePlacedEntities();
            spawnPlacedEntities();
        }

        public static void EnableGame() {
            DisablePlacement();
            POMRPlacementFeatureTrigger.InGame = true;
            Level.Tracker.GetEntities<PlacableEntity>().ForEach(e => { ((PlacableEntity) e).Spawn(); });

            Level.Tracker.GetEntities<PlacableEntity>().ForEach(e => {
                if (((PlacableEntity) e).Sprite == null) return;
                ((PlacableEntity) e).Sprite.Color = Color.White * 0.2f;
            });
        }

        public static void DisablePlacement() {
            POMRPlacementFeatureTrigger.Enabled = false;
            Celeste.IsMouseVisible = false;
        }

        public static void DisableGame() {
            POMRPlacementFeatureTrigger.InGame = false;
        }

        private static void removePlacedEntities() {
            Level.Tracker.GetEntities<PlacableEntity>().ForEach(e => { e.RemoveSelf(); });
        }

        private static void spawnPlacedEntities() {
            foreach (KeyValuePair<Vector2, Placable> pair in Placed) {
                foreach (PlacableItem item in List) {
                    if (pair.Value == item.Type) {
                        item.Place(pair.Key);
                    }
                }
            }
        }
    }
}