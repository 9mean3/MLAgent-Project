using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGround : Ground
{
    [SerializeField] private ItemListSO itemSO;

    private void Awake()
    {
        int r = Random.Range(0, itemSO.ItemList.Count);
        Instantiate(itemSO.ItemList[r], transform.position, Quaternion.identity);
    }
}
