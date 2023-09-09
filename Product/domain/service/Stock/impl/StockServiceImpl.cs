using EntityFramework.Context;
using Product.domain.model.repository;
using Product.dto;
using Product.exception;
using Product.utils;

namespace Product.domain.service.Stock.impl
{
    public class StockServiceImpl:StockService
    {
        private readonly ModelContext modelContext;
        public CategoryRepository categoryRepository;
        public StockServiceImpl(ModelContext _modelContext, CategoryRepository _categoryRepository)
        {
            modelContext = _modelContext;
            categoryRepository = _categoryRepository;
        }

        public async Task restoreStock(StockDto restoreStock)
        {
            var pickdto = new PickDto(new PickAuxDto
            {
                PickId = restoreStock.PickId
            });
           

            decimal original_stocks = 0;
            var first_pick = categoryRepository.getPicks(pickdto);

            if (first_pick.Count() == 0)
                throw new NotFoundException("no picks found");


            original_stocks = first_pick[0].First().Stock;


            if (restoreStock.Number <= 0)
                throw new StockException("reduce number should be larger than 0");


            await categoryRepository.setPick(new PickDto(new PickAuxDto
            {
                PickId = restoreStock.PickId,
                Stock = original_stocks + restoreStock.Number,
            }));
           


        }
       


       

        public async Task reduceStock(StockDto reduceStock)
        {
            var pickdto = new PickDto(new PickAuxDto
            {
                PickId = reduceStock.PickId
            });

            decimal original_stocks = 0;
            var first_pick = categoryRepository.getPicks(pickdto);

            if (first_pick.Count() == 0)
                throw new NotFoundException("no picks found");
            
           
            original_stocks = first_pick[0].First().Stock;
            

            if(reduceStock.Number <= 0)
                throw new StockException("reduce number should be larger than 0");

            if (original_stocks < reduceStock.Number)
                throw new StockException("Insufficient stock");

            await categoryRepository.setPick(new PickDto(new PickAuxDto
            {
                PickId = reduceStock.PickId,
                Stock = original_stocks-reduceStock.Number,
            }));
        }

        public async Task LockStock(StockEventDto stockEventDto)
        {
            var stockdto = new StockDto
            {
                PickId = stockEventDto.pickId,
                Number = 1,
            };
            reduceStock(stockdto);
            RabbitMQEventSender sender = new RabbitMQEventSender("stock_delay_queue");
            var stockEvent = new StockLockMessage
            {
                orderId = stockEventDto.orderId,
                pickId = stockEventDto.pickId,
                number = stockEventDto.number,
                isReduced = stockEventDto.isReduced,
            };
            sender.sendDelayedEvent(stockEvent, "stock.locked");
        }

    }
}
