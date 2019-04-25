using PrintShopModel;
using PrintShopServiceDAL.BindingModel;
using PrintShopServiceDAL.Interfaces;
using PrintShopServiceDAL.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintShopServiceImplementDataBase.Implementations
{
    public class IngredientServiceDB : IIngredientService
    {
        private PrintShopDbContext context;
        public IngredientServiceDB(PrintShopDbContext context)
        {
            this.context = context;
        }
        public List<IngredientViewModel> GetList()
        {
            List<IngredientViewModel> result = context.Ingredients.Select(rec => new IngredientViewModel
            {
                Id = rec.Id,
                IngredientName = rec.IngredientName
            })
            .ToList();
            return result;
        }
        public IngredientViewModel GetElement(int id)
        {
            Ingredient element = context.Ingredients.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new IngredientViewModel
                {
                    Id = element.Id,
                    IngredientName = element.IngredientName
                };
            }
            throw new Exception("Элемент не найден");
        }
        public void AddElement(IngredientBindingModel model)
        {
            Ingredient element = context.Ingredients.FirstOrDefault(rec => rec.IngredientName == model.IngredientName);
            if (element != null)
            {
                throw new Exception("Уже есть клиент с таким ФИО");
            }
            context.Ingredients.Add(new Ingredient
            {
                IngredientName = model.IngredientName
            });
            context.SaveChanges();
        }
        public void UpdElement(IngredientBindingModel model)
        {
            Ingredient element = context.Ingredients.FirstOrDefault(rec => rec.IngredientName == model.IngredientName && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть клиент с таким ФИО");
            }
            element = context.Ingredients.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.IngredientName = model.IngredientName;
            context.SaveChanges();
        }
        public void DelElement(int id)
        {
            Ingredient element = context.Ingredients.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                context.Ingredients.Remove(element);
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
}