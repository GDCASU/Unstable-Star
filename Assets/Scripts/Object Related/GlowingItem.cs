using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowingItem : MonoBehaviour
{
    Material glowingMaterial;
    MeshRenderer[] mRenderers;

    [Header("Debugging")]
    [SerializeField] bool printDebug = false;
    [SerializeField] bool glowAtStart = false;

    private void Awake()
    {
        Shader shader = Shader.Find("Unlit/SFX/PulsingEffect");
        glowingMaterial = new Material(shader);
    }

    private void Start()
    {
        mRenderers = GetComponentsInChildren<MeshRenderer>();

        if (glowAtStart) StartGlowing();
    }

    /// <summary>
    /// Makes the item start glowing.
    /// </summary>
    public void StartGlowing()
    {
        foreach (MeshRenderer mRenderer in mRenderers) {
            List<Material> materials = new List<Material>(mRenderer.sharedMaterials);
            materials.Add(glowingMaterial);
            mRenderer.SetMaterials(materials);
        }
    }

    /// <summary>
    /// Makes the item stop glowing and resets the material.
    /// </summary>
    public void StopGlowing()
    {
        foreach (MeshRenderer mRenderer in mRenderers)
        {
            List<Material> materials = new List<Material>(mRenderer.sharedMaterials);
            materials.Remove(glowingMaterial);
            mRenderer.SetMaterials(materials);
        }
    }
}
