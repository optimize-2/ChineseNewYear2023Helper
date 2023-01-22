module ChineseNewYear2023HelperPOMRKillIfNotPlacementModeTrigger
using ..Ahorn, Maple

@mapdef Trigger "ChineseNewYear2023Helper/POMRKillIfNotPlacementModeTrigger" POMRKillIfNotPlacementModeTrigger(
    x::Integer, y::Integer, width::Integer=Maple.defaultTriggerWidth, height::Integer=Maple.defaultTriggerHeight
)

const placements = Ahorn.PlacementDict(
    "POMR Kill If Not Placement Mode Trigger (Chinese New Year 2023 Helper, Path of Most Resistance)" => Ahorn.EntityPlacement(
        POMRKillIfNotPlacementModeTrigger,
        "rectangle",
    )
)

end