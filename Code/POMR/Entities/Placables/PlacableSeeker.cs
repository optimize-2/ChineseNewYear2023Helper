using System;
using System.Collections.Generic;
using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste.Mod.ChineseNewYear2023Helper.POMR.Entities.Placables {

    [Tracked]
    [CustomEntity("ChineseNewYear2023Helper/PlacableSeeker")]
    public class PlacableSeeker : PlacableEntity {
        public static int SeekerIndex = 0;

        public PlacableSeeker() : base() {
            base.Type = Placable.SEEKER;
			base.Add(this.Sprite = GFX.SpriteBank.Create("seeker"));
			base.Collider = new Hitbox(6f, 6f, -3f, -3f);
        }

        public override void Recycle() {
            POMRController.List.ForEach(item => {
                if (item is SeekerItem) {
                    if (POMRController.Money + item.Cost < 0) return;
                    item.Count += 1;
                    POMRController.Money += item.Cost;
                    this.RemoveSelf();
                    return;
                }
            });
        }

        public override void Spawn() {
            EntityData seekerData = generateBasicEntityData(base.SceneAs<Level>(), 10 + SeekerIndex);
            seekerData.Position = this.Position;
            Seeker seeker = new Seeker(seekerData, Vector2.Zero);
            base.SceneAs<Level>().Add(seeker);
        }
        private static EntityData generateBasicEntityData(Level level, int entityNumber) {
            EntityData entityData = new EntityData();
            int roomHash = Math.Abs(level.Session.Level.GetHashCode()) % 50_000_000;
            entityData.ID = 1_000_000_000 + roomHash * 20 + entityNumber;
            entityData.Level = level.Session.LevelData;
            entityData.Values = new Dictionary<string, object>();
            return entityData;
        }

        public override void Initialize(Vector2 pos) {
            this.Position = pos;
			this.Sprite.Play("idle", false, false);
        }
    }

    public class SeekerItem : PlacableItem {
        public SeekerItem(int cost, int count) : base(cost, count) {
            base.Type = Placable.SEEKER;
            base.Texture = GFX.Gui["ChineseNewYear2023/optimize_2/items/seeker"];
        }

        public override bool Place(Vector2 pos) {
            PlacableEntity entity = new PlacableSeeker();
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
                || Collide.Check(entity, level.Tracker.Entities[typeof(Solid)])
                || Collide.Check(entity, level.Tracker.Entities[typeof(PlacableSeeker)]);
        }
    }
}