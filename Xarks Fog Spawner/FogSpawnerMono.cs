using UnityEngine;

public class FogSpawnerMono : MonoBehaviour
{
    public static FogSpawnerMono Instance;

    [Header("Required")]
    [SerializeField] private GameObject fogPrefab;

    [Header("Default Fog Settings")]
    public Vector3 fogPosition = new Vector3(0f, 10f, 0f);
    public Vector3 fogScale = new Vector3(1f, 1f, 1f);
    public float fogSize = 10f;
    public float fogEmissionRate = 1000f;
    public float startSpeed = 0f;
    public Color fogColor = new Color(0.9f, 0.9f, 0.9f, 0.3f);

    [Header("Shape Settings")]
    public Vector3 shapeScale = new Vector3(100f, 20f, 100f);

    [Header("Noise Settings")]
    public float noiseStrength = 0.5f;
    public float noiseScroll = 0.1f;

    private GameObject fogInstance;

    private void Awake()
    {
        Instance = this;
    }

#if UNITY_EDITOR
    private void Start()
    {
        if (Application.isPlaying && fogInstance == null)
        {
            Debug.Log("[FogSpawnerMono] Editor Play Mode — auto-spawning fog.");
            SpawnFog();
        }
    }
#endif

    public void SpawnFog()
    {
        if (fogInstance != null)
        {
            Debug.Log("FogSpawnerMono: Fog already exists.");
            return;
        }

        if (fogPrefab == null)
        {
            Debug.LogError("FogSpawnerMono: Fog prefab not assigned.");
            return;
        }

        fogInstance = Instantiate(fogPrefab);
        fogInstance.name = "CustomFog";
        fogInstance.transform.position = fogPosition;
        fogInstance.transform.localScale = fogScale;

        var ps = fogInstance.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            var main = ps.main;
            main.startSize = fogSize;
            main.startColor = fogColor;
            main.startSpeed = startSpeed;

            var emission = ps.emission;
            emission.rateOverTime = fogEmissionRate;

            var shape = ps.shape;
            shape.enabled = true;
            shape.scale = shapeScale;

            var noise = ps.noise;
            noise.enabled = true;
            noise.strength = noiseStrength;
            noise.scrollSpeed = noiseScroll;
        }

        Debug.Log($"FogSpawnerMono: Spawned fog at {fogPosition} with scale {fogScale}, shape box {shapeScale}, color {fogColor}.");
    }
}
