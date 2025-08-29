using BatchIDnumber.Service.Interface;
using DBEntities.Entities;
using Infrastructure.Models;
using Infrastructure.Repository.Interface;

namespace BatchIDnumber.Service.Implement
{
    internal class IDNumberBatchService : IIDNumberBatchService
    {
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// 建置
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name=""></param>
        public IDNumberBatchService(IUnitOfWork unitOfWork) 
        {
            this.unitOfWork = unitOfWork;
        }

        public List<OrdersView> GetOrders(List<string> customerTypes)
        {
            return unitOfWork.OrdersRepository.GetOrders(customerTypes);            
        }

        public void Process (List<OrdersView> ordersViewList)
        {
            MarkDuplicateOrders(ordersViewList);

            foreach (OrdersView order in ordersViewList.Where(o => o.IsSingle))
            {
                ProcessSingleOrders(order);
            }

            foreach (IGrouping<string, OrdersView> group in ordersViewList.Where(o => !o.IsSingle).GroupBy(o => o.IDNumber))
            {                
                int idnumberCount = 1;
                foreach (var order in group)
                {                    
                    ProcessDuplicateOrder(order, ++idnumberCount);
                }
            }             
        }

        /// <summary>
        /// Orders的IDNumber只有一筆帳號時所做的批次處理
        /// </summary>
        /// <param name="ordersView"></param>
        private void ProcessSingleOrders(OrdersView ordersView)
        {
            CopyParam copyParam = new()
            {
                NewIDNumber = $"{ordersView.IDNumber}{"~1"}",
                IDNumber = ordersView.IDNumber
            };

            UpdateOrders updateOrders = new()
            {
                AccNo = ordersView.AccNo,
                IDNumber = copyParam.IDNumber,
                NewIDNumber = copyParam.NewIDNumber
            };

            UpdateCombinid updateCombinid = new()
            {
                AccNo = ordersView.AccNo,
                IDNumber = copyParam.IDNumber,
                NewIDNumber = copyParam.NewIDNumber
            };

            unitOfWork.CustomerDataRepository.CopyWithNewId(copyParam);
            unitOfWork.CustomerDataRepository.Delete(copyParam);
            unitOfWork.OrdersRepository.Update(updateOrders);
            unitOfWork.CombinidRepository.Update(updateCombinid);
            unitOfWork.PhotoRepository.UpdateIDNumber(copyParam);
            unitOfWork.IdentifycardRepository.UpdateIDNumber(copyParam);
            unitOfWork.OldPhotoRepository.UpdateIDNumber(copyParam);
            unitOfWork.OldIdentifycardRepository.UpdateIDNumber(copyParam);
        }

        /// <summary>
        /// Orders的IDNumber有多筆帳號時所做的批次處理
        /// </summary>
        /// <param name="ordersView"></param>
        /// <param name="idNumberCount"></param>
        private void ProcessDuplicateOrder(OrdersView ordersView, int idNumberCount)
        {
            CopyParam copyParam = new()
            {
                NewIDNumber = $"{ordersView.IDNumber}{"~"}{idNumberCount}",
                IDNumber = ordersView.IDNumber
            };

            Orders newOrders = new()
            {
                AccNo = ordersView.AccNo,
                IDNumber = copyParam.NewIDNumber
            };

            Combinid combinid = new()
            {
                AccNo = ordersView.AccNo,
                IDNumber = copyParam.NewIDNumber
            };

            List<OldPhoto> oldPhotos = unitOfWork.OldPhotoRepository.GetOldPhotos(copyParam.IDNumber);
            List<OldIdentifycard> oldIdentifycards = unitOfWork.OldIdentifycardRepository.GetOldIdentifycards(copyParam.IDNumber);

            unitOfWork.CustomerDataRepository.CopyWithNewId(copyParam);
            unitOfWork.OrdersRepository.Insert(newOrders);
            unitOfWork.CombinidRepository.Insert(combinid);
            unitOfWork.PhotoRepository.InsertCopy(copyParam);
            unitOfWork.IdentifycardRepository.InsertCopy(copyParam);
            unitOfWork.OldPhotoRepository.Inserts(oldPhotos, copyParam.NewIDNumber);
            unitOfWork.OldIdentifycardRepository.Inserts(oldIdentifycards, copyParam.NewIDNumber);

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
