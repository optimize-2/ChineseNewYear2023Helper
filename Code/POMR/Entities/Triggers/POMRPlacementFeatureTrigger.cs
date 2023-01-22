using System.Collections.Generic;
using Celeste.Mod.ChineseNewYear2023Helper.POMR.Entities.Placables;
using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste.Mod.ChineseNewYear2023Helper.POMR.Entities.Triggers {
    [Tracked]
    [CustomEntity("ChineseNewYear2023Helper/POMRPlacementFeatureTrigger")]
    class POMRPlacementFeatureTrigger : Trigger {
        public static bool Enabled, InGame;

        private static EntityData entityData;

        public POMRPlacementFeatureTrigger(EntityData data, Vector2 offset) : base(data, offset) {
            this.Depth = 10000;
            Enabled = false;
            entityData = data;
        }

        public override void Added(Scene scene) {
            base.Added(scene);
        }

        public override void OnEnter(Player player) {
            base.OnEnter(player);
            if (!Enabled) POMRController.EnablePlacement();

            POMRController.RefreshPlacedList();
            
            List<PlacableItem> list = new List<PlacableItem>();

            if (entityData.Int("buyJellyFishCount") != 0) {
                list.Add(new JellyFishItem(entityData.Int("buyJellyFishCost"), entityData.Int("buyJellyFishCount")));
            }
            if (entityData.Int("buyTheoCount") != 0) {
                list.Add(new TheoItem(entityData.Int("buyTheoCost"), entityData.Int("buyTheoCount")));
            }
            if (entityData.Int("buySpaceJamCount") != 0) {
                list.Add(new SpaceJamItem(entityData.Int("buySpaceJamCost"), entityData.Int("buySpaceJamCount")));
            }
            if (entityData.Int("buyStrawberryCount") != 0) {
                list.Add(new StrawberryItem(entityData.Int("buyStrawberryCost"), entityData.Int("buyStrawberryCount")));
            }
            if (entityData.Int("buyRightSpringCount") != 0) {
                list.Add(new RightSpringItem(entityData.Int("buyRightSpringCost"), entityData.Int("buyRightSpringCount")));
            }
            if (entityData.Int("buyCrumbleBlock2Count") != 0) {
                list.Add(new CrumbleBlock2Item(entityData.Int("buyCrumbleBlock2Cost"), entityData.Int("buyCrumbleBlock2Count")));
            }
            if (entityData.Int("buyCrumbleBlock6Count") != 0) {
                list.Add(new CrumbleBlock6Item(entityData.Int("buyCrumbleBlock6Cost"), entityData.Int("buyCrumbleBlock6Count")));
            }
            if (entityData.Int("buyRenewableSingleDashRefillCount") != 0) {
                list.Add(new RenewableSingleDashRefillItem(entityData.Int("buyRenewableSingleDashRefillCost"), entityData.Int("buyRenewableSingleDashRefillCount")));
            }
            if (entityData.Int("buyOneUseSingleDashRefillCount") != 0) {
                list.Add(new OneUseSingleDashRefillItem(entityData.Int("buyOneUseSingleDashRefillCost"), entityData.Int("buyOneUseSingleDashRefillCount")));
            }
            if (entityData.Int("buyRenewableDoubleDashRefillCount") != 0) {
                list.Add(new RenewableDoubleDashRefillItem(entityData.Int("buyRenewableDoubleDashRefillCost"), entityData.Int("buyRenewableDoubleDashRefillCount")));
            }
            if (entityData.Int("buyOneUseDoubleDashRefillCount") != 0) {
                list.Add(new OneUseDoubleDashRefillItem(entityData.Int("buyOneUseDoubleDashRefillCost"), entityData.Int("buyOneUseDoubleDashRefillCount")));
            }
            if (entityData.Int("buySeekerCount") != 0) {
                list.Add(new SeekerItem(entityData.Int("buySeekerCost"), entityData.Int("buySeekerCount")));
            }
            if (entityData.Int("buyGreenBubbleCount") != 0) {
                list.Add(new GreenBubbleItem(entityData.Int("buyGreenBubbleCost"), entityData.Int("buyGreenBubbleCount")));
            }
            if (entityData.Int("buyRedBubbleCount") != 0) {
                list.Add(new RedBubbleItem(entityData.Int("buyRedBubbleCost"), entityData.Int("buyRedBubbleCount")));
            }
            POMRController.Money = entityData.Int("money");
            
            // list.Add(new JellyFishItem(1, 100));
            // list.Add(new SpaceJamItem(1, 100));
            // list.Add(new StrawberryItem(1, 100));
            // list.Add(new RightSpringItem(1, 100));
            // list.Add(new CrumbleBlock2Item(1, 100));
            // list.Add(new CrumbleBlock6Item(1, 100));
            // list.Add(new RenewableSingleDashRefillItem(1, 100));
            // list.Add(new OneUseSingleDashRefillItem(1, 100));
            // list.Add(new RenewableDoubleDashRefillItem(1, 100));
            // list.Add(new OneUseDoubleDashRefillItem(1, 100));
            // list.Add(new SeekerItem(-20, 100));
            // list.Add(new GreenBubbleItem(1, 100));
            // list.Add(new RedBubbleItem(1, 100));
            // POMRController.Money = 1000;

            POMRController.List = list;

            foreach (KeyValuePair<Vector2, Placable> entity in POMRController.Placed) {
                foreach (PlacableItem item in POMRController.List) {
                    if (item.Type == entity.Value) {
                        POMRController.Money -= item.Cost;
                        item.Count -= 1;
                        break;
                    }
                }
            }
        }

        public override void OnStay(Player player) {
            base.OnStay(player);
        }

        public override void OnLeave(Player player) {
            base.OnLeave(player);
            if (!InGame) POMRController.EnableGame();
        }
        
        public static bool GetSpaceJamIntegralPoint() {
            return entityData.Bool("configSpaceJamIntegralPoint", true);
        }
    }
}