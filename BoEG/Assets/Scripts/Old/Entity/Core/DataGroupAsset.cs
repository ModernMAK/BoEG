using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Old.Entity.Core
{
    public class DataGroupAsset : ScriptableObject, IDataGroup
    {
        public T GetData<T>()
        {
            foreach (var data in Data)
            {
                if (data is T)
                    return (T) data;
            }
            return default(T);
        }

        protected virtual IEnumerable<object> Data
        {
            get { return Enumerable.Empty<object>(); }
        }
    }
}