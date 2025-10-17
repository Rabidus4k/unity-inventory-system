using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIContextMenu : MonoBehaviour
{
    [SerializeField] private RectTransform _root;
    [SerializeField] private Image _icon;
    [SerializeField] private TMPro.TextMeshProUGUI _nameText;
    [SerializeField] private TMPro.TextMeshProUGUI _typeText;

    public RectTransform Root => _root;

    public void Initialize(ItemDefinition item)
    {
        _icon.sprite = item.Icon;
        _nameText.SetText(item.DisplayName);
        _typeText.SetText(item.ItemType.ToString());
    }
}
