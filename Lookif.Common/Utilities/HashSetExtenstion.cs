using System.Collections.Generic;
using System.Linq;

namespace Lookif.Library.Common.Utilities;

public static class HashSetExtenstion
{
    public static IEnumerable<(HashSet<T>, HashSet<T>)> To2SubHashSets<T>(this HashSet<T> hashSet)
    {
        foreach (var item in hashSet)
        { 
            var firstLeg = new HashSet<T>();
            var mainItem = item;
            while (true)
            {
                firstLeg.Add(mainItem);
                hashSet.Remove(mainItem);
                mainItem = hashSet.FirstOrDefault();
                if (mainItem is null)
                    break;
                yield return (firstLeg, hashSet);
            }




        }

    }





}