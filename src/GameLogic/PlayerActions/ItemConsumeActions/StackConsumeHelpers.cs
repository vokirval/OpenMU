using MUnique.OpenMU.GameLogic.Views.Inventory;

namespace MUnique.OpenMU.GameLogic.PlayerActions.ItemConsumeActions;

internal static class StackConsumeHelpers
{
    /// <summary>
    /// Корректно расходует 1 шт. из стака (или удаляет предмет, если он был 1).
    /// Также сбрасывает курсор у клиента и присылает нужные события, чтобы предмет не "исчезал".
    /// </summary>
    public static async ValueTask ConsumeOneFromStackAsync(
        Player player,
        Item consumable,
        IStorage storageHoldingConsumable,
        bool clearCursorFirst = true)
    {
        if (clearCursorFirst)
        {
            // Снять предмет с курсора — иначе "залипнет"
            await player.InvokeViewPlugInAsync<IItemMoveFailedPlugIn>(p => p.ItemMoveFailedAsync(consumable)).ConfigureAwait(false);
        }

        if (StackRules.IsStackable(consumable) && consumable.Durability > 1)
        {
            // Просто уменьшаем количество
            consumable.Durability -= 1;

            // Некоторые клиенты при use локально скрывают предмет. Форсим "предмет есть" и затем число.
            await player.InvokeViewPlugInAsync<IItemAppearPlugIn>(p => p.ItemAppearAsync(consumable)).ConfigureAwait(false);
            await player.InvokeViewPlugInAsync<IItemDurabilityChangedPlugIn>(p => p.ItemDurabilityChangedAsync(consumable, false)).ConfigureAwait(false);
            return;
        }

        // Было 1 — удаляем предмет
        await storageHoldingConsumable.RemoveItemAsync(consumable).ConfigureAwait(false);
        await player.InvokeViewPlugInAsync<IItemRemovedPlugIn>(p => p.RemoveItemAsync(consumable.ItemSlot)).ConfigureAwait(false);
    }
}
