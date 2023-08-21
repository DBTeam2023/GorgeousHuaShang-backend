//using System.Reflection;

//namespace Product.common
//{
//    public class BasicSortType
//    {
//        public virtual List<BasicSortType> getChildren()
//        {
//            List<BasicSortType> childTypes = new List<BasicSortType>();
//            childTypes.Add(new CoatType());
//            childTypes.Add(new UnderClothType());
//            return childTypes;
//        }

//        public string getType()
//        {
//            return "";
//        }

//        public virtual string getTypeVir()
//        {
//            return "";
//        }

//        public static List<string> getAns_aux(BasicSortType x, Type y)
//        {
//            List<string> ans;
//            //y is BasicSortType,then return an empty list
//            if (y == typeof(BasicSortType))
//                return new List<string> { };
//            else
//            {
//                //recursive
//                ans = getAns_aux(x, y.BaseType);
//                //get the corresponding method
//                MethodInfo? currrentMethod = y.GetMethod("getType");
//                string result = (string)currrentMethod.Invoke(x, null);
//                ans.Add(result);
//                return ans;
//            }

//        }
//        public static string? getAns(BasicSortType? x)
//        {
//            if (x == null)
//                return null;
//            return string.Join(',', getAns_aux(x, x.GetType()));
//        }

//        public static BasicSortType? getFinalType(string? type)
//        {
//            if (type == null)
//                return null;
//            string[] split = type.Split(',');
//            var typeObject = new BasicSortType().getChildren();
//            BasicSortType? finalType = null;
            
//            foreach (string item in split)
//            {
//                int exit = 0;
//                foreach (var typeobject in typeObject)
//                {
//                    if (item == typeobject.getTypeVir())
//                    {
//                        exit = 1;
//                        typeObject = typeobject.getChildren();
//                        if (typeObject.Count() == 0)
//                            finalType = typeobject;

//                        break;
//                    }
//                }
//                if (exit == 0)//找不到，说明输入有误
//                    return null;
//            }
//            return finalType;
//        }
//    }

   
