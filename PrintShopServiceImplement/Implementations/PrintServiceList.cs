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
    public class PrintServiceList : IPrintService
    {
        private DataListSingleton source;
        public PrintServiceList()
        {
            source = DataListSingleton.GetInstance();
        }
        public List<PrintViewModel> GetList()
        {
            List<PrintViewModel> result = new List<PrintViewModel>();
            for (int i = 0; i < source.Prints.Count; ++i)
            {
                // требуется дополнительно получить список компонентов для изделия и ихколичество
                List<PrintIngredientViewModel> PrintIngredients = new
    List<PrintIngredientViewModel>();
                for (int j = 0; j < source.PrintIngredient.Count; ++j)
                {
                    if (source.PrintIngredient[j].PrintId == source.Prints[i].Id)
                    {
                        string ingredientName = string.Empty;
                        for (int k = 0; k < source.Ingredients.Count; ++k)
                        {
                            if (source.PrintIngredient[j].PrintId ==
                           source.Ingredients[k].Id)
                            {
                                ingredientName = source.Ingredients[k].IngredientName;
                                break;
                            }
                        }
                        PrintIngredients.Add(new PrintIngredientViewModel
                        {
                            Id = source.PrintIngredient[j].Id,
                            PrintId = source.PrintIngredient[j].PrintId,
                            IngredientId = source.PrintIngredient[j].IngredientId,
                            IngredientName = ingredientName,
                            Count = source.PrintIngredient[j].Count
                        });
                    }
                }
                result.Add(new PrintViewModel
                {
                    Id = source.Prints[i].Id,
                    PrintName = source.Prints[i].PrintName,
                    Price = source.Prints[i].Price,
                    PrintIngredients = PrintIngredients
                });
            }
            return result;
        }
        public PrintViewModel GetElement(int id)
        {
            for (int i = 0; i < source.Prints.Count; ++i)
            {
                // требуется дополнительно получить список компонентов для изделия и их количество
                List<PrintIngredientViewModel> PrintIngredients = new
    List<PrintIngredientViewModel>();
                for (int j = 0; j < source.PrintIngredient.Count; ++j)
                {
                    if (source.PrintIngredient[j].PrintId == source.Prints[i].Id)
                    {
                        string ingredientName = string.Empty;
                        for (int k = 0; k < source.Ingredients.Count; ++k)
                        {
                            if (source.PrintIngredient[j].IngredientId ==
                           source.Ingredients[k].Id)
                            {
                                ingredientName = source.Ingredients[k].IngredientName;
                                break;
                            }
                        }
                        PrintIngredients.Add(new PrintIngredientViewModel
                        {
                            Id = source.PrintIngredient[j].Id,
                            PrintId = source.PrintIngredient[j].PrintId,
                            IngredientId = source.PrintIngredient[j].IngredientId,
                            IngredientName = ingredientName,
                            Count = source.PrintIngredient[j].Count
                        });
                    }
                }
                if (source.Prints[i].Id == id)
                {
                    return new PrintViewModel
                    {
                        Id = source.Prints[i].Id,
                        PrintName = source.Prints[i].PrintName,
                        Price = source.Prints[i].Price,
                        PrintIngredients = PrintIngredients
                    };
                }
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(PrintBindingModel model)
        {
            int maxId = 0;
            for (int i = 0; i < source.Prints.Count; ++i)
            {
                if (source.Prints[i].Id > maxId)
                {
                    maxId = source.Prints[i].Id;
                }
                if (source.Prints[i].PrintName == model.PrintName)
                {
                    throw new Exception("Уже есть изделие с таким названием");
                }
            }
            source.Prints.Add(new Print
            {
                Id = maxId + 1,
                PrintName = model.PrintName,
                Price = model.Price
            });
            // компоненты для изделия
            int maxPCId = 0;
            for (int i = 0; i < source.PrintIngredient.Count; ++i)
            {
                if (source.PrintIngredient[i].Id > maxPCId)
                {
                    maxPCId = source.PrintIngredient[i].Id;
                }
            }
            // убираем дубли по компонентам
            for (int i = 0; i < model.PrintIngredient.Count; ++i)
            {
                for (int j = 1; j < model.PrintIngredient.Count; ++j)
                {
                    if (model.PrintIngredient[i].IngredientId ==
                    model.PrintIngredient[j].IngredientId)
                    {
                        model.PrintIngredient[i].Count +=
                        model.PrintIngredient[j].Count;
                        model.PrintIngredient.RemoveAt(j--);
                    }
                }
            }
            // добавляем компоненты
            for (int i = 0; i < model.PrintIngredient.Count; ++i)
            {
                source.PrintIngredient.Add(new PrintIngredient
                {
                    Id = ++maxPCId,
                    PrintId = maxId + 1,
                    IngredientId = model.PrintIngredient[i].IngredientId,
                    Count = model.PrintIngredient[i].Count
                });
            }
        }
        public void UpdElement(PrintBindingModel model)
        {
            int index = -1;
            for (int i = 0; i < source.Prints.Count; ++i)
            {
                if (source.Prints[i].Id == model.Id)
                {
                    index = i;
                }
                if (source.Prints[i].PrintName == model.PrintName &&
                source.Prints[i].Id != model.Id)
                {
                    throw new Exception("Уже есть изделие с таким названием");
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            source.Prints[index].PrintName = model.PrintName;
            source.Prints[index].Price = model.Price;
            int maxPCId = 0;
            for (int i = 0; i < source.PrintIngredient.Count; ++i)
            {
                if (source.PrintIngredient[i].Id > maxPCId)
                {
                    maxPCId = source.PrintIngredient[i].Id;
                }
            }
            // обновляем существуюущие компоненты
            for (int i = 0; i < source.PrintIngredient.Count; ++i)
            {
                if (source.PrintIngredient[i].PrintId == model.Id)
                {
                    bool flag = true;
                    for (int j = 0; j < model.PrintIngredient.Count; ++j)
                    {
                        // если встретили, то изменяем количество
                        if (source.PrintIngredient[i].Id ==
                       model.PrintIngredient[j].Id)
                        {
                            source.PrintIngredient[i].Count =
                           model.PrintIngredient[j].Count;
                            flag = false;
                            break;
                        }
                    }
                    // если не встретили, то удаляем
                    if (flag)
                    {
                        source.PrintIngredient.RemoveAt(i--);
                    }
                }
            }
            // новые записи
            for (int i = 0; i < model.PrintIngredient.Count; ++i)
            {
                if (model.PrintIngredient[i].Id == 0)
                {
                    // ищем дубли
                    for (int j = 0; j < source.PrintIngredient.Count; ++j)
                    {
                        if (source.PrintIngredient[j].PrintId == model.Id &&
                        source.PrintIngredient[j].IngredientId ==
                       model.PrintIngredient[i].IngredientId)
                        {
                            source.PrintIngredient[j].Count +=
                           model.PrintIngredient[i].Count;
                            model.PrintIngredient[i].Id =
                           source.PrintIngredient[j].Id;
                            break;
                        }
                    }
                    // если не нашли дубли, то новая запись
                    if (model.PrintIngredient[i].Id == 0)
                    {
                        source.PrintIngredient.Add(new PrintIngredient
                        {
                            Id = ++maxPCId,
                            PrintId = model.Id,
                            IngredientId = model.PrintIngredient[i].IngredientId,
                            Count = model.PrintIngredient[i].Count
                        });
                    }
                }
            }
        }
        public void DelElement(int id)
        {
            // удаяем записи по компонентам при удалении изделия
            for (int i = 0; i < source.PrintIngredient.Count; ++i)
            {
                if (source.PrintIngredient[i].PrintId == id)
                {
                    source.PrintIngredient.RemoveAt(i--);
                }
            }
            for (int i = 0; i < source.Prints.Count; ++i)
            {
                if (source.Prints[i].Id == id)
                {
                    source.Prints.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Элемент не найден");
        }
    }
}

