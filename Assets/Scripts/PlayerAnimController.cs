using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimController : MonoBehaviour
{
    [SerializeField]
    private MeltShaderController meltShaderController;

    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public Animator GetAnimator()
    {
        return animator;
    }

    public void UpdateMeltingAnim(float ratio)
    {
        meltShaderController.SetMeltProgress(1 - ratio);
    }
}
