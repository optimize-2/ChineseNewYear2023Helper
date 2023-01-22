using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste.Mod.ChineseNewYear2023Helper.POMR.Entities.Placables {
    [Tracked]
    [CustomEntity("ChineseNewYear2023Helper/PlacableRightSpring")]
    public class PlacableRightSpring : PlacableEntity {
        public PlacableRightSpring() : base() {
            base.Type = Placable.SPRING_RIGHT;
			base.Add(this.Sprite = new Sprite(GFX.Game, "objects/spring/"));
            base.Collider = new Hitbox(6f, 16f, 0f, -8f);
            this.Sprite.SetOrigin(8f, 16f);
            this.Sprite.Rotation = 1.5707964f;
			this.Sprite.Add("idle", "", 0f, new int[1]);
        }

        public override void Recycle() {
            POMRController.List.ForEach(item => {
                if (item is RightSpringItem) {
                    item.Count += 1;
                    POMRController.Money += item.Cost;
                    this.RemoveSelf();
                    return;
                }
            });
        }

        public override void Spawn() {
            Spring spring = new Spring(this.Position, Spring.Orientations.WallLeft, true);
            base.SceneAs<Level>().Add(spring);
        }

        public override void Initialize(Vector2 pos) {
            this.Position = pos;
			this.Sprite.Play("idle", false, false);
        }
    }

    public class RightSpringItem : PlacableItem {
        public RightSpringItem(int cost, int count) : base(cost, count) {
            base.Type = Placable.SPRING_RIGHT;
            base.Texture = GFX.Gui["ChineseNewYear2023/optimize_2/items/rightspring"];
        }

        public override bool Place(Vector2 pos) {
            PlacableEntity entity = new PlacableRightSpring();
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