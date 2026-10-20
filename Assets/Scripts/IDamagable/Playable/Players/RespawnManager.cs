using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/// Replaces the hard scene reload on death: the fallen character disappears,
/// the camera travels slowly back to the start, gentle fog blooms at the spawn
/// and the bear wakes up inside it with full health. Level progress is kept.
public class RespawnManager : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private CameraFollow cameraFollow;
    [SerializeField] private float cameraPanDuration = 2.2f;
    [SerializeField] private float fogDuration = 1.6f;
    [SerializeField] private Color fogColor = new Color(0.85f, 0.88f, 0.95f, 0.75f);

    private CharacterSwitcher switcher;
    private HealthSystem healthSystem;
    private PlayerInput playerInput;
    private bool respawning;
    private static Sprite fogSprite;

    void Awake()
    {
        switcher = GetComponent<CharacterSwitcher>();
        healthSystem = GetComponent<HealthSystem>();
        playerInput = GetComponent<PlayerInput>();
        if (cameraFollow == null)
            cameraFollow = FindAnyObjectByType<CameraFollow>();
        if (spawnPoint == null)
        {
            var sp = GameObject.Find("SpawnPoint");
            if (sp != null) spawnPoint = sp.transform;
        }
    }

    public void BeginRespawn()
    {
        if (respawning)
            return;
        respawning = true;
        StartCoroutine(RespawnSequence());
    }

    private IEnumerator RespawnSequence()
    {
        // the fallen character disappears, input is cut
        if (playerInput != null) playerInput.enabled = false;
        switcher.ActiveCharacter.gameObject.SetActive(false);

        yield return new WaitForSeconds(0.5f); // a small beat after the hit

        // slow camera travel back to the start of the level
        Camera cam = Camera.main;
        if (cameraFollow != null) cameraFollow.enabled = false;
        if (cam != null && cameraFollow != null && spawnPoint != null)
        {
            Vector3 from = cam.transform.position;
            float targetX = Mathf.Clamp(spawnPoint.position.x, cameraFollow.minX, cameraFollow.maxX);
            Vector3 to = new Vector3(targetX, cameraFollow.fixedY, from.z);
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime / cameraPanDuration;
                cam.transform.position = Vector3.Lerp(from, to, Mathf.SmoothStep(0f, 1f, t));
                yield return null;
            }
        }

        // gentle fog blooms at the spawn; the bear appears inside it
        Vector3 spawnPos = spawnPoint != null ? spawnPoint.position : transform.position;
        GameObject[] puffs = SpawnFog(spawnPos + new Vector3(0f, 1.3f, 0f));

        yield return new WaitForSeconds(fogDuration * 0.3f);

        switcher.ForceCharacter(0, spawnPos); // always wake up as the bear
        healthSystem.ResetHealth();

        yield return new WaitForSeconds(fogDuration * 0.7f);

        foreach (var p in puffs)
            if (p != null) Destroy(p);
        if (cameraFollow != null) cameraFollow.enabled = true;
        if (playerInput != null) playerInput.enabled = true;
        respawning = false;
    }

    private GameObject[] SpawnFog(Vector3 center)
    {
        Vector3[] offsets =
        {
            new Vector3(-0.7f,  0.1f, 0f),
            new Vector3( 0.6f, -0.1f, 0f),
            new Vector3( 0.0f,  0.5f, 0f),
            new Vector3( 0.1f, -0.5f, 0f)
        };
        var puffs = new GameObject[offsets.Length];
        for (int i = 0; i < offsets.Length; i++)
        {
            var go = new GameObject("FogPuff");
            go.transform.position = center + offsets[i];
            var sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = GetFogSprite();
            sr.color = fogColor;
            sr.sortingOrder = 60; // drawn in front of the character
            StartCoroutine(AnimatePuff(go.transform, sr, 0.08f * i));
            puffs[i] = go;
        }
        return puffs;
    }

    private IEnumerator AnimatePuff(Transform puff, SpriteRenderer sr, float delay)
    {
        yield return new WaitForSeconds(delay);
        float t = 0f;
        Vector3 drift = new Vector3(Random.Range(-0.3f, 0.3f), 0.35f, 0f);
        while (t < 1f && puff != null)
        {
            t += Time.deltaTime / fogDuration;
            puff.localScale = Vector3.one * Mathf.Lerp(1.2f, 3.4f, t);
            puff.position += drift * Time.deltaTime;
            Color c = fogColor;
            c.a = fogColor.a * (t < 0.25f ? t / 0.25f : 1f - (t - 0.25f) / 0.75f);
            sr.color = c;
            yield return null;
        }
    }

    // soft radial gradient generated in code, so no art asset is needed
    private static Sprite GetFogSprite()
    {
        if (fogSprite != null)
            return fogSprite;

        const int size = 64;
        var tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float d = Vector2.Distance(new Vector2(x, y), new Vector2(size / 2f, size / 2f)) / (size / 2f);
                float a = Mathf.Clamp01(1f - d);
                tex.SetPixel(x, y, new Color(1f, 1f, 1f, a * a));
            }
        }
        tex.Apply();
        fogSprite = Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 64f);
        return fogSprite;
    }
}
