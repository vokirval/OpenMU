using MUnique.OpenMU.DataModel.Configuration.Items;

internal static class StackRules
{
    /// <summary>
    /// Явный whitelist стакаемых предметов по (Group, Number).
    /// Заполни под себя. Примеры для Jewels 0.75:
    /// Bless  (14,13), Soul (14,14), Chaos (12,15) — проверь соответствие своей версии.
    /// </summary>
    private static readonly HashSet<(byte Group, short Number)> _stackable =
        new()
        {
            (14, 13), // Jewel of Bless
            (14, 14), // Jewel of Soul
            (12, 15), // Jewel of Chaos
        };

    public static bool IsStackable(Item item)
        => item?.Definition is ItemDefinition def && IsStackable(def);

    public static bool IsStackable(ItemDefinition def)
        => _stackable.Contains((def.Group, def.Number));
}
