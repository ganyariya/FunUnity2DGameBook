using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : MonoBehaviour
{
    [SerializeField]
    private int value = 0;

    public int GetValue()
    {
        return value;
    }

    public void DestroyObject()
    {
        // Destroy(this) だと「ItemData コンポーネント」のみが削除されるので注意
        Destroy(gameObject);
    }
}
