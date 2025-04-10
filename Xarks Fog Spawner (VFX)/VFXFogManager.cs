using UnityEngine;
using UnityEngine.VFX;

public class VFXFogManager : MonoBehaviour
{
    [SerializeField] private VisualEffect fogEffect;

    // Default fallback values
    private Vector3 fogBounds = new Vector3(500f, 10f, 500f);
    private Color fogColor = new Color(0.5f, 0.5f, 0.5f, 0.2f);

    private void Start()
    {
        if (fogEffect == null)
        {
            Debug.LogWarning("VFXFogManager: VisualEffect reference not assigned.");
            return;
        }

        ApplyFogSettings();
    }

    public void PassConfigVariables(string[] values)
    {
        foreach (var entry in values)
        {
            var split = entry.Split(':');
            if (split.Length != 3 || split[0] != "XFS") continue;

            string key = split[1].Trim().ToLowerInvariant();
            string[] components = split[2].Split(',');

            switch (key)
            {
                case "color":
                    if (TryParseColor(components, out Color color))
                        fogColor = color;
                    break;

                case "tile shape":
                case "bounds":
                    if (TryParseVector3(components, out Vector3 bounds))
                        fogBounds = bounds;
                    break;
            }
        }

        ApplyFogSettings();
    }

    private void ApplyFogSettings()
    {
        if (fogEffect == null) return;

        fogEffect.SetVector3("Bounds", fogBounds);
        fogEffect.SetVector4("FogColor", fogColor);
        fogEffect.Play();

        Debug.Log($"VFXFogManager: Applied bounds {fogBounds} and color {fogColor}");
    }

    private bool TryParseVector3(string[] parts, out Vector3 result)
    {
        result = Vector3.zero;
        return parts.Length == 3 &&
               float.TryParse(parts[0], out result.x) &&
               float.TryParse(parts[1], out result.y) &&
               float.TryParse(parts[2], out result.z);
    }

    private bool TryParseColor(string[] parts, out Color result)
    {
        result = Color.white;
        return parts.Length == 4 &&
               float.TryParse(parts[0], out result.r) &&
               float.TryParse(parts[1], out result.g) &&
               float.TryParse(parts[2], out result.b) &&
               float.TryParse(parts[3], out result.a);
    }
}
