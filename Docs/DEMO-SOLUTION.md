# Forest Demo — Reference Solution (how it was wired)

This documents the demo implementation that used to live in the `Main` scene.
The code and wiring were removed on purpose so it can be rebuilt as an exercise —
the scene (tiles, characters, goal, heart, spawn point, UI texts) is untouched,
and only `IDamagable.cs` and `PlayableCharacter.cs` remain in `Assets/Scripts`.
Use this as a reference if you get stuck or want to compare approaches.

## What the demo did

- Two-three playable characters (Base, Small, Bird) standing in the scene; keys **1/2/3** switched between them, shapeshift-style (the incoming character appears where the outgoing one stood; only the active one is visible).
- A/D or arrows to move, Space to jump; the Bird could fly (hold Space to rise, slow glide down).
- Spikes (the `Tilemap_Hazards` tilemap) damaged the player via `IDamagable`, which respawned them at `SpawnPoint`.
- The heart in the Small-only cave was a collectible; the `Goal` sprite ended the level and showed `Canvas/CompleteText`.
- Camera smoothly followed whichever character was active.

## Script architecture

Everything derived from the team's existing base:

```
IDamagable (interface: ApplyDamage(int), Die())
    └── PlayableCharacter (abstract MonoBehaviour: speed, maxHp, currentHp; Die() placeholder)
            └── PlayerController2D (the demo's movement controller)
```

### PlayerController2D (one per character, ~130 lines)
- `RequireComponent(Rigidbody2D)`; cached `Rigidbody2D`, `Collider2D`, `SpriteRenderer`, `AudioSource` in `Awake()`.
- Read input in `Update()` (new Input System, `Keyboard.current`): A/D/arrows → `moveInput`, Space → jump pressed/held. Flipped `spriteRenderer.flipX` by direction.
- Applied movement in `FixedUpdate()`: `rb.linearVelocity = new Vector2(moveInput * stats.moveSpeed, rb.linearVelocity.y)`; jump only when a small `Physics2D.OverlapCircle` at the collider's feet hit the ground layer.
- Flight (Bird only, `stats.canFly`): holding Space added `flyAcceleration * dt` upward, then clamped vertical speed between `maxFallSpeed` (slow glide) and `jumpForce` (max rise).
- `SetControlled(bool)` — called by the switcher; inactive characters became **kinematic** with zero velocity so they froze in place and ignored input.
- `Die()` override — teleport to `respawnPoint`, zero velocity, reset HP. Base class `ApplyDamage` called it when HP hit 0.
- Gotcha: the base `PlayableCharacter` has a private `Start()` that sets `currentHp = maxHp`. Don't declare `Start()` in the subclass — Unity only calls the most-derived one, silently skipping HP init.

### CharacterStats (ScriptableObject)
Per-character tuning data, assigned to each controller's `stats` field, created via `Assets > Create > GothicForest > Character Stats`. Values used:

| Asset | moveSpeed | jumpForce | gravityScale | canFly | flyAcceleration | maxFallSpeed |
|---|---|---|---|---|---|---|
| BaseStats | 6 | 11 | 3 | no | – | – |
| SmallStats | 5 | 6.6 | 3 | no | – | – |
| BirdStats | 5 | 6 | 0.5 | yes | 25 | -2 |

(Small jumps lower on purpose — it fits through the 2-tile cave but can't clear the spike pit; the Bird flies over instead.)

### CharacterSwitcher (on `GameManager`)
- Serialized `List<PlayerController2D>` holding Base, Small, Bird (in key order).
- `Update()` polled `Keyboard.current` digits 1..N; on switch: move incoming to outgoing's position, `SetControlled(false)` + `SetActive(false)` the old one, activate + `SetControlled(true)` the new one.
- `Start()` activated only index 0.

### CameraFollow (on `Main Camera`)
`LateUpdate()` + `Vector3.SmoothDamp` toward `switcher.Active.transform.position + offset` (offset `(0, 1.5, -10)`, smoothTime 0.2). No Cinemachine.

### Hazard (on `Grid/Tilemap_Hazards`)
Trigger collider; `OnTriggerEnter2D` did `other.GetComponent<IDamagable>()?.ApplyDamage(damage)` with damage 999 (instant kill → respawn). This is the piece that exercised the IDamagable interface.

### Collectible (on `Heart (cave reward)`)
Trigger; if the toucher had a `PlayerController2D`, log + play pickup sound + `Destroy(gameObject)`.

### GoalMarker (on `Goal`)
Trigger; first player touch logged "Demo complete!" and `SetActive(true)` on `Canvas/CompleteText`.

## Scene wiring checklist (to rebuild)

1. `Characters/Base`, `Small`, `Bird`: add controller script, assign its stats asset, `groundMask` = Ground layer, `respawnPoint` = `SpawnPoint`. (Rigidbody2D + CapsuleCollider2D are still on the objects.)
2. `GameManager`: switcher script with the three characters listed in order.
3. `Main Camera`: camera-follow script pointing at the switcher.
4. `Grid/Tilemap_Hazards`: TilemapCollider2D set to **trigger** + hazard script.
5. `Heart (cave reward)`: CircleCollider2D trigger + collectible script.
6. `Goal`: BoxCollider2D trigger + goal script, wire `Canvas/CompleteText`.

## What was removed (2026-09-26)

- Scripts: `PlayerController2D`, `CharacterSwitcher`, `CharacterStats`, `CameraFollow`, `Hazard`, `Collectible`, `GoalMarker`, `DevNote`.
- Assets: `Assets/Data/` (the three stats assets), `Assets/_FOREST DEMO - test/Sounds/` (all sfx).
- Scene components: the above scripts plus all `AudioSource`s. All GameObjects, sprites, tilemaps, colliders, and rigidbodies were kept.
