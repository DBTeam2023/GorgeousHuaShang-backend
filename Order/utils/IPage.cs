using Order.exception;

namespace Order.utils
{
    /**
     * @author sty
     * 
     * template data structure designed for page querying result
     * 
     * @implnote params records, total, size, current are required for building page
     * @param Records page querying records
     * @param Total      total number of data in database
     * @param Size       page size required
     * @param RecordNum  number of elements in page querying record
     * @param Current    current page num
     * @param Pages      total page in database
     */
    public class IPage<RecordType>
    {
        public List<RecordType> Records { get; set; }
        public int Total { get; set; } = -1;
        public int Size { get; set; } = -1;
        public int RecordNum { get; set; }
        public int Current { get; set; } = -1;
        public int Pages { get; set; }

        public static IPageBuilder<RecordType> builder()
        {
            return new IPageBuilder<RecordType>();
        }

        protected IPage(List<RecordType> records, int total, int size, int current, int pages)
        {
            Records = records;
            Total = total;
            Size = size;
            Current = current;
            Pages = pages;
        }

        protected IPage() { }

        public class IPageBuilder<T>
        {
            private IPage<T> builder;

            public IPageBuilder()
            {
                builder = new IPage<T>();
            }

            public IPageBuilder<T> records(List<T> items)
            {
                builder.Records = items;
                return this;
            }

            public IPageBuilder<T> total(int total)
            {
                builder.Total = total;
                return this;
            }

            public IPageBuilder<T> size(int size)
            {
                builder.Size = size;
                return this;
            }

            public IPageBuilder<T> current(int current)
            {
                builder.Current = current;
                return this;
            }

            public IPage<T> build()
            {
                //check builder status
                check();

                //record count
                builder.RecordNum = builder.Records.Count;

                //calculate pages
                builder.Pages = builder.Total / builder.Size + 1;

                return builder;
            }

            private void check()
            {
                if (builder == null) throw new IncompleteBuildException("null builder");

                if (builder.Records == null) throw new IncompleteBuildException("null records");
                if (builder.Size == -1) throw new IncompleteBuildException("parameter size missing");
                if (builder.Total == -1)   throw new IncompleteBuildException("parameter total missing");
                if (builder.Current == -1) throw new IncompleteBuildException("parameter current missing");
            }
        }
    }
}
