using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public virtual void EquipItem(Transform user)
    {

    }

    public virtual void UnequipItem(Transform user)
    {
        Destroy(gameObject);
    }
}
