using PrintShopServiceDAL.BindingModel;
using PrintShopServiceDAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PrintShopRestApi.Controllers
{
    public class PrintController : ApiController
    {
        private readonly IPrintService _service;
        public PrintController(IPrintService service)
        {
            _service = service;
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

        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            var element = _service.GetElement(id);
            if (element == null)
            {
                InternalServerError(new Exception("Нет данных"));
            }
            return Ok(element);
        }

        [HttpPost]
        public void AddElement(PrintBindingModel model)
        {
            _service.AddElement(model);
        }

        [HttpPost]
        public void UpdElement(PrintBindingModel model)
        {
            _service.UpdElement(model);
        }

        [HttpPost]
        public void DelElement(PrintBindingModel model)
        {
            _service.DelElement(model.Id);
        }
    }
}