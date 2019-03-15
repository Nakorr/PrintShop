using PrintShopModel;
using PrintShopServiceDAL.BindingModel;
using PrintShopServiceDAL.Interfaces;
using PrintShopServiceDAL.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintShopServiceImplement.Implementations
{
    public class MainServiceList : IMainService
    {
        private DataListSingleton source;
        public MainServiceList()
        {
            source = DataListSingleton.GetInstance();
        }
        public List<IndentViewModel> GetList()
        {
            List<IndentViewModel> result = new List<IndentViewModel>();
            for (int i = 0; i < source.Indents.Count; ++i)
            {
                string clientFIO = string.Empty;
                for (int j = 0; j < source.Customers.Count; ++j)
                {
                    if (source.Customers[j].Id == source.Indents[i].CustomerId)
                    {
                        clientFIO = source.Customers[j].CustomerFIO;
                        break;
                    }
                }
                string productName = string.Empty;
                for (int j = 0; j < source.Prints.Count; ++j)
                {
                    if (source.Prints[j].Id == source.Indents[i].PrintId)
                    {
                        productName = source.Prints[j].PrintName;
                        break;
                    }
                }
                result.Add(new IndentViewModel
                {
                    Id = source.Indents[i].Id,
                    CustomerId = source.Indents[i].CustomerId,
                    CustomerFIO = clientFIO,
                    PrintId = source.Indents[i].PrintId,
                    PrintName = productName,
                    Count = source.Indents[i].Count,
                    Sum = source.Indents[i].Sum,
                    DateCreate = source.Indents[i].DateCreate.ToLongDateString(),
                    DateImplement = source.Indents[i].DateImplement?.ToLongDateString(),
                    Status = source.Indents[i].Status.ToString()
                });
            }
            return result;
        }
        public void CreateIndent(IndentBindingModel model)
        {
            int maxId = 0;
            for (int i = 0; i < source.Indents.Count; ++i)
            {
                if (source.Indents[i].Id > maxId)
                {
                    maxId = source.Customers[i].Id;
                }
            }
            source.Indents.Add(new Indent
            {
                Id = maxId + 1,
                CustomerId = model.CustomerId,
                PrintId = model.PrintId,
                DateCreate = DateTime.Now,
                Count = model.Count,
                Sum = model.Sum,
                Status = IndentStatus.Принят
            });
        }
        public void TakeOrderInWork(IndentBindingModel model)
        {
            int index = -1;
            for (int i = 0; i < source.Indents.Count; ++i)
            {
                if (source.Indents[i].Id == model.Id)
                {
                    index = i;
                    break;
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            if (source.Indents[index].Status != IndentStatus.Принят)
            {
                throw new Exception("Заказ не в статусе \"Принят\"");
            }
            source.Indents[index].DateImplement = DateTime.Now;
            source.Indents[index].Status = IndentStatus.Выполняется;
        }
        public void FinishOrder(IndentBindingModel model)
        {
            int index = -1;
            for (int i = 0; i < source.Indents.Count; ++i)
            {
                if (source.Indents[i].Id == model.Id)
                {
                    index = i;
                    break;
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            if (source.Indents[index].Status != IndentStatus.Выполняется)
            {
                throw new Exception("Заказ не в статусе \"Выполняется\"");
            }
            source.Indents[index].Status = IndentStatus.Готов;
        }
        public void PayOrder(IndentBindingModel model)
        {
            int index = -1;
            for (int i = 0; i < source.Indents.Count; ++i)
            {
                if (source.Indents[i].Id == model.Id)
                {
                    index = i;
                    break;
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            if (source.Indents[index].Status != IndentStatus.Готов)
            {
                throw new Exception("Заказ не в статусе \"Готов\"");
            }
            source.Indents[index].Status = IndentStatus.Оплачен;
        }
    }
}
