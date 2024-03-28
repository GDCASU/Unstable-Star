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
    private List<MaterialList> meshRendererObjects;

    // Material List Class for handling mesh renderers with multiple mats
    private class MaterialList
    {
        private MeshRenderer meshRenderer;
        private readonly List<Material> defaultMaterials;
        private List<Material> inputMatNonAlloc = new List<Material>();
        
        public MaterialList(MeshRenderer meshRenderer)
        {
            this.meshRenderer = meshRenderer;

            // Populate default materials list
            meshRenderer.GetMaterials(defaultMaterials);

            // HACK: TESTING
            string msg = "Default Material List for = " + meshRenderer.gameObject.name + "\n";
            msg += "Default Mat List = " + defaultMaterials;
            Debug.Log(msg);
        }

        // Method to set materials to default
        public void SetMatDefault()
        {
            meshRenderer.SetMaterials(defaultMaterials);
            // HACK: TESTING
            string msg = "SetMatDefault called for = " + meshRenderer.gameObject.name + "\n";
            msg += "Mat List = " + meshRenderer.materials;
            Debug.Log(msg);
        }

        // Method to set materials to target
        public void SetMatTarget(Material input)
        {
            // Method only takes an array, so we place it into one
            inputMatNonAlloc[0] = input;
            meshRenderer.SetMaterials(inputMatNonAlloc);
            // HACK: TESTING
            string msg = "SetMatTarget called for = " + meshRenderer.gameObject.name + "\n";
            msg += "Mat List = " + meshRenderer.materials;
            Debug.Log(msg);
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
