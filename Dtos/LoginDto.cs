namespace EFCore.Identity.WebAPI.Dtos
{
    public sealed record LoginDto(string UserNameOrEmail, string Password);
}
