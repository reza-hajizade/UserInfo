namespace UserInfo.Models;

public class User
{
    public int UserId { get;  set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string NationalCode { get; private set; }

    public string? phoneNumber { get; private set; } 

    public static User Create(string firstName, string lastName, string nationalCode)
    {
        return new User
        {
            FirstName = firstName,
            LastName = lastName,
            NationalCode = nationalCode
        };
    }

    public void Update(string firstName,string lastName, string nationalCode)
    {
        FirstName = firstName;
        LastName = lastName;    
        NationalCode = nationalCode;
    }



    public void CreateContactUserMessage(string PhoneNumber)
    {
        phoneNumber = PhoneNumber;
    }
}