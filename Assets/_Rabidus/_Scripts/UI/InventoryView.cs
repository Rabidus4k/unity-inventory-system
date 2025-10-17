using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryView : MonoBehaviour, IInventoryView
{
    [Header("Prefabs & Refs")]
    [SerializeField] private InventorySlotView slotPrefab;
    [SerializeField] private ItemView itemPrefab;
    [SerializeField] private RectTransform slotsParent;
    [SerializeField] private DragCursor dragCursor;
    
    [Header("UI Buttons")]
    [SerializeField] private Button useButton;
    [SerializeField] private Button deleteButton;

    private readonly List<InventorySlotView> _slots = new List<InventorySlotView>();

    public event Action<int> OnBeginDrag;
    public event Action<int, int> OnDropOnSlot;
    public event Action<int> OnDropOutside;

    public event Action<int> OnSlotClick;

    public event Action OnUseClick;
    public event Action OnDeleteClick;

    private int _selectedIndex = -1;

    private void Awake()
    {
        if (useButton) useButton.onClick.AddListener(() => OnUseClick?.Invoke());
        if (deleteButton) deleteButton.onClick.AddListener(() => OnDeleteClick?.Invoke());
        SetButtonsState(false, false);
    }

    public void BuildSlots(int slotCount)
    {
        ClearChildren(slotsParent);
        _slots.Clear();

        for (int i = 0; i < slotCount; i++)
        {
            var slot = Instantiate(slotPrefab, slotsParent);
            slot.Init(i, this);
            var item = Instantiate(itemPrefab);
            item.Init(i, this, dragCursor);
            slot.BindItemView(item);
            _slots.Add(slot);
        }
    }

    public void RenderSlot(int index, SlotRenderData data)
    {
        if (index < 0 || index >= _slots.Count) return;
        _slots[index].ItemView.Render(data);
    }
    public void SetButtonsState(bool canUse, bool canDelete)
    {
        if (useButton) useButton.interactable = canUse;
        if (deleteButton) deleteButton.interactable = canDelete;
    }

    public void RaiseBeginDrag(int sourceIndex) => OnBeginDrag?.Invoke(sourceIndex);
    public void RaiseDropOnSlot(int sourceIndex, int targetIndex) => OnDropOnSlot?.Invoke(sourceIndex, targetIndex);
    public void RaiseDropOutside(int sourceIndex) => OnDropOutside?.Invoke(sourceIndex);
    public void RaiseSlotClick(int index) => OnSlotClick?.Invoke(index);

    private static void ClearChildren(RectTransform rt)
    {
        for (int i = rt.childCount - 1; i >= 0; i--)
        {
            var c = rt.GetChild(i);
#if UNITY_EDITOR
            if (!Application.isPlaying)
                GameObject.DestroyImmediate(c.gameObject);
            else
                GameObject.Destroy(c.gameObject);
#else
            GameObject.Destroy(c.gameObject);
#endif
        }
    }

    public void SetSelected(int index)
    {
        _selectedIndex = index;
        for (int i = 0; i < _slots.Count; i++)
            _slots[i].SetSelected(i == index);
    }
}