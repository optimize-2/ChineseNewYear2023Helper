using System.Collections.Generic;
using Celeste.Mod.ChineseNewYear2023Helper.POMR;
using Celeste.Mod.ChineseNewYear2023Helper.POMR.Entities.Renderer;
using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste.Mod.ChineseNewYear2023Helper.POMR.Entities.Placables {
    [Tracked(true)]
    [CustomEntity("ChineseNewYear2023Helper/PlacableEntity")]
    public abstract class PlacableEntity : Entity {
        public Sprite Sprite;
        
        public Placable Type;

        public PlacableEntity() : base(Vector2.Zero) {
        }

        public abstract void Recycle();

        public abstract void Spawn();

        public abstract void Initialize(Vector2 pos);
    }

    public abstract class PlacableItem {
        public int Cost, Count;
        public MTexture Texture;
        public string DialogName;

        public Placable Type = Placable.NONE;

        public PlacableItem(int cost, int count) {
            this.Cost = cost;
            this.Count = count;
        }

        public abstract bool Place(Vector2 pos);

        public void Buy(Vector2 pos) {
            if (Count == 0) {
                POMRRenderer.Message(Dialog.Get("ChineseNewYear2023Helper_OPTIMIZE2_BUY_FAILED_COUNT"));
                return;
            }
            if (POMRController.Money < Cost) {
                POMRRenderer.Message(Dialog.Get("ChineseNewYear2023Helper_OPTIMIZE2_BUY_FAILED_MONEY"));
                return;
            }
            Vector2 posPlaced = TryPlace(pos);
            if (posPlaced != Vector2.Zero) {
                POMRController.Money -= Cost;
                Count -= 1;
                POMRController.Placed.Add(new KeyValuePair<Vector2, Placable>(posPlaced, Type));
                POMRRenderer.Message("-" + Cost.ToString());
            }
        }

        public void Select() {
            POMRController.Selected = Type;
        }

        public abstract Vector2 TryPlace(Vector2 pos);
    }

    // public abstract class PlacableComponent : Component {
        
    // }

    public enum Placable {
        NONE,
        JELLYFISH, THEO,
        SPACEJAM,
        STRAWBERRY,
        SPRING_RIGHT,
        CRUMBLEBLOCK2, CRUMBLEBLOCK6,
        REFILL_RENEWABLE_SINGLEDASH, REFILL_ONEUSE_SINGLEDASH, REFILL_ONEUSE_DOUBLEDASH, REFILL_RENEWABLE_DOUBLEDASH,
        SEEKER,
        BUBBLE_GREEN, BUBBLE_RED
    }
}