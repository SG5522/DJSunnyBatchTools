using BatchIDnumber.Models;
using BatchIDnumber.Service.Interface;
using DBEntities.Entities;
using Infrastructure.Models;
using Infrastructure.Repository.Interface;
using Microsoft.Extensions.Logging;

namespace BatchIDnumber.Service.Implement
{
    internal class IDNumberBatchService : IIDNumberBatchService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<IDNumberBatchService> logger;

        /// <summary>
        /// 建置
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="">logger</param>
        public IDNumberBatchService(IUnitOfWork unitOfWork, ILogger<IDNumberBatchService> logger) 
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        public List<OrdersView> GetOrders(List<string> customerTypes)
        {
            return unitOfWork.OrdersRepository.GetOrders(customerTypes);            
        }

        /// <summary>
        /// 批次處理
        /// </summary>
        /// <param name="ordersViewList"></param>
        public void Process(List<OrdersView> ordersViewList)
        {
            try
            {
                logger.LogInformation("============開始批次處理===========");
                MarkDuplicateOrders(ordersViewList);

                foreach (OrdersView ordersView in ordersViewList.Where(o => o.IsSingle))
                {
                     ReportViewModel reportViewModel = ProcessSingleOrders(ordersView);
                    //logger.LogInformation(" 帳號：{ordersView.AccNo} 統編：{ordersView.IDNumber} 變更的統編：{copyParam.NewIDNumber}",
                    //                    reportViewModel.CopyParam.AccNo, reportViewModel.CopyParam.IDNumber, reportViewModel.CopyParam.NewIDNumber);
                    logger.LogInformation("{@ReportViewModel}", reportViewModel);
                }

                foreach (IGrouping<string, OrdersView> group in ordersViewList.Where(o => !o.IsSingle).GroupBy(o => o.IDNumber))
                {
                    int idnumberCount = 1;
                    foreach (OrdersView order in group)
                    {
                        ReportViewModel reportViewModel = ProcessDuplicateOrder(order, ++idnumberCount);
                        logger.LogInformation("{@ReportViewModel}", reportViewModel);
                    }
                }

                unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                logger.LogError($"Process Error {ex.Message}");
                unitOfWork.Rollback();
            }            
        }

        /// <summary>
        /// Orders的IDNumber只有一筆帳號時所做的批次處理
        /// </summary>
        /// <param name="ordersView"></param>
        private ReportViewModel ProcessSingleOrders(OrdersView ordersView)
        {
            ReportViewModel result = new();

            CopyParam copyParam = new()
            {
                AccNo = ordersView.AccNo,
                IDNumber = ordersView.IDNumber,
                NewIDNumber = $"{ordersView.IDNumber}{"~1"}"
            };

            try
            {                
                result.CopyParam = copyParam;
                result.InsertCustomerDataCount = unitOfWork.CustomerDataRepository.CopyWithNewId(copyParam);
                result.DeleteCustomerDataCount = unitOfWork.CustomerDataRepository.Delete(copyParam.IDNumber);
                result.UpdateOrdersCount = unitOfWork.OrdersRepository.Update(copyParam);
                result.UpdateCombinidCount = unitOfWork.CombinidRepository.Update(copyParam);
                result.UpdatePhotoCount = unitOfWork.PhotoRepository.UpdateIDNumber(copyParam);
                result.UpdateIdentifycardCount = unitOfWork.IdentifycardRepository.UpdateIDNumber(copyParam);
                result.UpdateOldPhotoCount = unitOfWork.OldPhotoRepository.UpdateIDNumber(copyParam);
                result.UpdateOldIdentifycardCount = unitOfWork.OldIdentifycardRepository.UpdateIDNumber(copyParam);
            }
            catch (Exception ex)
            {                
                logger.LogError("帳號：{@AccNo} 處理失敗，處理錯誤 {@ErrorMessage} ", ordersView.AccNo, ex.Message);
            }                     
            
            return result;
        }

        /// <summary>
        /// Orders的IDNumber有多筆帳號時所做的批次處理
        /// </summary>
        /// <param name="ordersView"></param>
        /// <param name="idNumberCount"></param>
        private ReportViewModel ProcessDuplicateOrder(OrdersView ordersView, int idNumberCount)
        {
            ReportViewModel result = new();

            CopyParam copyParam = new()
            {
                AccNo = ordersView.AccNo,
                IDNumber = ordersView.IDNumber,
                NewIDNumber = $"{ordersView.IDNumber}{"~"}{idNumberCount}"
            };

            try
            {
                List<OldPhoto> oldPhotos = unitOfWork.OldPhotoRepository.GetOldPhotos(copyParam.IDNumber);
                List<OldIdentifycard> oldIdentifycards = unitOfWork.OldIdentifycardRepository.GetOldIdentifycards(copyParam.IDNumber);

                result.CopyParam = copyParam;
                result.InsertCustomerDataCount = unitOfWork.CustomerDataRepository.CopyWithNewId(copyParam);
                result.InsertOrdersCount = unitOfWork.OrdersRepository.InsertNewIDNumber(copyParam);
                result.InsertCombinidCount = unitOfWork.CombinidRepository.InsertCopy(copyParam);
                result.InsertPhotoCount = unitOfWork.PhotoRepository.InsertCopy(copyParam);
                result.InsertIdentifycardCount = unitOfWork.IdentifycardRepository.InsertCopy(copyParam);
                result.InsertOldPhotoCount = unitOfWork.OldPhotoRepository.Inserts(oldPhotos, copyParam.NewIDNumber);
                result.InsertOldIdentifycardCount = unitOfWork.OldIdentifycardRepository.Inserts(oldIdentifycards, copyParam.NewIDNumber);
            }
            catch (Exception ex)
            {
                logger.LogError("帳號：{@AccNo} 處理失敗，處理錯誤 {@ErrorMessage}", ordersView.AccNo, ex.Message);
            }

            return result;
        }
        
        /// <summary>
        /// 處理Orders清單，並標記其中的重複項目。
        /// </summary>
        /// <param name="partialOrders">需要處理的訂單清單。</param>
        private void MarkDuplicateOrders(List<OrdersView> partialOrders)
        {
            List<string> uniqueIds = partialOrders.Select(o => o.IDNumber).Distinct().ToList();

            List<string> duplicateIds = unitOfWork.OrdersRepository.GetDuplicateOrderIds(uniqueIds);

            HashSet<string> duplicateIdsSet = new(duplicateIds);

            foreach (var order in partialOrders)
            {
                order.IsSingle = !duplicateIdsSet.Contains(order.IDNumber);
            }
        }
    }
}
