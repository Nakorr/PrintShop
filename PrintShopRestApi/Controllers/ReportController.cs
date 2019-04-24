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
    public class ReportController : ApiController
    {
        private readonly IReportService _service;
        public ReportController(IReportService service)
        {
            _service = service;
        }
        [HttpGet]
        public IHttpActionResult GetStocksLoad()
        {
            var list = _service.GetStocksLoad();
            if (list == null)
            {
                InternalServerError(new Exception("Нет данных"));
            }
            return Ok(list);
        }
        [HttpPost]
        public IHttpActionResult GetCustomerIndents(ReportBindingModel model)
        {
            var list = _service.GetCustomerIndents(model);
            if (list == null)
            {
                InternalServerError(new Exception("Нет данных"));

            }
            return Ok(list);
        }
        [HttpPost]
        public void SavePrintPrice(ReportBindingModel model)
        {
            _service.SavePrintPrice(model);
        }
        [HttpPost]
        public void SaveStocksLoad(ReportBindingModel model)
        {
            _service.SaveStocksLoad(model);
        }
        [HttpPost]
        public void SaveCustomerIndents(ReportBindingModel model)
        {
            _service.SaveCustomerIndents(model);
        }
    }
}