using AngleSharp.Io;
using my_portfolio_mvc.Api.Interfaces;
using my_portfolio_mvc.Api.Models.Phone;
using my_portfolio_mvc.Infrastructure.Http;
using my_portfolio_mvc.ViewModels.Phone;
using System.Net;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace my_portfolio_mvc.Api.Clients;

public sealed class PhoneApiClient : IPhoneApiClient
{
    private readonly HttpClient _httpClient;

    public PhoneApiClient(HttpClient httpClient) => _httpClient = httpClient;
    public async Task<PhoneDto> AddPhoneAsync(PhoneViewModel vm, CancellationToken cancellationToken = default)
    {
        var request = new PhoneDto
        {
            Id = vm.Id,
            AreaCode = vm.AreaCode,
            PhoneNumber = vm.PhoneNumber
        };

        var response = await _httpClient.PostAsJsonAsync("api/phones/add", request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new ApiUnauthorizedException();
        }

        response.EnsureSuccessStatusCode();

        var createdPhone = await response.Content.ReadFromJsonAsync<PhoneDto>(cancellationToken);

        return createdPhone ?? throw new InvalidOperationException("Api created the phone but returned an empty response.");

    }

    public async Task<bool> DeletePhoneAsync(long id, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PutAsync($"api/phones/delete/{id}", null, cancellationToken);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new ApiUnauthorizedException();
        }

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return false;
        }

        response.EnsureSuccessStatusCode();

        return true;
    }

    public async Task<List<PhoneDto>> GetAllPhonesAsync(CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync("api/phones", cancellationToken);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new ApiUnauthorizedException();
        }

        response.EnsureSuccessStatusCode();

        var dtoResult = await response.Content.ReadFromJsonAsync<List<PhoneDto>>(cancellationToken);

        return dtoResult ?? new List<PhoneDto>();   
    }

    public async Task<PhoneDto?> GetPhoneByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync($"api/phones/{id}", cancellationToken);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new ApiUnauthorizedException();
        }

        response.EnsureSuccessStatusCode();

        var dtoResult = await response.Content.ReadFromJsonAsync<PhoneDto>(cancellationToken);

        return dtoResult ?? new PhoneDto();
    }

    public async Task<bool> UpdatePhoneAsync(PhoneViewModel vm, CancellationToken cancellationToken = default)
    {
        var phoneDto = new PhoneDto
        {
            Id = vm.Id,
            AreaCode = vm.AreaCode,
            PhoneNumber = vm.PhoneNumber
        };

        var response = await _httpClient.PutAsJsonAsync($"api/phones/update/{vm.Id}", phoneDto, cancellationToken);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new ApiUnauthorizedException();
        }

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return false;
        }

        response.EnsureSuccessStatusCode();

        return true;

    }
}
