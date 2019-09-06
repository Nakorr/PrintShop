using PrintShopServiceDAL.BindingModel;
using PrintShopServiceDAL.ViewModel;
using PrintShopServiceDAL.Interfaces;
using PrintShopRestApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PrintShopRestApi.Controllers
{
    public class MainController : ApiController
    {
        private readonly IMainService _service;
        private readonly IImplementerService _serviceImplementer;
        public MainController(IMainService service, IImplementerService
       serviceImplementer)
        {
            _service = service;
            _serviceImplementer = serviceImplementer;
        }

        [HttpGet]
        public IHttpActionResult GetList()
        {
            var list = _service.GetList();
            if (list == null)
            {
                InternalServerError(new Exception("Нет данных"));
            }
            return Ok(list);
        }

        [HttpPost]
        public void FinishIndent(IndentBindingModel model)
        {
            _service.FinishIndent(model);
        }

        [HttpPost]
        public void CreateIndent(IndentBindingModel model)
        {
            _service.CreateIndent(model);
        }

        [HttpPost]
        public void PayIndent(IndentBindingModel model)
        {
            _service.PayIndent(model);
        }

        [HttpPost]
        public void PutIngredientOnStock(StockIngredientBindingModel model)
        {
            _service.PutIngredientOnStock(model);
        }

        [HttpPost]
        public void StartWork()
        {
            List<IndentViewModel> orders = _service.GetFreeIndents();
            foreach (var order in orders)
            {
                ImplementerViewModel impl = _serviceImplementer.GetFreeWorker();
                if (impl == null)
                {
                    throw new Exception("Нет сотрудников");
                }
                new WorkImplementer(_service, _serviceImplementer, impl.Id, order.Id);
            }
            orders = _service.GetFreeIndents();
        }
        [HttpGet]
        public IHttpActionResult GetInfo() { ReflectionService service = new ReflectionService(); var list = service.GetInfoByAssembly(); if (list == null) { InternalServerError(new Exception("Нет данных")); } return Ok(list); }
    }
}