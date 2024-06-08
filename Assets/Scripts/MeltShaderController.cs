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
    private float bottomLimit;
    private float maxMeltOffset = 1.0f;
    private float offset;

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
            offset = hit.point.y;
        }
    }

    private void CalculateBottomLimit()
    {
        // Calculate the world position of the object's bottom
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            bottomLimit = collider.bounds.min.y;
        }
        else
        {
            bottomLimit = transform.position.y; // Fallback if no collider is present
        }
    }

    private void AdjustPositionForCollisions()
    {
        // Perform a downward raycast to check for collisions
        RaycastHit hit;
        Vector3 rayOrigin = transform.position + Vector3.up * 1; // Start the ray above the object
        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, Mathf.Infinity))
        {
            // Adjust the position based on the hit point, but limit the downward movement
            float newY = Mathf.Max(transform.position.y - (MeltProcess * maxMeltOffset), hit.point.y);
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
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
        //CalculateBottomLimit();
        //AdjustPositionForCollisions();
        foreach (var mat in materials)
        {
            mat.SetFloat(Mat_MeltProgress, value);
            //mat.SetFloat("_BottomLimit", bottomLimit);
            mat.SetFloat("_Offset", offset);
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
