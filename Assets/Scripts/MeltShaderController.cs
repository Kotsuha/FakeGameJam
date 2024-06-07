using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[RequireComponent(typeof(Material))]
public class MeltShaderController : MonoBehaviour
{
    MeshRenderer meshRenderer;
    List<Material> materials;

    readonly string Mat_MeltProgress = "_MeltProgress";

    [Range(0, 1)]
    public float MeltProcess;
    public float size;
    [SerializeField] Transform bottomIceCream;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        materials = meshRenderer.materials.ToList();
    }

    // public void Init(Material material)
    // {
    //     this.materials = material;
    // }

    // private void Update()
    // {
    //     SetMeltProgress(MeltProcess);
    // }


    public void SetMeltProgress(float value)
    {
        MeltProcess = value;
        foreach (var mat in materials)
        {
            mat.SetFloat(Mat_MeltProgress, value);
        }
        if (MeltProcess >= 0.001f)
        {
            bottomIceCream.localScale = new Vector3(MeltProcess * size, 1, MeltProcess * size);
        }
        else if(MeltProcess <= 0.001f)
        {
            bottomIceCream.localScale = Vector3.zero;
        }
    }

    // public float GetMelyProgress()
    // {
    //     foreach (var mat)
    //     return materials.GetFloat(Mat_MeltProgress);
    // }
}
