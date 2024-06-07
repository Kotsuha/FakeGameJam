using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimController : MonoBehaviour
{
    [SerializeField]
    private MeltShaderController meltShaderController;

    public void UpdateMeltingAnim(float ratio)
    {
        meltShaderController.SetMeltProgress(1 - ratio);
    }
}
