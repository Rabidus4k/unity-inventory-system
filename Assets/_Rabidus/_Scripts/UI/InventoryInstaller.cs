using System.Collections.Generic;
using UnityEngine;

public class InventoryInstaller : MonoBehaviour
{
    [Header("View")]
    [SerializeField] private InventoryView view;

    [Header("Config")]
    [SerializeField] private int slots = 24;
    [SerializeField] private List<InventoryInfo> initialItems;

    private InventoryPresenter _presenter;

    private void Awake()
    {
        var model = new Inventory(Mathf.Max(1, slots));

        foreach (var item in initialItems)
        {
            model.Add(item.ItemDefinition, item.Count, true);
        }

        IItemUseService useService = new UseItemService();
        IItemUseService deleteService = new DeteleItemService();
        IItemUseService contextService = new ContextItemService();

        _presenter = new InventoryPresenter(model, view, useService, deleteService, contextService);
        _presenter.Initialize();
    }
}