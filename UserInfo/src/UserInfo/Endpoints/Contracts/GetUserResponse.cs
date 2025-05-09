using UserInfo.Models;

namespace UserInfo.Endpoints.Contracts;

    public sealed record GetUserResponse(
        string firstName,
        string lastName,
        string nationalCode,
        string phoneNumber);
    

