using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste.Mod.ChineseNewYear2023Helper.POMR.Entities.Placables {
    [Tracked]
    [CustomEntity("ChineseNewYear2023Helper/PlacableJellyFish")]
    public class PlacableJellyFish : PlacableEntity {
        public PlacableJellyFish() : base() {
            base.Type = Placable.JELLYFISH;
            base.Collider = new Hitbox(8f, 10f, -4f, -10f);
            base.Add(this.Sprite = GFX.SpriteBank.Create("glider"));
        }

        public override void Recycle() {
            POMRController.List.ForEach(item => {
                if (item is JellyFishItem) {
                    item.Count += 1;
                    POMRController.Money += item.Cost;
                    this.RemoveSelf();
                    return;
                }
            });
        }

        public override void Spawn() {
            Glider glider = new Glider(this.Position, true, false);
            base.SceneAs<Level>().Add(glider);
        }

        public override void Initialize(Vector2 pos) {
            this.Position = pos;
            this.Sprite.Play("idle", false, false);
        }
    }

    public class JellyFishItem : PlacableItem {
        public JellyFishItem(int cost, int count) : base(cost, count) {
            base.Type = Placable.JELLYFISH;
            base.Texture = GFX.Gui["ChineseNewYear2023/optimize_2/items/jellyfish"];
        }

        public override bool Place(Vector2 pos) {
            PlacableEntity entity = new PlacableJellyFish();
            entity.Initialize(pos);
            if (collideOrOffscreenCheck(POMRController.Level, entity)) {
                return false;
            } else {
                POMRController.Level.Add(entity);
                return true;
            }
        }

        public override Vector2 TryPlace(Vector2 pos) {
            if (Place(pos)) {
                return pos;
            } else {
                return Vector2.Zero;
            }
        }

        private bool collideOrOffscreenCheck(Level level, Entity entity) {
            return entity.Position.X + entity.Collider.Right > level.Bounds.Right
                || entity.Position.X + entity.Collider.Left < level.Bounds.Left
                || entity.Position.Y + entity.Collider.Top < level.Bounds.Top
                || Collide.Check(entity, level.Tracker.Entities[typeof(Solid)]);
        }
    }
}