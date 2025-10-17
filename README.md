# Unity Inventory — Drag & Drop (MVP, SOLID)

[![Unity](https://img.shields.io/badge/Unity-2021%2B-black)](#)
[![Pattern](https://img.shields.io/badge/Pattern-MVP-blue)](#)
[![Architecture](https://img.shields.io/badge/Architecture-SOLID-success)](#)
[![UI](https://img.shields.io/badge/UI-UGUI-lightgrey)](#)
[![License](https://img.shields.io/badge/License-MIT-green)](#)

Простая, но расширяемая система инвентаря для условной игры. Фокус — **логика, интерфейс, архитектура**. Отделяем UI от бизнес-логики, используем ScriptableObject, поддерживаем стекуемые предметы и drag & drop.

---

## ✨ Возможности

- ✅ Фиксированная сетка слотов
- ✅ Несколько типов предметов
- ✅ Предметы: **название, иконка, описание, тип**, стекуемость (SO)
- ✅ **Drag & Drop** между слотами (merge / swap)
- ✅ **Use** по кнопке **Use**
- ✅ **Drop** кнопкой **Delete**
- ✅ **Tooltip** с описанием при наведении

---

## 🧱 Архитектура (MVP)

```
 Model (чистая логика)     Presenter (координатор)          View (Unity UI)
 ──────────────────────     ───────────────────────          ───────────────
 • Inventory, Slot,         • Слушает View-события           • Рисует и реагирует
   ItemStack, ItemDef       • Вызывает методы Model            на ввод (drag/click)
                            • Готовит данные для View         • Не содержит логики
```

## 📂 Структура проекта

```
Assets/
  Scripts/
    Inventory/
      Domain/              // Model
        ItemDefinition.cs
        ItemStack.cs
        InventorySlot.cs
        IInventory.cs
        Inventory.cs
      Presentation/        // Presenter + контракты View
        SlotRenderData.cs
        IInventoryView.cs
        InventoryPresenter.cs
        IItemUseService.cs        // сервис "использования"
        SimpleItemUseService.cs
      UI/                  // View (Unity UI)
        InventoryInstaller.cs
        InventoryView.cs
        InventorySlotView.cs
        ItemView.cs
        DragCursor.cs
        TooltipView.cs           // (простая реализация, см. ниже)
```

---
## ✅ Поведение по умолчанию

- Перетаскивание на слот того же предмета и не заполненный стек → **merge**.
- Иначе → **swap**.
- Клик по предмету → **Select**.
- Кнопка **Use** активна, если предмет можно использовать (решает `IItemUseService`).
- Кнопка **Delete** активна при непустом слоте → удаляет стек.
- Тултип наводится на иконку предмета.

---

## 🔧 Зависимости

- Unity **2021+**
- uGUI, TextMeshPro

---

## 📜 Лицензия

MIT — используйте в продакшене, Pet-проектах и учебных целях без ограничений.

---

## 🙌 Благодарности

Если эта заготовка помогла, звезда ⭐️.
