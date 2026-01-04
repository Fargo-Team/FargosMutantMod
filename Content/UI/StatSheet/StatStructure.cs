using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Terraria;
using Terraria.Localization;

namespace Fargowiltas.Content.UI.StatSheet
{
    public struct PermaUpgrade
    {
        public Item Item;
        public Func<bool> ConsumedBool;

        public PermaUpgrade(Item item, Func<bool> consumedBool)
        {
            Item = item;
            ConsumedBool = consumedBool;
        }
    }

    public class Stat : IComparable<Stat>
    {
        public string Name;
        public string ModName;
        float priority;
        public Func<bool> condition;
        public Func<string> TextFunction;
        public Func<object> Value;

        public Stat(string name, float priority, Func<object> value, Func<string> textFunction, Func<bool> condition = null, string modName = "Fargowiltas")
        {
            this.condition = condition == null ? () => true : condition;
            Name = name;
            ModName = modName;
            TextFunction = textFunction;
            Value = value;
            this.priority = priority;
        }

        public override bool Equals(Object obj)
        {
            if (obj is Stat s)
                return Name.Equals(s.Name);
            return false;
        }

        public int CompareTo(Stat other)
        {
            return priority.CompareTo(other.priority);
        }
    }

    public class StatCategory
    {
        public int priorityMax;
        public string Name;
        public string HeaderLocalPath;
        public Func<bool> Condition;
        public List<Stat> Stats;
        public string iconPath;

        private StatCategory(string key, string headerLocalPath, Func<bool> condition, string iconPath)
        {
            Name = key;
            HeaderLocalPath = headerLocalPath;
            Condition = condition;
            this.iconPath = iconPath;
            Stats = [];
            priorityMax = 1;
        }

        /// <summary>
        /// Attempts a new stat category with the given parameters.
        /// </summary>
        /// <param name="key">Unique category name identifier</param>
        /// <param name="HeaderLocalPath">Localization path of the category header</param>
        /// <param name="iconPath">Filepath of the category's icon</param>
        /// <param name="condition"></param>
        /// <returns>A new <see cref="StatCategory"/> with the given parameters</returns>
        public static StatCategory Create(string key, string HeaderLocalPath, string iconPath = null, Func<bool> condition = null)
        {
            condition ??= (() => true);

            return new StatCategory(key, HeaderLocalPath, condition, iconPath);
        }

        internal StatCategory FargoStat(string key, Func<object> value, string localPath = null, Func<bool> condition = null)
            => AddStat(key, priorityMax, value, $"Mods.Fargowiltas.UI.StatSheet.{key}", condition);

        public StatCategory AddStat(string key, float priority, Func<object> value, string localPath, Func<bool> condition = null)
        {
            condition ??= (() => true);


            Stat newStat = new Stat(key, priority, value, () => Language.GetTextValue(localPath), condition);
            if (!Stats.Contains(newStat))
            {
                Stats.Add(newStat);
                priorityMax++;
            }
            return this;
        }

        public override bool Equals([NotNullWhen(true)] object obj)
        {
            if (obj is StatCategory s)
                return Name.Equals(s.Name);
            return false;
        }
    }

    public static class StatRegistry
    {
        private static bool finalized = false;
        private static Dictionary<string, StatCategory> registry = new Dictionary<string, StatCategory>();

        /// <summary>
        /// Attempts to register the stat category
        /// </summary>
        /// <param name="category"></param>
        /// <returns><see langword="true"/> if the category was registered successfully, <see langword="false"/> otherwise</returns>
        public static bool RegisterCategory(this StatCategory category)
        {
            if (finalized)
                return false;

            if (!registry.ContainsKey(category.Name))
            {
                registry.Add(category.Name, category);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Attempts to add a stat to an existing category.
        /// </summary>
        /// <param name="categoryName"></param>
        /// <param name="statName"></param>
        /// <param name="modName"></param>
        /// <param name="value"></param>
        /// <param name="textFunction"></param>
        /// <param name="priorityOverride"></param>
        /// <returns><see langword="true"/> if the stat was added successfully, <see langword="false"/> otherwise.</returns>
        public static bool TryAddStatToCategory(string categoryKey, string statKey, Func<object> value, Func<string> textFunction, float priorityOverride = -1, string modName = "Fargowiltas")
        {
            if (finalized)
                return false;

            if (registry.TryGetValue(categoryKey, out StatCategory category))
            {
                float p = priorityOverride;
                if (p < 0 || p > category.priorityMax)
                {
                    p = category.priorityMax;
                    category.priorityMax++;
                }

                Stat newStat = new Stat(statKey, p, value, textFunction, modName: modName);
                if (!category.Stats.Contains(newStat))
                {
                    category.Stats.Add(newStat);
                    return true;
                }
            }
            return false;
        }

        public static void FinalizeRegistry()
        {
            if (finalized)
                return;

            finalized = true;

            List<StatCategory> categories = GetCategories();
            foreach (StatCategory category in categories)
            {
                category.Stats.Sort();
            }
        }

        public static StatCategory GetCategory(string key) => registry.GetValueOrDefault(key);

        public static List<StatCategory> GetCategories() => [.. registry.Values];
    }
}
