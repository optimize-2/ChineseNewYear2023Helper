module ChineseNewYear2023HelperPOMRStrawberry
using ..Ahorn, Maple

@mapdef Entity "ChineseNewYear2023Helper/POMRStrawberry" POMRStrawberry(x::Integer, y::Integer)

const placements = Ahorn.PlacementDict(
    "POMR Strawberry (Chinese New Year 2023 Helper, Path of Most Resistance)" => Ahorn.EntityPlacement(
        POMRStrawberry
    )
)

const sprite = "collectables/strawberry/normal00"

function Ahorn.selection(entity::POMRStrawberry)
    x, y = Ahorn.position(entity)
    return Ahorn.getSpriteRectangle(sprite, x, y)
end

Ahorn.render(ctx::Ahorn.Cairo.CairoContext, entity::POMRStrawberry, room::Maple.Room) = Ahorn.drawSprite(ctx, sprite, 0, 0)

end