using UserManagementApi.Models;
using UserManagementApi.Services;

namespace UserManagementApi.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this WebApplication app) {
        var group = app.MapGroup("/api/users")
               .WithTags("Users")
               .WithOpenApi();

        // GET api/users
        group.MapGet("/", async (IUserService service) => {
            var users = await service.GetAllAsync();
            return Results.Ok(users);
        })
        .WithName("GetAllUsers")
        .Produces<IEnumerable<User>>(StatusCodes.Status200OK)
        .WithSummary("Retrieves all users")
        .WithDescription("Returns a JSON array of all users in the system, including associated Car details");

        // GET api/users/{id}
        group.MapGet("/{id:int}", async (int id, IUserService service) => {
            var user = await service.GetByIdAsync(id);
            return user is not null ? Results.Ok(user) : Results.NotFound();
        })
        .WithName("GetUserById")
        .Produces<User>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithSummary("Retrieves a specific user by ID")
        .WithDescription("Returns the user object (including associated Car) for the given user ID.");

        // POST api/users
        group.MapPost("/", async (User newUser, IUserService service) => {
            var created = await service.CreateAsync(newUser);
            return Results.Created($"/api/users/{created.Id}", created);
        })
        .WithName("CreateUser")
        .Accepts<User>("application/json")
        .Produces<User>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .WithSummary("Creates a new user")
        .WithDescription("Creates a new user with Name, Email, Password, and optional CarId. Returns the created object.");

        // PUT api/users/{id}
        group.MapPut("/{id:int}", async (int id, User updatedUser, IUserService service) => {
            var success = await service.UpdateAsync(id, updatedUser);
            return success ? Results.NoContent() : Results.NotFound();
        })
        .WithName("UpdateUser")
        .Accepts<User>("application/json")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status400BadRequest)
        .WithSummary("Updates an existing user")
        .WithDescription("Updates the fields Name, Email, Password, and CarId for an existing user. Returns 204 if successful or 404 if not found.");

        // DELETE api/users/{id}
        group.MapDelete("/{id:int}", async (int id, IUserService service) => {
            var success = await service.DeleteAsync(id);
            return success ? Results.NoContent() : Results.NotFound();
        })
        .WithName("DeleteUser")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound)
        .WithSummary("Deletes a user by ID")
        .WithDescription("Removes the user with the given ID from the database. Returns 204 if deleted or 404 if not found.");
    }
}
