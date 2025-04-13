using UnityEngine;
using System.Collections;

public class XarksFogDomeController : MonoBehaviour
{
    public static XarksFogDomeController Instance { get; private set; }

    [Header("Dome Prefab & Material")]
    public GameObject fogDomePrefab;
    public Material domeMaterial;

    private GameObject spawnedDome;
    private Camera localCam;

    [Header("Fog Settings")]
    public Color fogColor = new Color(1f, 0f, 0f, 1f);
    [Range(0f, 1f)] public float fogDensity = 0.5f;
    [Range(0f, 1f)] public float minFog = 0.1f;

    [Header("Runtime Flags")]
    public bool IsClient = true; // Default to true in editor, must be set false by the server

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public IEnumerator WaitForRealLocalCamera()
    {
        while (true)
        {
            Camera[] cams = Camera.allCameras;
            Debug.Log("[XarksFog] Waiting for Main Camera SCENE to exist...");

            foreach (Camera cam in cams)
            {
                if (cam.name == "Main Camera SCENE" && cam.enabled && cam.isActiveAndEnabled && cam.targetTexture == null)
                {
                    localCam = cam;
                    Debug.Log($"✅ [XarksFog] Hard-selected local player camera: {cam.name}");
                    SpawnFogDome();
                    yield break;
                }
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    private void ForceCameraCullingMask()
    {
        if (localCam == null || spawnedDome == null) return;

        int fogLayer = spawnedDome.layer;
        string layerName = LayerMask.LayerToName(fogLayer);

        if ((localCam.cullingMask & (1 << fogLayer)) == 0)
        {
            localCam.cullingMask |= (1 << fogLayer);
            Debug.Log($"[XarksFog] Added dome's layer '{layerName}' to camera culling mask.");
        }
        else
        {
            Debug.Log($"[XarksFog] Camera already rendering dome's layer '{layerName}'.");
        }
    }

    private IEnumerator VisibilityCheck()
    {
        yield return new WaitForSeconds(1f);
        if (spawnedDome.TryGetComponent(out Renderer renderer))
        {
            Debug.Log($"[XarksFog] Renderer visibility: {renderer.isVisible}");
        }
    }

    public void SpawnFogDome()
    {
        if (spawnedDome != null || localCam == null) return;

        spawnedDome = Instantiate(fogDomePrefab, localCam.transform.position, Quaternion.identity);
        spawnedDome.transform.SetParent(localCam.transform);
        spawnedDome.transform.localPosition = new Vector3(0, 0, 10); // Push forward
        spawnedDome.transform.localScale = Vector3.one * 500f;

        Debug.Log($"[XarksFog] Spawned dome: {spawnedDome.name} | Position: {spawnedDome.transform.position} | Scale: {spawnedDome.transform.localScale}");

        ForceCameraCullingMask();

        if (spawnedDome.TryGetComponent(out Renderer renderer))
        {
            if (domeMaterial != null && domeMaterial.shader != null)
            {
                renderer.material = domeMaterial;
                Debug.Log("[XarksFog] Assigned domeMaterial to fog dome renderer.");
            }
            else
            {
                Debug.LogWarning("[XarksFog] domeMaterial is null or missing shader. Falling back to magenta debug material.");
                renderer.material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                renderer.material.color = Color.magenta;
            }

            renderer.material.renderQueue = 5000; // Force render over everything
            renderer.enabled = true;
        }

        ApplyFogSettings();
        StartCoroutine(VisibilityCheck());
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

    public void SetFog(Color color, float density, float min)
    {
        fogColor = color;
        fogDensity = density;
        minFog = min;
        ApplyFogSettings();
    }

    public void ApplyModVarSettings(float r, float g, float b, float a, float density, float min)
    {
        SetFog(new Color(r, g, b, a), density, min);
    }
}