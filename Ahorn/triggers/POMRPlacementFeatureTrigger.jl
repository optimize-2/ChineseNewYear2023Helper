module ChineseNewYear2023HelperPOMRPlacementFeatureTrigger
using ..Ahorn, Maple

@mapdef Trigger "ChineseNewYear2023Helper/POMRPlacementFeatureTrigger" POMRPlacementFeatureTrigger(
    x::Integer, y::Integer, width::Integer=Maple.defaultTriggerWidth, height::Integer=Maple.defaultTriggerHeight,
    money::Integer=0,
    buyJellyFishCount::Integer=0, buyJellyFishCost::Integer=0,
    buyTheoCount::Integer=0, buyTheoCost::Integer=0,
    buySpaceJamCount::Integer=0, buySpaceJamCost::Integer=0,
    buyStrawberryCount::Integer=0, buyStrawberryCost::Integer=0,
    buyRightSpringCount::Integer=0, buyRightSpringCost::Integer=0,
    buyCrumbleBlock2Count::Integer=0, buyCrumbleBlock2Cost::Integer=0,
    buyCrumbleBlock6Count::Integer=0, buyCrumbleBlock6Cost::Integer=0,
    buyRenewableSingleDashRefillCount::Integer=0, buyRenewableSingleDashRefillCost::Integer=0,
    buyOneUseSingleDashRefillCount::Integer=0, buyOneUseSingleDashRefillCost::Integer=0,
    buyRenewableDoubleDashRefillCount::Integer=0, buyRenewableDoubleDashRefillCost::Integer=0,
    buyOneUseDoubleDashRefillCount::Integer=0, buyOneUseDoubleDashRefillCost::Integer=0,
    buySeekerCount::Integer=0, buySeekerCost::Integer=0,
    buyGreenBubbleCount::Integer=0, buyGreenBubbleCost::Integer=0,
    buyRedBubbleCount::Integer=0, buyRedBubbleCost::Integer=0,
    configSpaceJamIntegralPoint::Bool=true
)

const placements = Ahorn.PlacementDict(
    "POMR Placement Feature Trigger (Chinese New Year 2023 Helper, Path of Most Resistance)" => Ahorn.EntityPlacement(
        POMRPlacementFeatureTrigger,
        "rectangle",
    )
)

end