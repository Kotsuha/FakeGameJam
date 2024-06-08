using System;
using CartoonFX;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ItemBase : MonoBehaviour, IItem
{
    [SerializeField] protected CFXR_Effect cFXR_Effect;

    public virtual string Name => string.Empty;

    public virtual event Action<string> OnGetItem;


    private void Start()
    {
        cFXR_Effect.cameraShake.cameras.Add(Camera.main);
    }

    public void SetEffect(bool isShow)
    {
        cFXR_Effect.gameObject.SetActive(true);
    }

    protected virtual async UniTask DestroyAsync(int delay)
    {
        await UniTask.Delay(delay);
        Destroy(this.gameObject);
    }
}
