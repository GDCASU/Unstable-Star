using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowingItem : MonoBehaviour
{
    [SerializeField] Color glowingColor;
    Material glowingMaterial;
    MeshRenderer[] mRenderers;

    private void Awake()
    {
        Shader shader = Shader.Find("Unlit/SFX/PulsingEffect");
        glowingMaterial = new Material(shader);

        if(glowingColor != null ) glowingMaterial.color = glowingColor;
    }

    private void Start()
    {
        mRenderers = GetComponentsInChildren<MeshRenderer>();

        StartGlowing();
    }

    /// <summary>
    /// Makes the item start glowing.
    /// </summary>
    public void StartGlowing()
    {
        foreach (MeshRenderer mRenderer in mRenderers)
        {
            List<Material> materials = new List<Material>(mRenderer.sharedMaterials);
            materials.Add(glowingMaterial);
            mRenderer.SetMaterials(materials);
        }
    }

    /// <summary>
    /// Makes the item stop glowing and destroys this component.
    /// </summary>
    public void StopGlowing()
    {
        foreach (MeshRenderer mRenderer in mRenderers)
        {
            List<Material> materials = new List<Material>(mRenderer.sharedMaterials);
            materials.Remove(glowingMaterial);
            mRenderer.SetMaterials(materials);
        }

        Destroy(this);
    }
}
