namespace Api.Controllers
{
    using System;
    using System.Linq;

    using Api.DAL.DataServices;
    using Api.IO;
    using Api.Validators;

    using DTO.DTO;

    using Microsoft.AspNetCore.Mvc;

    // todo implement authorization
    public class EstatesController : Controller
    {
        private readonly IDataService<EstateTempDto> dataService;

        private readonly ImageIoService imageIoService;

        private readonly IValidator<EstateTempDto> validator;

        public EstatesController(
            IDataService<EstateTempDto> dataService,
            IValidator<EstateTempDto> validator,
            ImageIoService imageIoService)
        {
            this.dataService = dataService;
            this.validator = validator;
            this.imageIoService = imageIoService;
        }

        [Route("api/estates/{id}")]
        [HttpDelete]
        public IActionResult Delete(string id)
        {
            try
            {
                var images = this.dataService.GetById(id).Images;
                this.imageIoService.DeleteFromDisk(images);

                this.dataService.Delete(id);
                return this.Ok();
            }
            catch (Exception ex)
            {
                return this.StatusCode(500, ex.Message);
            }
        }

        [Route("api/estates")]
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var data = this.dataService.GetAll();

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
                var data = this.dataService.GetFilteredBy(name);

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
        public IActionResult Post([FromBody] EstateTempDto estate)
        {
            try
            {
                var validationErrors = this.validator.Validate(estate);

                if (validationErrors.Any())
                {
                    return this.BadRequest(validationErrors);
                }

                var newId = this.dataService.Create(estate);
                return this.Ok(newId);
            }
            catch (Exception ex)
            {
                return this.StatusCode(500, ex.Message);
            }
        }

        [Route("api/estates")]
        [HttpPut]
        public IActionResult Put([FromBody] EstateTempDto estate)
        {
            try
            {
                var validationErrors = this.validator.Validate(estate);

                if (validationErrors.Any())
                {
                    return this.BadRequest(validationErrors);
                }

                return this.dataService.Update(estate) ? this.Ok() : (IActionResult)this.NotFound();
            }
            catch (Exception ex)
            {
                return this.StatusCode(500, ex.Message);
            }
        }
    }
}