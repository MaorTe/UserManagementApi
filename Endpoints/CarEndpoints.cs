using UserManagementApi.Models;
using UserManagementApi.Services;

namespace UserManagementApi.Endpoints;

public static class CarEndpoints
{
    public static void MapCarEndpoints(this WebApplication app) {
        var group = app.MapGroup("/api/cars")
                .WithTags("Cars")                                   
                .WithOpenApi();                                     

        // GET api/cars
        group.MapGet("/", async (ICarService service) => {
            var cars = await service.GetAllAsync();
            return Results.Ok(cars);
        })
        .WithName("GetAllCars")
        .Produces<IEnumerable<Car>>(StatusCodes.Status200OK)   
        .WithSummary("Retrieves all cars")
        .WithDescription("Returns a JSON array of all cars in the system.");


        // GET api/cars/{id}
        group.MapGet("/{id:int}", async (int id, ICarService service) => {
            var car = await service.GetByIdAsync(id);
            return car is not null ? Results.Ok(car) : Results.NotFound();
        })
        .WithName("GetCarById")
        .Produces<Car>(StatusCodes.Status200OK)                
        .Produces(StatusCodes.Status404NotFound)               
        .WithSummary("Retrieves a specific car by ID")
        .WithDescription("Returns the car object for the given ID, including CreatedAt and UpdatedAt.");


        // POST api/cars
        group.MapPost("/", async (Car newCar, ICarService service) => {
            var created = await service.CreateAsync(newCar);
            return Results.Created($"/api/cars/{created.Id}", created);
        })
        .WithName("CreateCar")
        .Accepts<Car>("application/json")                       
        .Produces<Car>(StatusCodes.Status201Created)            
        .Produces(StatusCodes.Status400BadRequest)              
        .WithSummary("Creates a new car")
        .WithDescription("Creates a new car with Company and Model. Returns the created Car with timestamps.");


        // PUT api/cars/{id}
        group.MapPut("/{id:int}", async (int id, Car updatedCar, ICarService service) => {
            var success = await service.UpdateAsync(id, updatedCar);
            return success ? Results.NoContent() : Results.NotFound();
        })
        .WithName("UpdateCar")
        .Accepts<Car>("application/json")                       
        .Produces(StatusCodes.Status204NoContent)                
        .Produces(StatusCodes.Status404NotFound)                 
        .Produces(StatusCodes.Status400BadRequest)               
        .WithSummary("Updates an existing car")
        .WithDescription("Updates the Company, Model and UpdatedAt for an existing car. Returns 204 on success or 404 if not found.");


        // DELETE api/cars/{id}
        group.MapDelete("/{id:int}", async (int id, ICarService service) => {
            var success = await service.DeleteAsync(id);
            return success ? Results.NoContent() : Results.NotFound();
        })
        .WithName("DeleteCar")
        .Produces(StatusCodes.Status204NoContent)                
        .Produces(StatusCodes.Status404NotFound)                 
        .WithSummary("Deletes a car by ID")
        .WithDescription("Removes the car with the given ID from the database. Returns 204 on success or 404 if not found.");
    }
}
