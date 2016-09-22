using System;
using System.Collections.Generic;
using Castle.Components.DictionaryAdapter;
using Gacha.Config;

namespace Gacha.Table
{
    public interface ITable<TKey, TValue>
    {
        Dictionary<TKey, TValue> WholeTable { get; }
        bool IsValid();
    }
}