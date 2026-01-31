using UnityEngine;

[CreateAssetMenu(menuName = "Game/Item", fileName = "NewItem")]
public class Item : ScriptableObject
{
    public string id;
    public string title;
    public Sprite icon;
}
