using System;
using System.Diagnostics;

public class InventoryPresenter
{
    private readonly IInventory _model;
    private readonly IInventoryView _view;

    private readonly IItemUseService _useService;
    private readonly IItemUseService _deleteService;
    private readonly IItemUseService _contextService;

    private int? _dragSourceIndex;
    private int _selectedIndex = -1;

    public InventoryPresenter(IInventory model, IInventoryView view, IItemUseService useService, IItemUseService deleteService, IItemUseService contextService)
    {
        _model = model ?? throw new ArgumentNullException(nameof(model));
        _view = view ?? throw new ArgumentNullException(nameof(view));

        _useService = useService;
        _deleteService = deleteService;
        _contextService = contextService;

        _model.SlotChanged += OnModelSlotChanged;
        _view.OnBeginDrag += OnBeginDrag;
        _view.OnDropOnSlot += OnDropOnSlot;
        _view.OnDropOutside += OnDropOutside;

        _view.OnSlotClick += OnSlotClicked;
        _view.OnDeleteClick += OnDeleteClicked;
        _view.OnUseClick += OnUseClicked;
    }

    public void Initialize()
    {
        _view.BuildSlots(_model.SlotCount);
        for (int i = 0; i < _model.SlotCount; i++)
            Render(i);
    }

    private void OnSlotClicked(int index)
    {
        _selectedIndex = index;
        _view.SetSelected(index);
        UpdateButtons();
        UpdateContextMenu();
    }

    private void UpdateContextMenu()
    {
        if (_selectedIndex >= 0 && _selectedIndex < _model.SlotCount)
        {
            var slot = _model.GetSlot(_selectedIndex);
            if (!slot.IsEmpty)
            {
                var canShowContext = _contextService != null && _contextService.CanUse(slot.Stack);

                if (canShowContext)
                {
                    ContextMenuManager.Instance.ShowContextMenu(slot.Stack.Definition);
                }
                else
                {
                    ContextMenuManager.Instance.HideContextMenu();
                }
            }
            else
            {
                ContextMenuManager.Instance.HideContextMenu();
            }
        }
        else
        {
            ContextMenuManager.Instance.HideContextMenu();
        }
    }

    private void UpdateButtons()
    {
        bool canDelete = false, canUse = false;
        if (_selectedIndex >= 0 && _selectedIndex < _model.SlotCount)
        {
            var slot = _model.GetSlot(_selectedIndex);
            if (!slot.IsEmpty)
            {
                canDelete = _deleteService != null && _deleteService.CanUse(slot.Stack);
                canUse = _useService != null && _useService.CanUse(slot.Stack);
            }
        }
        _view.SetButtonsState(canUse, canDelete);
    }

    private void OnModelSlotChanged(int index) => Render(index);

    private void Render(int index)
    {
        var slot = _model.GetSlot(index);
        var data = new SlotRenderData
        {
            HasItem = !slot.IsEmpty,
            Icon = slot.IsEmpty ? null : slot.Stack.Definition.Icon,
            Count = slot.IsEmpty ? 0 : slot.Stack.Quantity,
            IsStackable = slot.IsEmpty ? false : slot.Stack.Definition.Stackable
        };
        _view.RenderSlot(index, data);
    }

    private void OnBeginDrag(int sourceIndex)
    {
        var slot = _model.GetSlot(sourceIndex);
        if (slot.IsEmpty) return;

        OnSlotClicked(-1);

        _dragSourceIndex = sourceIndex;
    }

    private void OnDropOnSlot(int sourceIndex, int targetIndex)
    {
        if (_dragSourceIndex.HasValue && _dragSourceIndex.Value == sourceIndex)
        {
            _model.MoveOrMerge(sourceIndex, targetIndex);
        }
        _dragSourceIndex = null;
    }

    private void OnDropOutside(int sourceIndex)
    {
        _dragSourceIndex = null;
    }

    private void OnDeleteClicked()
    {
        if (_selectedIndex < 0) return;
        var slot = _model.GetSlot(_selectedIndex);
        if (slot.IsEmpty) return;

        _model.RemoveAt(_selectedIndex, int.MaxValue);

        OnSlotClicked(-1);
    }

    private void OnUseClicked()
    {
        if (_selectedIndex < 0) return;
        var slot = _model.GetSlot(_selectedIndex);
        if (slot.IsEmpty) return;

        UnityEngine.Debug.Log("Use Item");
    }

}