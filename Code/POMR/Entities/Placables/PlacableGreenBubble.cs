using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste.Mod.ChineseNewYear2023Helper.POMR.Entities.Placables {
    [Tracked]
    [CustomEntity("ChineseNewYear2023Helper/PlacableGreenBubble")]
    public class PlacableGreenBubble : PlacableEntity {
        public PlacableGreenBubble() : base() {
            base.Type = Placable.BUBBLE_GREEN;
			base.Collider = new Circle(10f, 0f, 2f);
            base.Add(this.Sprite = GFX.SpriteBank.Create("booster"));
        }

        public override void Recycle() {
            POMRController.List.ForEach(item => {
                if (item is GreenBubbleItem) {
                    item.Count += 1;
                    POMRController.Money += item.Cost;
                    this.RemoveSelf();
                    return;
                }
            });
        }

        public override void Spawn() {
            Booster bubble = new Booster(this.Position, false);
            base.SceneAs<Level>().Add(bubble);
        }

        public override void Initialize(Vector2 pos) {
            this.Position = pos;
            this.Sprite.Play("loop", false, false);
        }
    }

    public class GreenBubbleItem : PlacableItem {
        public GreenBubbleItem(int cost, int count) : base(cost, count) {
            base.Type = Placable.BUBBLE_GREEN;
            base.Texture = GFX.Gui["ChineseNewYear2023/optimize_2/items/greenbubble"];
        }

        public override bool Place(Vector2 pos) {
            PlacableEntity entity = new PlacableGreenBubble();
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