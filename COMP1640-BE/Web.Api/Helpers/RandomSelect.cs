using System;
using System.Collections.Generic;
using System.Linq;

namespace Web.Api.Helpers
{
    public static class RandomSelect
    {
        public static IEnumerable<T> GetRandom<T>(this IEnumerable<T> list, int count)
        {
            if (count <= 0)
                yield break;
            var r = new Random();
            int limit = (count * 10);
            foreach (var item in list.OrderBy(x => r.Next(0, limit)).Take(count))
                yield return item;
        }
    }
}
