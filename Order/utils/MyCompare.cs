using EntityFramework.Models;
namespace Product.utils
{
    public class MyCompare
    {
        public class GroupingComparer : IEqualityComparer<IGrouping<string, Pick>>
        {
            public bool Equals(IGrouping<string, Pick> x, IGrouping<string, Pick> y)
            {
                var xPick = new HashSet<Pick>(x, new PickComparer());
                var yPick = new HashSet<Pick>(y, new PickComparer());

                // 判断 xPersons 和 yPersons 是否相等（无序）
                return xPick.SetEquals(yPick);

            }

            public int GetHashCode(IGrouping<string, Pick> obj)
            {
                int hashCode = 1;

                foreach (var pick in obj)
                {
                    if (pick != null)
                    {
                        hashCode ^= pick.PropertyType.GetHashCode() ^ pick.PropertyValue.GetHashCode();
                    }
                }

                return hashCode;
            }

        }

        public class PickComparer : IEqualityComparer<Pick>
        {
            public bool Equals(Pick x, Pick y)
            {
                
                return x.PropertyType == y.PropertyType && x.PropertyValue == y.PropertyValue;

            }

            public int GetHashCode(Pick obj)
            {
                return obj.PropertyType.GetHashCode() ^ obj.PropertyValue.GetHashCode();
            }
        }

        



    }
}
