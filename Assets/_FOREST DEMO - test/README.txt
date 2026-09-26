FOREST PLATFORMER DEMO - curated from Kenney New Platformer Pack (CC0)

CHARACTERS (switch with 1/2/3)
1 Base   -> Characters/Base_green   (idle/walk_a+b/jump/duck/climb/hit frames)
2 Bird   -> Characters/Bird         (fly_a/fly_b = wing flap; bee_* = alternative)
3 Small  -> Characters/Small_purple (same rig - import at 50% scale for tiny hitbox)

LEVEL
Tiles/Terrain     -> terrain_grass_* full autotile set (tops, corners, ramps, clouds=floating platforms)
Tiles/Hazards     -> spikes, block_spikes, water (Base+Small die, Bird flies over)
Tiles/Interactive -> spring+spring_out, key_*+lock_*+hud_key_* (fetch quests),
                     door_closed/open (+_top = doors are 2 tiles tall), ladder, flag_green (checkpoint/goal), signs
Tiles/Pickups     -> coins, gems, heart/star, hud_heart_* for the HUD
Tiles/Decor       -> bush, mushrooms, rock, fence, bridge (forest dressing)

BACKGROUNDS
background_color_trees = forest parallax layer; fade_trees behind it; solid_grass as base fill

SOUNDS: jump, jump-high (spring), coin, gem, hurt, bump, disappear, magic (character switch), select

Sprites are 64x64 (Default size). XML atlases exist in the pack's Spritesheets folder if you prefer one texture.
License: CC0 (see License.txt) - no credit required.
