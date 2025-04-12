using UnityEngine;

public class XarksFogDomeController : MonoBehaviour
{
    [Header("Dome Prefab & Material")]
    public GameObject fogDomePrefab;
    public Material domeMaterial;

    private GameObject spawnedDome;

    [Header("Fog Settings")]
    public Color fogColor = new Color(1f, 0f, 0f, 1f);
    [Range(0f, 1f)] public float fogDensity = 0.5f;
    [Range(0f, 1f)] public float minFog = 0.1f;

    private void Start()
    {
#if UNITY_EDITOR
        SpawnFogDome();
#endif
    }

    public void SpawnFogDome()
    {
        if (spawnedDome != null) return;

        Transform cam = Camera.main?.transform;
        if (cam == null)
        {
            Debug.LogWarning("❌ XarksFogDomeController: No main camera found for Fog Dome!");
            return;
        }

        spawnedDome = Instantiate(fogDomePrefab, cam.position, Quaternion.identity);
        spawnedDome.transform.SetParent(cam);
        spawnedDome.transform.localPosition = Vector3.zero;

        ApplyFogSettings();
    }

    public void RemoveFogDome()
    {
        if (spawnedDome != null)
        {
            Destroy(spawnedDome);
            spawnedDome = null;
        }
    }

    public void ApplyFogSettings()
    {
        if (domeMaterial == null)
        {
            Debug.LogWarning("⚠️ XarksFogDomeController: domeMaterial not assigned!");
            return;
        }

        domeMaterial.SetColor("_FogColor", fogColor);
        domeMaterial.SetFloat("_FogDensity", fogDensity);
        domeMaterial.SetFloat("_MinFog", minFog);
    }
}
