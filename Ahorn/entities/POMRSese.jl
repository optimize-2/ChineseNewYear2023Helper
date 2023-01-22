module ChineseNewYear2023HelperPOMRSese
using ..Ahorn, Maple

@mapdef Entity "ChineseNewYear2023Helper/POMRSese" POMRSese(x::Integer, y::Integer)

const placements = Ahorn.PlacementDict(
    "POMR Se se (Chinese New Year 2023 Helper, Path of Most Resistance)" => Ahorn.EntityPlacement(
        POMRSese
    )
)

end