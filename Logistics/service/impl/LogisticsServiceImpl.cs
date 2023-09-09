using EntityFramework.Context;
using EntityFramework.Models;
using Logistics.exception;


namespace Logistics.service.impl
{
    public class LogisticsServiceImpl : LogisticsService
    {
        private readonly ModelContext _context;
        
        
        private static CancellationTokenSource cancellation;

       
        //定期删除的时间
        static private TimeSpan timespan = TimeSpan.FromHours(1);

        public void setTimespan(double value)
        {
            Timespan = TimeSpan.FromHours(value);
        }

        public TimeSpan getTimespan()
        {
            return Timespan;
        }

        static public TimeSpan Timespan
        {
            get 
            {                
                    return timespan;             
            }
            set
            {                                  
                    if (value <= TimeSpan.Zero)
                        throw new TimeSpanException("time span input error");
                    timespan = value;               
            }
        } 

        public LogisticsServiceImpl(ModelContext context)
        {
            _context = context;
        }

        public async Task<Logisticsinfo> addLogisticsInfo(string id, string place)
        {       
            var info = new Logisticsinfo()
            {
                LogisticsId = id,
                ArrivePlace = place,
                ArriveTime = DateTime.Now
            };

            if(_context.Logisticsinfos.Where(e=> e==info).FirstOrDefault()!=null)
                throw new DuplicateException("same logisticinfo occurred");

            if (_context.Logistics.Where(e => e.LogisticsId == id).Count()==0)
                throw new NotFoundException("no logistic found");

            await _context.Logisticsinfos.AddAsync(info);
            await _context.SaveChangesAsync();
            return info;
        }

        

        //获得所有物流信息
        public IList<Logisticsinfo> getAllLogisticsInfo(string id)
        {           
            IList<Logisticsinfo> result = _context.Logisticsinfos
            .Where(entity => entity.LogisticsId == id).ToList();

            if(result.Count()==0)
                throw new NotFoundException("no logisticsinfo found");
            return result;
        }

        //物流是否到达终点
        public bool arriveDestination(string id)
        {      
            Logistic mylogistic = _context.Logistics.Where(b => b.LogisticsId == id).FirstOrDefault();
            if(mylogistic==null)
                throw new NotFoundException("no logistics found");

            if (mylogistic.EndTime == null)
                return false;
            else
                return true;
        }


        public async Task<Logistic> addLogistics(string company, string address_beg, string address_end)
        {
            string id = Guid.NewGuid().ToString();
            var existLogistic = _context.Logistics.Where(e => e.LogisticsId == id).FirstOrDefault();
            if(existLogistic!=null)//in fact no need
                throw new DuplicateException("same logistic id occurred");

            var info = new Logistic()
            {
                LogisticsId = id,
                StartTime=DateTime.Now,
                Company=company,
                ShipAddress=address_beg,
                PickAddress=address_end
            };
            await _context.Logistics.AddAsync(info);
            await _context.SaveChangesAsync();

            await JustForDemonstrate(id, address_end);
            return info;
        }
        

        public async Task JustForDemonstrate(string id,string address)
        {
            string[] placeList = { "上海青浦中转站", "上海宝山货运码头","菜鸟驿站","顺丰速运站","圆通快递服务区" };
            string firstPlace = "华商异彩";
            // 使用 Fisher-Yates 算法打乱数组顺序
            Random random = new Random();
            for (int i = placeList.Length - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                string temp = placeList[i];
                placeList[i] = placeList[j];
                placeList[j] = temp;
            }

            await addLogisticsInfo(id, firstPlace);
            for (int i = 0; i < 2; i++)
                await addLogisticsInfo(id, placeList[i]);

            Random random2 = new Random();
            int randomNumber = random.Next(0, 100);
            if (randomNumber % 2 == 1)
                await addLogisticsInfo(id, address);


        }




        //清理物流
        public async Task deleteLogistics()
        {
            var now = DateTime.Now;

            
            var end = now - Timespan;

            var info = _context.Logistics
                .Where(e => e.EndTime != null && e.EndTime <= end)
                .ToList();

            if (info.Count() != 0)
            {
                _context.Logistics.RemoveRange(info);
                await _context.SaveChangesAsync();
            }
        }

        public Logistic getLogistics(string id)
        {
            var info = _context.Logistics.Where(e => e.LogisticsId == id).FirstOrDefault();
            if(info==null)
                throw new NotFoundException("cannot find such logistic");
            return info;
        }

        public async Task<Logistic> addArrivalTime(string id)
        {
            var info = _context.Logistics.Where(e => e.LogisticsId == id).FirstOrDefault();
            if(info==null)
                throw new NotFoundException("no logistic find");
            info.EndTime = DateTime.Now;
            await _context.SaveChangesAsync();
            return info;          
        }


        
        public void endRegularClear()
        {
            if (cancellation == null)
                return;
            cancellation.Cancel();
        }
        
        


        public async Task openRegularClear()
        {
            if (cancellation == null || cancellation.IsCancellationRequested)
            {
                //Console.WriteLine("open状态开启");
                cancellation = new CancellationTokenSource();
            }
            else
            {
                //Console.WriteLine("按原来的来");
                return;
            }
                

            try
            {                                              
                while (true)
                {
                    //Console.WriteLine(this.GetHashCode().ToString());
                    CancellationToken token = cancellation.Token;
                    
                    await deleteLogistics();
                    //Console.WriteLine("ccccccc");
                    await Task.Delay(Timespan, token);//token no waiting
                    //Console.WriteLine("aaaaaaaa");
                    token.ThrowIfCancellationRequested();
                   // Console.WriteLine("bbbbbbbb");
                    //if (cancellation.Token.GetHashCode() != hashcode_before)
                    //{
                    //    Console.WriteLine("hashcode更改，close状态开启");
                    //    break;
                    //}
                        
                }
            }
            catch
            {
                //Console.WriteLine("进入close状态");
                ;
            }


                
            
            

            
        }
    }
}
