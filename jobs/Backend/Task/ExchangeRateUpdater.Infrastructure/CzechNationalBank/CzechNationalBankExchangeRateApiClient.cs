﻿using ExchangeRateUpdater.Domain;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace ExchangeRateUpdater.Infrastructure.CzechNationalBank
{
    public class CzechNationalBankExchangeRateApiClient(
        HttpClient httpClient, 
        ILogger<CzechNationalBankExchangeRateApiClient> logger) : IExchangeRateApiClient
    {
        public async Task<IReadOnlyList<ApiExchangeRate>> GetDailyExchangeRatesAsync(LanguageCode languageCode = LanguageCode.EN)
        {
            try
            {
                var response = await httpClient.GetFromJsonAsync<CzechNationalBankExchangeRatesResponse>($"exrates/daily?lang={languageCode}");

                return response.Rates.Select(x => new ApiExchangeRate(x.CurrencyCode, x.Rate)).ToArray();
            }
            catch(Exception e)
            {
                logger.LogError(e, "Failed to get daily exchange rates from bank API");
                throw;
            }            
        }
    }
}
