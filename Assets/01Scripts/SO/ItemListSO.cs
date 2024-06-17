using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Item/ItemListSO")]
public class ItemListSO : ScriptableObject
{
    public List<Item> ItemList;
}
