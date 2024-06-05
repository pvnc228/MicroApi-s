
using PasswordManagerService.Models;

public class PasswordService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public PasswordService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<bool> StorePasswordAsync(PasswordModel passwordModel)
    {
        using var httpClient = _httpClientFactory.CreateClient();
        var response = await httpClient.PostAsJsonAsync("https://passwordmanagerservice/passwords/store", passwordModel);

        return response.IsSuccessStatusCode;
    }
}
