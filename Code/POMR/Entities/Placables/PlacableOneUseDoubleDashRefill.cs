using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste.Mod.ChineseNewYear2023Helper.POMR.Entities.Placables {
    [Tracked]
    [CustomEntity("ChineseNewYear2023Helper/PlacableOneUseDoubleDashRefill")]
    public class PlacableOneUseDoubleDashRefill : PlacableEntity {
        public PlacableOneUseDoubleDashRefill() : base() {
            base.Type = Placable.REFILL_ONEUSE_DOUBLEDASH;
			base.Collider = new Hitbox(16f, 16f, -8f, -8f);
			base.Add(this.Sprite = new Sprite(GFX.Game, "objects/refillTwo/idle"));
        }

        public override void Recycle() {
            POMRController.List.ForEach(item => {
                if (item is OneUseDoubleDashRefillItem) {
                    item.Count += 1;
                    POMRController.Money += item.Cost;
                    this.RemoveSelf();
                    return;
                }
            });
        }

        public override void Spawn() {
            base.SceneAs<Level>().Add(new Refill(Position, true, true));
        }

        public override void Initialize(Vector2 pos) {
            this.Position = pos;
			this.Sprite.AddLoop("idle", "", 0.1f);
			this.Sprite.Play("idle", false, false);
			this.Sprite.CenterOrigin();
        }
    }

    public class OneUseDoubleDashRefillItem : PlacableItem {
        public OneUseDoubleDashRefillItem(int cost, int count) : base(cost, count) {
            base.Type = Placable.REFILL_ONEUSE_DOUBLEDASH;
            base.Texture = GFX.Gui["ChineseNewYear2023/optimize_2/items/oneusedoubledashrefill"];
        }

        public override bool Place(Vector2 pos) {
            PlacableEntity entity = new PlacableOneUseDoubleDashRefill();
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