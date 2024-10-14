namespace EFCore.Identity.WebAPI.Dtos
{
    public sealed record ChangePasswordDto(Guid Id,
        string CurrentPassword,
        string NewPassword);
}
