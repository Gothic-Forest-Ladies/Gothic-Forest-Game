# How to edit the level with the Tile Palette

The level in `Main.unity` is painted on two tilemaps under the **Grid** object,
using the **ForestPalette** (`Assets/Tiles/ForestPalette.prefab`).

## Setup (once)

1. Open the scene `Assets/Scenes/Main.unity`.
2. Open the palette window: **Window > 2D > Tile Palette**.
3. In the dropdown at the top of that window, pick **ForestPalette**.

## Painting

1. In the **Active Tilemap** dropdown (inside the Tile Palette window), choose where to paint:
   - **Tilemap_Ground** — solid terrain the player stands on.
   - **Tilemap_Hazards** — the spikes.
2. Click a tile in the palette, then paint in the Scene view.

Tools (top of the palette window, with keyboard shortcuts):

| Tool | Key | Use |
|---|---|---|
| Brush | B | Paint single tiles |
| Eraser | D | Remove tiles (or paint with Shift held) |
| Box fill | U | Drag a rectangle of tiles |
| Picker | I | Click an existing tile in the scene to copy it |
| Fill | G | Flood-fill an area |

## The tiles

- `terrain_grass_block_*` — 9-piece grass terrain set (top/center/bottom × left/middle/right) plus a standalone block. Use the edge pieces so platforms look finished.
- `spikes` — paint these **only on Tilemap_Hazards**.

## Rules that keep gameplay working

- **Tilemap_Ground** must stay on the **Ground layer** — the player's jump ground-check looks for that layer.
- **Tilemap_Hazards** has its own TilemapCollider2D; when you wire the hazard/damage script, set that collider to **Is Trigger** so the player passes into the spikes instead of standing on them.
- Colliders update automatically as you paint — no extra work needed.

## Adding a new tile type

1. Drop the sprite into the project (e.g. under `Assets/Tiles/`).
2. Drag the sprite into the Tile Palette window while ForestPalette is selected — Unity asks where to save the generated Tile asset (choose `Assets/Tiles/`).
3. Paint with it like any other tile.
