namespace WebApp.Controllers
{
    using System;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;

    using DTO.DTO;
    using DAL.DataServices;
    using DAL.Validators;

    // todo implement authorization 
    public class EstatesController : Controller
    {
        private readonly IDataService<EstateTempDto> dataService;

        private readonly IValidator<EstateTempDto> validator;

        public EstatesController(IDataService<EstateTempDto> dataService, IValidator<EstateTempDto> validator)
        {
            this.dataService = dataService;
            this.validator = validator;
        }

        [Route("api/estates")]
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var data = dataService.GetAll();

                if (data.Any())
                {
                    return this.Ok(data);
                }

                return this.NotFound();
            }
            catch(Exception ex)
            {
                return this.StatusCode(500, ex.Message);
            }
        }

        [Route("api/estates/{name}")]
        [HttpGet]
        public IActionResult Get(string name = null)
        {
            if (string.IsNullOrEmpty(name))
            {
                return this.BadRequest("Name filter can not be empty");
            }

            try
            {
                var data = dataService.GetFilteredBy(name);

                if (data.Any())
                {
                    return this.Ok(data);
                }

                return this.NotFound();
            }
            catch (Exception ex)
            {
                return this.StatusCode(500, ex.Message);
            }
        }

        [Route("api/estates")]
        [HttpPost]
        public IActionResult Post([FromBody]EstateTempDto estate)
        {
            try
            {
                var validationErrors = this.validator.Validate(estate);

                if (validationErrors.Any())
                {
                    return this.BadRequest(validationErrors);
                }

                var newId = dataService.Create(estate);
                return this.Ok(newId);
            }
            catch (Exception ex)
            {
                return this.StatusCode(500, ex.Message);
            }
        }

        [Route("api/estates")]
        [HttpPut]
        public IActionResult Put([FromBody]EstateTempDto estate)
        {
            try
            {
                var validationErrors = this.validator.Validate(estate);

                if (validationErrors.Any())
                {
                    return this.BadRequest(validationErrors);
                }

                return dataService.Update(estate) 
                    ? (IActionResult)this.Ok() 
                    : (IActionResult)this.NotFound();
                
            }
            catch(Exception ex)
            {
                return this.StatusCode(500, ex.Message);
            }
        }

        [Route("api/estates/{id}")]
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            try
            {
                dataService.Delete(id);
                return this.Ok();
            }
            catch(Exception ex)
            {
                return this.StatusCode(500, ex.Message);
            }
        }
    }
}