using System.Reflection;

namespace Product.common
{
    public class BasicSortType
    {
        public virtual List<BasicSortType> getChildren()
        {
            List<BasicSortType> childTypes = new List<BasicSortType>();
            childTypes.Add(new CoatType());
            childTypes.Add(new UnderClothType());
            return childTypes;
        }

        public string getType()
        {
            return "";
        }

        public virtual string getTypeVir()
        {
            return "";
        }

        public static List<string> getAns_aux(BasicSortType x, Type y)
        {
            List<string> ans;
            //y is BasicSortType,then return an empty list
            if (y == typeof(BasicSortType))
                return new List<string> { };
            else
            {
                //recursive
                ans = getAns_aux(x, y.BaseType);
                //get the corresponding method
                MethodInfo? currrentMethod = y.GetMethod("getType");
                string result = (string)currrentMethod.Invoke(x, null);
                ans.Add(result);
                return ans;
            }

        }
        public static string getAns(BasicSortType x)
        {
            return string.Join(',', getAns_aux(x, x.GetType()));
        }

        public static BasicSortType? getFinalType(string type)
        {
            if (type == null)
                return null;
            string[] split = type.Split(',');
            var typeObject = new BasicSortType().getChildren();
            BasicSortType? finalType = null;
            foreach (string item in split)
            {
                foreach (var typeobject in typeObject)
                {
                    Console.WriteLine(typeobject.getTypeVir());
                    if (item == typeobject.getTypeVir())
                    {
                        typeObject = typeobject.getChildren();
                        if (typeObject.Count() == 0)
                            finalType = typeobject;
                        break;
                    }
                }
            }
            return finalType;
        }
    }

    public class CoatType : BasicSortType
    {
        //public new : in order not to override
        public override List<BasicSortType> getChildren()
        {
            var childTypes = new List<BasicSortType>();
            childTypes.Add(new TshirtType());
            childTypes.Add(new JacketType());
            return childTypes;
        }
        public new string getType()
        {
            return "上衣";
        }
        public override string getTypeVir()
        {
            return "上衣";
        }
    }

    public class UnderClothType : BasicSortType
    {
        public override List<BasicSortType> getChildren()
        {
            var childTypes = new List<BasicSortType>();
            childTypes.Add(new PantsType());
            childTypes.Add(new TrousersType());
            return childTypes;
        }
        public new string getType()
        {
            return "下衣";
        }
        public override string getTypeVir()
        {
            return "下衣";
        }
    }

    public class JacketType : CoatType
    {
        public override List<BasicSortType> getChildren()
        {
            var childTypes = new List<BasicSortType>();
            return childTypes;
        }


        public new string getType()
        {
            return "夹克";
        }
        public override string getTypeVir()
        {
            return "夹克";
        }

    }
    public class TshirtType : CoatType
    {
        public override List<BasicSortType> getChildren()
        {
            var childTypes = new List<BasicSortType>();
            return childTypes;
        }
        public new string getType()
        {
            return "T恤";
        }
        public override string getTypeVir()
        {
            return "T恤";
        }
    }

    public class TrousersType : UnderClothType
    {
        public override List<BasicSortType> getChildren()
        {
            var childTypes = new List<BasicSortType>();
            return childTypes;
        }
        public new string getType()
        {
            return "长裤";
        }
        public override string getTypeVir()
        {
            return "长裤";
        }
    }

    public class PantsType : UnderClothType
    {
        public override List<BasicSortType> getChildren()
        {
            var childTypes = new List<BasicSortType>();
            return childTypes;
        }
        public new string getType()
        {
            return "短裤";
        }
        public override string getTypeVir()
        {
            return "短裤";
        }
    }



}
