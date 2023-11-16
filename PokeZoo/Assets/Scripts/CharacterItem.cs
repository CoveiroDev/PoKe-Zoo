using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Characters/New Character")]
public class CharacterItem : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
    public int itemID;
}
