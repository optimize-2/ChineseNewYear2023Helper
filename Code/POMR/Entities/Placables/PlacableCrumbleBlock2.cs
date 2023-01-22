using Celeste.Mod.ChineseNewYear2023Helper.POMR.Entities.Triggers;
using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste.Mod.ChineseNewYear2023Helper.POMR.Entities.Placables {
    [Tracked]
    [CustomEntity("ChineseNewYear2023Helper/PlacableCrumbleBlock2")]
    public class PlacableCrumbleBlock2 : PlacableEntity {
        CrumblePlatform block;
        public PlacableCrumbleBlock2() : base() {
            base.Type = Placable.CRUMBLEBLOCK2;
			base.Collider = new Hitbox(16f, 8f, 0f, 0f);
        }

        public override void Recycle() {
            block.RemoveSelf();
            POMRController.List.ForEach(item => {
                if (item is CrumbleBlock2Item) {
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
            block = new CrumblePlatform(Position, 16f);
            base.SceneAs<Level>().Add(block);
        }

        public override void Removed(Scene scene) {
            ChineseNewYear2023HelperModule.info("removed");
            scene.Remove(block);
            // block?.RemoveSelf();
            base.Removed(scene);
        }
    }

    public class CrumbleBlock2Item : PlacableItem {
        public CrumbleBlock2Item(int cost, int count) : base(cost, count) {
            base.Type = Placable.CRUMBLEBLOCK2;
            base.Texture = GFX.Gui["ChineseNewYear2023/optimize_2/items/crumbleblock2"];
        }

        public override bool Place(Vector2 pos) {
            PlacableCrumbleBlock2 entity = new PlacableCrumbleBlock2();
            Vector2 finalPos = round(pos);
            
            entity.Initialize(finalPos);
            if (POMRController.Level.CollideCheck<Solid>(finalPos)) {
                // entity.RemoveBlock();
                ChineseNewYear2023HelperModule.info("failed");
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