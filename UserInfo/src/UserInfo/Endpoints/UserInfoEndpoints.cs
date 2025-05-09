using System.ComponentModel.DataAnnotations;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using UserInfo.Endpoints.Contracts;
using UserInfo.Models;
using UserInfo.Services;

namespace UserInfo.Endpoints
{
    public static class UserInfoEndpoints
    {
        // Map all user info endpoints (create, update, delete, get)
        public static IEndpointRouteBuilder MapUserInfoEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("/create", CreateUser);
            app.MapPut("/update {NationalCode}", UpdateUser);
            app.MapDelete("/delete", DeleteUser);
            app.MapGet("/get", GetUser);

            return app;
        }


        public static async Task<IResult> CreateUser(
            [AsParameters] UserInfoServices services,
            CreateUserRequest request,
            IValidator<CreateUserRequest> validator
            )
        {
            var validate = await validator.ValidateAsync(request);
            if (!validate.IsValid)
            {

                var errors = validate.Errors.Select(e => e.ErrorMessage).ToList();
                return Results.BadRequest(errors);
            }

            var existUser = await services.Context.Users.FirstOrDefaultAsync(p => p.NationalCode == request.NationalCode);
            if (existUser != null)
            {
                return Results.BadRequest("This User Already Exist");
            }

            var newUser = User.Create(request.FirstName, request.LastName, request.NationalCode);


            if (newUser is null)
            {
                return Results.BadRequest("Invalid user data");

            }

            services.Context.Users.Add(newUser);
            await services.Context.SaveChangesAsync();

            return Results.Ok(newUser);
        }


        public static async Task<IResult> UpdateUser(
            [AsParameters] UserInfoServices services,
            UpdateUserRequest request,
            string nationalCode,
            IValidator<UpdateUserRequest> validator)
        {
            var validate = await validator.ValidateAsync(request);
            if (!validate.IsValid)
            {

                var errors = validate.Errors.Select(e => e.ErrorMessage).ToList();
                return Results.BadRequest(errors);
            }

            var user = await services.Context.Users.SingleOrDefaultAsync(p=>p.NationalCode==nationalCode);
            if (user is null)
            {
                return Results.NotFound("User not found");
            }

            var isDuplicate=await services.Context.Users
                .AnyAsync(x=>x.NationalCode==nationalCode);

            if (isDuplicate)
                return Results.BadRequest("This NationalCode Already Exist"); 

            user.Update(request.FirstName, request.LastName, request.NationalCode);

            await services.Context.SaveChangesAsync();

            return Results.Ok(user);

        }

        public static async Task<IResult> GetUser(
            [AsParameters] UserInfoServices services
            )
        {
            var users =await services.Context.Users
                .OrderBy(p => p.UserId)
                .Select(p => new GetUserResponse(p.FirstName, p.LastName, p.NationalCode,p.phoneNumber))
                .ToListAsync();

            return Results.Ok(users);
        }


        public static async Task<IResult> DeleteUser(
            [AsParameters] UserInfoServices services,
            int nationalCode)
        {
            var user = await services.Context.Users.FindAsync(nationalCode);
            if (user is null)
            {
                return Results.NotFound("User not found");
            }
            services.Context.Users.Remove(user);
            await services.Context.SaveChangesAsync();
            return Results.Ok("User deleted successfully");
        }
    }
}
