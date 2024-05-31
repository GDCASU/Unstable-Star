using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handler script to modify the models materials, used by the phase shift effect
/// </summary>
public class ShipMaterialHandler : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] [Range(0f, 1f)] private float iFramesTransparency;
    [SerializeField] private float flashingInterval;
    [SerializeField] private Material flashingMat;

    [Header("Mesh Renderers")]
    [SerializeField] private GameObject[] subModelsWithMeshes;

    // Local variables
    private List<MaterialList> meshRendererObjects = new();
    private Coroutine iFramesFlashRoutine;

    // Material List Class for handling mesh renderers with multiple mats
    private class MaterialList
    {
        private MeshRenderer meshRenderer;
        private Material flashingMat;
        private readonly Material[] defaultMaterials;
        private readonly Material[] emptyList = new Material[0];

        public MaterialList(MeshRenderer meshRenderer, Material flashMat)
        {
            this.meshRenderer = meshRenderer;

            // Populate default materials list
            defaultMaterials = new Material[meshRenderer.materials.Length];
            for (int i = 0; i < meshRenderer.materials.Length; i++)
            {
                defaultMaterials[i] = new Material(meshRenderer.materials[i]);
            }

            // Set the flashing mat
            flashingMat = flashMat;
        }

        /// <summary> Set materials to default </summary>
        public void SetMatDefault()
        {
            // Remove existing materials
            meshRenderer.materials = emptyList;

            // Set it to default
            meshRenderer.materials = defaultMaterials;
        }

        /// <summary> Set materials to target </summary>
        public void SetMatTarget(Material input)
        {
            // Remove existing materials
            meshRenderer.materials = emptyList;

            // Set it to the input only
            Material[] inputMat = new Material[] { input };
            meshRenderer.materials = inputMat;
        }

        /// <summary> Set materials to transparent defaults </summary>
        public void SetMatTransparent()
        {
            // Remove existing materials
            meshRenderer.materials = emptyList;

            // Set it to default
            meshRenderer.material = flashingMat;
        }
    }
    
    // Get all mesh renderers
    void Awake()
    {
        // Create mesh renderer list
        for (int i = 0; i < subModelsWithMeshes.Length; i++)
        {
            MeshRenderer meshRendComp = subModelsWithMeshes[i].GetComponent<MeshRenderer>();
            MaterialList materialObj = new MaterialList(meshRendComp, flashingMat);
            meshRendererObjects.Add(materialObj);
        }

        // Subscribe to events
        EventData.OnInvulnerabilityToggled += HandleFrameFlash;
    }

    private void OnDestroy()
    {
        // Unsubscribe from events on destroy
        EventData.OnInvulnerabilityToggled -= HandleFrameFlash;
    }

    /// <summary> Sets the ships materials back to default </summary>
    public void SetDefaultMaterials()
    {
        int i;
        for (i = 0; i < meshRendererObjects.Count; i++)
        {
            meshRendererObjects[i].SetMatDefault();
        }
    }

    /// <summary> Sets the ships materials to the argument passed </summary>
    public void SetMaterialsTo(Material inputMat)
    {
        int i;
        for (i = 0; i < meshRendererObjects.Count; i++)
        {
            meshRendererObjects[i].SetMatTarget(inputMat);
        }
    }

    /// <summary> Sets the ships materials to transparents </summary>
    public void SetMaterialsTransparent()
    {
        int i;
        for (i = 0; i < meshRendererObjects.Count; i++)
        {
            meshRendererObjects[i].SetMatTransparent();
        }
    }

    /// <summary> Function called by the invulnerability event, Handles the flashing routine </summary>
    private void HandleFrameFlash(bool isEntering)
    {
        // If its starting, attempt to start the flashing effect
        if (isEntering && iFramesFlashRoutine == null)
        {
            iFramesFlashRoutine = StartCoroutine(iFramesFlash());
            return;
        }
        // Else, its exiting, stop it
        StopCoroutine(iFramesFlashRoutine);
        iFramesFlashRoutine = null;
        SetDefaultMaterials();
    }

    // Coroutine to make the player ship flash while invulnerable
    private IEnumerator iFramesFlash()
    {
        // Alternate the materials from normal to transparent while running
        WaitForSeconds interval = new WaitForSeconds(flashingInterval);
        while (true) 
        {
            SetMaterialsTransparent();
            yield return interval;
            SetDefaultMaterials();
            yield return interval;
        }
    }
}
