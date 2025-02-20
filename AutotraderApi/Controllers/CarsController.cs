using AutotraderApi.Models;
using AutotraderApi.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutotraderApi.Controllers
{
    [Route("cars")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        [HttpPost]
        public async Task <ActionResult> AddNewCar(CreateCarDto createCarDto)
        {
            var car = new Car
            {
                Id = Guid.NewGuid(),
                Brand = createCarDto.Brand,
                Type = createCarDto.Type,
                Color = createCarDto.Color,
                Myear = createCarDto.Myear
            };

            using (var context = new AutotraderContext())
            {
                await context.Cars.AddAsync(car);
                await context.SaveChangesAsync();

                return StatusCode(201, new { result = car, message = "Sikeres felvétel." });
            }

        }

        [HttpGet]
        public async Task <ActionResult> GetAllCar()
        {
            using (var context = new AutotraderContext())
            {
                var cars = await context.Cars.ToListAsync();

                if (cars != null)
                {
                    return Ok(new { result = cars, message = "Sikeres lekérdezés." });
                }

                Exception e = new();
                return BadRequest(new { result = "", message = e.Message });

            }
        }

        [HttpGet("ById")]
        public async Task <ActionResult> GetCar(Guid id)
        {
            using (var context = new AutotraderContext())
            {
                var car =  await context.Cars.FirstOrDefaultAsync(x => x.Id == id);

                if (car != null)
                {
                    return Ok(new { result = car, message = "Sikere találat." });
                }

                return NotFound(new { result = "", message = "Nincs ilyen auto az adatbázisban." });
            }

        }

        [HttpDelete]
        public async Task <ActionResult> DeleteCar(Guid id)
        {
            using (var context = new AutotraderContext())
            {
                var car =await context.Cars.FirstOrDefaultAsync(x => x.Id == id);

                if (car != null)
                {
                    context.Cars.Remove(car);
                    context.SaveChanges();

                    return Ok(new { result = car, message = "Sikere törlés." });
                }

                return NotFound(new { result = "", message = "Nincs ilyen auto az adatbázisban." });
            }
        }

        [HttpPut]
        public async Task <ActionResult> UpdateCar(Guid id, UpdateCarDto updateCarDto)
        {
            using (var context = new AutotraderContext())
            {
                var existingCar = await context.Cars.FirstOrDefaultAsync(x => x.Id == id);

                if (existingCar != null)
                {
                    existingCar.Brand = updateCarDto.Brand;
                    existingCar.Type = updateCarDto.Type;
                    existingCar.Color = updateCarDto.Color;
                    existingCar.Myear = updateCarDto.Myear;
                    existingCar.UpdatedTime = DateTime.Now;

                    context.Cars.Update(existingCar);
                    context.SaveChanges();

                    return Ok(new { result = existingCar, message = "Sikere frissítés." });
                }

                return NotFound(new { result = "", message = "Nincs ilyen auto az adatbázisban." });
            }
        }

    }
}
