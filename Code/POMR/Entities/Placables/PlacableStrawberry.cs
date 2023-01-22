using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste.Mod.ChineseNewYear2023Helper.POMR.Entities.Placables {
    [Tracked]
    [RegisterStrawberry(true, false)]
    [CustomEntity("ChineseNewYear2023Helper/POMRStrawberry")]
    public class POMRStrawberry : Strawberry {
        public POMRStrawberry(EntityData data, Vector2 offset, EntityID gid) : base(data, offset, gid) {
            this.Visible = false;
        }

        public void Spawn(Vector2 pos) {
            this.Position = pos;
            this.Visible = true;
        }
    }

    
    [Tracked]
    [CustomEntity("ChineseNewYear2023Helper/PlacableStrawberry")]
    public class PlacableStrawberry : PlacableEntity {
        public PlacableStrawberry() : base() {
            base.Type = Placable.STRAWBERRY;
			base.Collider = new Hitbox(14f, 14f, -7f, -7f);
            base.Add(this.Sprite = GFX.SpriteBank.Create("strawberry"));
        }

        public override void Recycle() {
            POMRController.List.ForEach(item => {
                if (item is StrawberryItem) {
                    item.Count += 1;
                    POMRController.Money += item.Cost;
                    this.RemoveSelf();
                    return;
                }
            });
        }

        public override void Spawn() {
            base.SceneAs<Level>().Tracker.GetEntities<POMRStrawberry>().ForEach(berry => {
                if (!berry.CollideCheck<PlacableStrawberry>()) {
                    ((POMRStrawberry) berry).Spawn(this.Position);
                }
            });
            ChineseNewYear2023HelperModule.info("count: " + base.SceneAs<Level>().Tracker.GetEntities<POMRStrawberry>().Count);
        }

        public override void Initialize(Vector2 pos) {
            this.Position = pos;
            this.Sprite.Play("idle", false, false);
        }
    }

    public class StrawberryItem : PlacableItem {
        public StrawberryItem(int cost, int count) : base(cost, count) {
            base.Type = Placable.STRAWBERRY;
            base.Texture = GFX.Gui["ChineseNewYear2023/optimize_2/items/strawberry"];
        }

        public override bool Place(Vector2 pos) {
            PlacableEntity entity = new PlacableStrawberry();
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