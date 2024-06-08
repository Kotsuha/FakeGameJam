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

    private void PositionBottomIceCream()
    {
        RaycastHit hit;
        Vector3 rayOrigin = transform.position + Vector3.up * 100;
        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, Mathf.Infinity, layerMask: LayerMask.GetMask(GameManager.GroundLayer)))
        {
            bottomIceCream.position = hit.point;
        }
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
            PositionBottomIceCream();
        }
        else if (MeltProcess <= 0.001f)
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
