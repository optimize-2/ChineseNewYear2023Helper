using Celeste.Mod.ChineseNewYear2023Helper.POMR.Entities.Triggers;
using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste.Mod.ChineseNewYear2023Helper.POMR.Entities.Placables {
    [Tracked]
    [CustomEntity("ChineseNewYear2023Helper/PlacableCrumbleBlock6")]
    public class PlacableCrumbleBlock6 : PlacableEntity {
        CrumblePlatform block;
        public PlacableCrumbleBlock6() : base() {
            base.Type = Placable.CRUMBLEBLOCK6;
			base.Collider = new Hitbox(48f, 8f, 0f, 0f);
        }

        public override void Recycle() {
            block.RemoveSelf();
            POMRController.List.ForEach(item => {
                if (item is CrumbleBlock6Item) {
                    item.Count += 1;
                    POMRController.Money += item.Cost;
                    this.RemoveSelf();
                    return;
                }
            });
        }

        public override void Spawn() {
        }

        public override void Initialize(Vector2 pos) {
            this.Position = pos;
        }

        public override void Added(Scene scene) {
            base.Added(scene);
            block = new CrumblePlatform(Position, 48f);
            base.SceneAs<Level>().Add(block);
        }

        public override void Removed(Scene scene) {
            scene.Remove(block);
            base.Removed(scene);
        }
    }

    public class CrumbleBlock6Item : PlacableItem {
        public CrumbleBlock6Item(int cost, int count) : base(cost, count) {
            base.Type = Placable.CRUMBLEBLOCK6;
            base.Texture = GFX.Gui["ChineseNewYear2023/optimize_2/items/crumbleblock6"];
        }

        public override bool Place(Vector2 pos) {
            PlacableCrumbleBlock6 entity = new PlacableCrumbleBlock6();
            Vector2 finalPos = round(pos);
            
            entity.Initialize(finalPos);
            if (POMRController.Level.CollideCheck<Solid>(finalPos)) {
                return false;
            } else {
                POMRController.Level.Add(entity);
                return true;
            }
        }

        public override Vector2 TryPlace(Vector2 pos) {
            Vector2 finalPos = round(pos);
            if (Place(finalPos)) {
                return finalPos;
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

        private Vector2 round(Vector2 pos) {
            float amp = 8f;
            return amp * Calc.Floor(pos / amp);
        }
    }
}