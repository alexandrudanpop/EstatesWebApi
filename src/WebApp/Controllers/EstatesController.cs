namespace WebApp.Controllers
{
    using System;
    using Microsoft.AspNetCore.Mvc;
    using DAL;
    using Model;
    using System.Linq;

    /// <summary>
    /// The estates controller.
    /// </summary>
    public class EstatesController : Controller
    {
        private readonly IRepository repository;

        public EstatesController(IRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// The get.
        /// </summary>
        /// <returns>
        /// The <see cref="IActionResult"/>.
        /// </returns>
        [Route("api/estates")]
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var data = this.repository.GetEntities<Estate>()
                                .Select(x => new EstateTempDto(x.Id, x.Title, x.Price))
                                .ToList();

                if (data.Any())
                {
                    return this.Ok(data);
                }

                return this.NotFound();
            }
            catch(Exception ex)
            {
                return this.Ok(ex.Message);
            }
        }

        [Route("api/estates")]
        [HttpPost]
        public IActionResult Post([FromBody]EstateTempDto estate)
        {
            try
            {
                var newEstate = new Estate
                {
                    Title = estate.Name,
                    Price = estate.Price
                };

                this.repository.Add<Estate>(newEstate);
                this.repository.SaveChanges();

                var newId = this.repository.GetEntities<Estate>()
                            .Where(e => e.Title == newEstate.Title).FirstOrDefault()?.Id;

                return this.Ok(newId);
            }
            catch (Exception ex)
            {

                return this.Ok(ex.Message);
            }
        }

        [Route("api/estates")]
        [HttpPut]
        public IActionResult Put([FromBody]EstateTempDto estate)
        {
            try
            {
                var editedEstate = this.repository.GetEntities<Estate>()
                                                  .Where(e => e.Id == estate.Id)
                                                  .FirstOrDefault();
                if (editedEstate == null){
                    return this.NotFound();
                }

                editedEstate.Title = estate.Name;
                editedEstate.Price = estate.Price;

                this.repository.SaveChanges();
                return this.Ok();
            }
            catch(Exception ex)
            {
                return this.Ok(ex.Message);
            }
        }

        [Route("api/estates/{id}")]
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            try
            {
                var estateToDelete = this.repository.GetEntities<Estate>()
                                        .Where(e => e.Id == id)
                                        .FirstOrDefault();
                
                if (estateToDelete == null)
                {
                    return this.Ok("Data already deleted");
                }

                this.repository.Delete<Estate>(estateToDelete);
                this.repository.SaveChanges();                       
                return this.Ok();
            }
            catch(Exception ex)
            {
                return this.Ok(ex.Message);
            }
        }
    }

    public class EstateTempDto
    {
        public EstateTempDto(int id, string name, int price)
        {
            this.Id = id;
            this.Name = name;
            this.Price = price;
        }

        public int Id { get; }
        public string Name { get; }
        public int Price { get; }
    }
}