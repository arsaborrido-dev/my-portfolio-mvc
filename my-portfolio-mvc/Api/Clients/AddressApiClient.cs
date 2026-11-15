using my_portfolio_mvc.Api.Interfaces;
using my_portfolio_mvc.Api.Models;
using my_portfolio_mvc.Api.Models.Address;
using my_portfolio_mvc.Api.Models.Project;
using my_portfolio_mvc.Infrastructure.Http;
using my_portfolio_mvc.ViewModels.Address;
using System.Net;
using System.Security.Claims;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace my_portfolio_mvc.Api.Clients;

public sealed class AddressApiClient : IAddressApiClient
{
    private readonly HttpClient _httpClient;
    public AddressApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<AddressDto?> GetAddressByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync($"api/addresses/{id}", cancellationToken);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new ApiUnauthorizedException();
        }

        response.EnsureSuccessStatusCode();

        var dtoResult = await response.Content.ReadFromJsonAsync<ApiResponse<AddressDto>>(cancellationToken);

        return dtoResult?.Data;

    }

    public async Task<List<AddressDto>> GetAddressesByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync($"api/addresses/user/{userId}", cancellationToken);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new ApiUnauthorizedException();
        }

        response.EnsureSuccessStatusCode();

        var dtoResult = await response.Content.ReadFromJsonAsync<ApiResponse<List<AddressDto>>>(cancellationToken);

        return dtoResult?.Data ?? new List<AddressDto>();

    }

    public async Task<List<AddressDto>> GetAllAddressesAsync(CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync("api/addresses", cancellationToken);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new ApiUnauthorizedException();
        }

        response.EnsureSuccessStatusCode();

        var dtoResult = await response.Content.ReadFromJsonAsync<List<AddressDto>>(cancellationToken);
                
        return dtoResult ?? new List<AddressDto>();

    }
    public async Task<AddressDto> AddAddressAsync(AddressViewModel vm, CancellationToken cancellationToken = default)
    {        

        var request = new AddressDto
        {
            ZipCode = vm.ZipCode,
            AddressLine1 = vm.AddressLine1,
            UserId = vm.UserId
        };

        var response = await _httpClient.PostAsJsonAsync("api/addresses/add", request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new ApiUnauthorizedException();
        }

        response.EnsureSuccessStatusCode();

        var createdAddress = await response.Content.ReadFromJsonAsync<AddressDto>(cancellationToken);

        return createdAddress ?? throw new InvalidOperationException("The API returned an empty response.");

    }

    public async Task<bool> DeleteAddressAsync(long id, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PutAsync($"api/addresses/delete/{id}", content: null, cancellationToken);

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
    public async Task<bool> UpdateAddressAsync(AddressViewModel vm, CancellationToken cancellationToken = default)
    {
        var request = new AddressDto
        {
            Id = vm.Id,
            AddressLine1 = vm.AddressLine1,
            ZipCode = vm.ZipCode
        };

        var response = await _httpClient.PutAsJsonAsync($"api/addresses/update/{vm.Id}", request, cancellationToken);

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
