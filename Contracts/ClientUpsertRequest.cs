using System.ComponentModel.DataAnnotations;

namespace Aspnet_server.Contracts;

public sealed class ClientUpsertRequest
{
    public string? Id { get; init; }

    [Required]
    [StringLength(100)]
    public string Name { get; init; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(320)]
    public string Email { get; init; } = string.Empty;
}
