using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handler script to modify the models materials, used by the phase shift effect
/// </summary>
public class ShipMaterialHandler : MonoBehaviour
{
    [Header("Mesh Renderers")]
    [SerializeField] private GameObject[] subModelsWithMeshes;

    // Local variables
    private List<MaterialList> meshRendererObjects = new();

    // Material List Class for handling mesh renderers with multiple mats
    private class MaterialList
    {
        private MeshRenderer meshRenderer;
        private readonly Material[] defaultMaterials;
        
        public MaterialList(MeshRenderer meshRenderer)
        {
            this.meshRenderer = meshRenderer;

            // Populate default materials list
            defaultMaterials = new Material[meshRenderer.materials.Length];
            for (int i = 0; i < meshRenderer.materials.Length; i++)
            {
                defaultMaterials[i] = new Material(meshRenderer.materials[i]);
            }
        }

        /// <summary> Set materials to default </summary>
        public void SetMatDefault()
        {
            // Remove existing materials
            meshRenderer.materials = new Material[0];

            // Set it to default
            meshRenderer.materials = defaultMaterials;
        }

        /// <summary> Set materials to target </summary>
        public void SetMatTarget(Material input)
        {
            // Remove existing materials
            meshRenderer.materials = new Material[0];

            // Set it to the input only
            Material[] inputMat = new Material[] { input };
            meshRenderer.materials = inputMat;
        }
    }
    
    // Get all mesh renderers
    void Awake()
    {
        // Create mesh renderer list
        for (int i = 0; i < subModelsWithMeshes.Length; i++)
        {
            MeshRenderer meshRendComp = subModelsWithMeshes[i].GetComponent<MeshRenderer>();
            MaterialList materialObj = new MaterialList(meshRendComp);
            meshRendererObjects.Add(materialObj);
        }
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
}
