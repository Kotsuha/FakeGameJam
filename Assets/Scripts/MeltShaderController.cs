using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Material))]
public class MeltShaderController : MonoBehaviour
{
    MeshRenderer meshRenderer;
    Material material;

    readonly string Mat_MeltProgress = "_MeltProgress";

    [Range(0, 1)]
    public float MeltProcess;
    public float size;
    [SerializeField] Transform bottomIceCream;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        material = meshRenderer.sharedMaterial;
    }

    public void Init(Material material)
    {
        this.material = material;
    }

    private void Update()
    {
        SetMeltProgress(MeltProcess);
    }


    public void SetMeltProgress(float value)
    {
        material.SetFloat(Mat_MeltProgress, value);
        if (MeltProcess >= 0.001f)
        {
            bottomIceCream.localScale = new Vector3(MeltProcess * size, 1, MeltProcess * size);
        }
        else if(MeltProcess <= 0.001f)
        {
            bottomIceCream.localScale = Vector3.zero;
        }
    }

    public float GetMelyProgress()
    {
        return material.GetFloat(Mat_MeltProgress);
    }
}
