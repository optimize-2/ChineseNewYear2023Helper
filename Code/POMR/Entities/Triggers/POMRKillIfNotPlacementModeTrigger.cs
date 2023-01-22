using System.Collections.Generic;
using Celeste.Mod.ChineseNewYear2023Helper.POMR.Entities.Placables;
using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste.Mod.ChineseNewYear2023Helper.POMR.Entities.Triggers {
    [Tracked]
    [CustomEntity("ChineseNewYear2023Helper/POMRKillIfNotPlacementModeTrigger")]
    class POMRKillIfNotPlacementModeTrigger : Trigger {

        public POMRKillIfNotPlacementModeTrigger(EntityData data, Vector2 offset) : base(data, offset) {
            this.Depth = 10000;
        }

        public override void Added(Scene scene) {
            base.Added(scene);
        }

        public override void OnEnter(Player player) {
            base.OnEnter(player);
            if (!POMRPlacementFeatureTrigger.InGame) {
                player.Die(Vector2.Zero);
            }
        }

        public override void OnStay(Player player) {
            base.OnStay(player);
        }

        public override void OnLeave(Player player) {
            base.OnLeave(player);
        }
    }
}