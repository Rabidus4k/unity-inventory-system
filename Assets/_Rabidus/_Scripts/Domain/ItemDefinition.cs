using System;
using UnityEditor;
using UnityEngine;

public enum ItemType
{
    None,
    Block,
    Armor
}

[CreateAssetMenu(menuName = "Inventory/Item Definition", fileName = "ItemDefinition")]
public class ItemDefinition : ScriptableObject
{
    [SerializeField] private string id;
    [SerializeField] private string displayName;
    [SerializeField] private Sprite icon;
    [SerializeField] private bool stackable = true;
    [SerializeField] private int maxStack = 99;
    [SerializeField] private ItemType type;

    public string Id => id;
    public string DisplayName => displayName;
    public Sprite Icon => icon;
    public bool Stackable => stackable;
    public int MaxStack => Mathf.Max(1, maxStack);
    public ItemType ItemType => type;

    [ContextMenu("Regenerate ID")]
    private void RegenerateId()
    {
        id = Guid.NewGuid().ToString();
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
    }
}