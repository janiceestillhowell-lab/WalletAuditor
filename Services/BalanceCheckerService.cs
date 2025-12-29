using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;

namespace WalletAuditor.Services
{
    /// <summary>
    /// Service for checking cryptocurrency wallet balances across multiple blockchain networks.
    /// Supports Bitcoin, Ethereum, Binance Smart Chain, Litecoin, Dogecoin, TRON, XRP, and Solana.
    /// Uses free RPC endpoints with optional premium API key support for enhanced rate limits.
    /// </summary>
    public class BalanceCheckerService
    {
        private readonly HttpClient _httpClient;
        private readonly Dictionary<string, NetworkConfig> _networks;

        /// <summary>
        /// Configuration for blockchain network RPC endpoints and API settings.
        /// </summary>
        private class NetworkConfig
        {
            public string Name { get; set; }
            public List<string> FreeRpcEndpoints { get; set; }
            public string PremiumApiKey { get; set; }
            public string PremiumRpcEndpoint { get; set; }
            public NetworkType Type { get; set; }
        }

        /// <summary>
        /// Supported blockchain network types.
        /// </summary>
        public enum NetworkType
        {
            Bitcoin,
            Ethereum,
            BinanceSmartChain,
            Litecoin,
            Dogecoin,
            TRON,
            XRP,
            Solana
        }

        /// <summary>
        /// Represents the balance result for a wallet address.
        /// </summary>
        public class BalanceResult
        {
            public string Network { get; set; }
            public string Address { get; set; }
            public string Balance { get; set; }
            public string BalanceFormatted { get; set; }
            public string Unit { get; set; }
            public bool Success { get; set; }
            public string ErrorMessage { get; set; }
            public DateTime QueryTime { get; set; }
        }

        /// <summary>
        /// Initializes a new instance of the BalanceCheckerService.
        /// </summary>
        /// <param name="httpClient">HttpClient instance for making HTTP requests.</param>
        /// <param name="premiumApiKeys">Optional dictionary of premium API keys for networks (network name as key).</param>
        public BalanceCheckerService(HttpClient httpClient, Dictionary<string, string> premiumApiKeys = null)
        {
            _httpClient = httpClient ?? new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(30);

            _networks = InitializeNetworks(premiumApiKeys);
        }

        /// <summary>
        /// Initializes network configurations with free RPC endpoints and optional premium API keys.
        /// </summary>
        private Dictionary<string, NetworkConfig> InitializeNetworks(Dictionary<string, string> premiumApiKeys)
        {
            return new Dictionary<string, NetworkConfig>
            {
                {
                    "Bitcoin",
                    new NetworkConfig
                    {
                        Name = "Bitcoin",
                        Type = NetworkType.Bitcoin,
                        FreeRpcEndpoints = new List<string>
                        {
                            "https://blockstream.info/api",
                            "https://blockchain.info/q",
                            "https://api.blockcypher.com/v1/btc/main"
                        },
                        PremiumApiKey = premiumApiKeys?.ContainsKey("Bitcoin") == true ? premiumApiKeys["Bitcoin"] : null,
                        PremiumRpcEndpoint = "https://mainnet.infura.io/v3/{KEY}" // Can be substituted
                    }
                },
                {
                    "Ethereum",
                    new NetworkConfig
                    {
                        Name = "Ethereum",
                        Type = NetworkType.Ethereum,
                        FreeRpcEndpoints = new List<string>
                        {
                            "https://eth.public.blastapi.io",
                            "https://rpc.ankr.com/eth",
                            "https://ethereum.publicnode.com"
                        },
                        PremiumApiKey = premiumApiKeys?.ContainsKey("Ethereum") == true ? premiumApiKeys["Ethereum"] : null,
                        PremiumRpcEndpoint = "https://mainnet.infura.io/v3/{KEY}"
                    }
                },
                {
                    "BinanceSmartChain",
                    new NetworkConfig
                    {
                        Name = "Binance Smart Chain",
                        Type = NetworkType.BinanceSmartChain,
                        FreeRpcEndpoints = new List<string>
                        {
                            "https://bsc.publicnode.com",
                            "https://rpc.ankr.com/bsc",
                            "https://bscrpc.com"
                        },
                        PremiumApiKey = premiumApiKeys?.ContainsKey("BinanceSmartChain") == true ? premiumApiKeys["BinanceSmartChain"] : null,
                        PremiumRpcEndpoint = "https://bsc-dataseed.binance.org"
                    }
                },
                {
                    "Litecoin",
                    new NetworkConfig
                    {
                        Name = "Litecoin",
                        Type = NetworkType.Litecoin,
                        FreeRpcEndpoints = new List<string>
                        {
                            "https://blockstream.info/litecoin/api",
                            "https://ltc.public.blastapi.io",
                            "https://api.blockcypher.com/v1/ltc/main"
                        },
                        PremiumApiKey = premiumApiKeys?.ContainsKey("Litecoin") == true ? premiumApiKeys["Litecoin"] : null,
                        PremiumRpcEndpoint = "https://ltc.public.blastapi.io"
                    }
                },
                {
                    "Dogecoin",
                    new NetworkConfig
                    {
                        Name = "Dogecoin",
                        Type = NetworkType.Dogecoin,
                        FreeRpcEndpoints = new List<string>
                        {
                            "https://dogechain.info/api",
                            "https://blockexplorer.one/api",
                            "https://doge.public.blastapi.io"
                        },
                        PremiumApiKey = premiumApiKeys?.ContainsKey("Dogecoin") == true ? premiumApiKeys["Dogecoin"] : null,
                        PremiumRpcEndpoint = "https://doge.public.blastapi.io"
                    }
                },
                {
                    "TRON",
                    new NetworkConfig
                    {
                        Name = "TRON",
                        Type = NetworkType.TRON,
                        FreeRpcEndpoints = new List<string>
                        {
                            "https://api.trongrid.io",
                            "https://tron.publicnode.com",
                            "https://api.tronscan.org"
                        },
                        PremiumApiKey = premiumApiKeys?.ContainsKey("TRON") == true ? premiumApiKeys["TRON"] : null,
                        PremiumRpcEndpoint = "https://api.trongrid.io"
                    }
                },
                {
                    "XRP",
                    new NetworkConfig
                    {
                        Name = "XRP Ledger",
                        Type = NetworkType.XRP,
                        FreeRpcEndpoints = new List<string>
                        {
                            "https://xrpl.ws",
                            "https://s1.ripple.com:51234",
                            "https://s2.ripple.com:51234"
                        },
                        PremiumApiKey = premiumApiKeys?.ContainsKey("XRP") == true ? premiumApiKeys["XRP"] : null,
                        PremiumRpcEndpoint = "https://xrpl.ws"
                    }
                },
                {
                    "Solana",
                    new NetworkConfig
                    {
                        Name = "Solana",
                        Type = NetworkType.Solana,
                        FreeRpcEndpoints = new List<string>
                        {
                            "https://api.devnet.solana.com",
                            "https://api.testnet.solana.com",
                            "https://public-rpc.blockeden.xyz/solana"
                        },
                        PremiumApiKey = premiumApiKeys?.ContainsKey("Solana") == true ? premiumApiKeys["Solana"] : null,
                        PremiumRpcEndpoint = "https://api.mainnet-beta.solana.com"
                    }
                }
            };
        }

        /// <summary>
        /// Checks the balance of a wallet address across specified networks.
        /// Executes requests in parallel for optimal performance.
        /// </summary>
        /// <param name="address">The wallet address to check.</param>
        /// <param name="networks">Networks to check (defaults to all if null).</param>
        /// <returns>List of balance results for each network.</returns>
        public async Task<List<BalanceResult>> CheckBalanceAsync(string address, List<string> networks = null)
        {
            if (string.IsNullOrWhiteSpace(address))
            {
                throw new ArgumentException("Address cannot be null or empty.", nameof(address));
            }

            var targetNetworks = networks == null 
                ? _networks.Keys.ToList() 
                : networks.Where(n => _networks.ContainsKey(n)).ToList();

            var tasks = targetNetworks.Select(network => CheckNetworkBalanceAsync(address, network))
                .ToList();

            var results = await Task.WhenAll(tasks);
            return results.ToList();
        }

        /// <summary>
        /// Checks the balance for a specific network with retry logic.
        /// </summary>
        private async Task<BalanceResult> CheckNetworkBalanceAsync(string address, string networkName)
        {
            if (!_networks.TryGetValue(networkName, out var networkConfig))
            {
                return new BalanceResult
                {
                    Network = networkName,
                    Address = address,
                    Success = false,
                    ErrorMessage = $"Network '{networkName}' is not supported.",
                    QueryTime = DateTime.UtcNow
                };
            }

            var startTime = DateTime.UtcNow;

            try
            {
                // Try premium endpoint first if API key is available
                if (!string.IsNullOrEmpty(networkConfig.PremiumApiKey))
                {
                    var premiumResult = await QueryNetworkAsync(address, networkConfig, true);
                    if (premiumResult != null)
                    {
                        premiumResult.QueryTime = DateTime.UtcNow;
                        return premiumResult;
                    }
                }

                // Fall back to free endpoints with retry logic
                foreach (var endpoint in networkConfig.FreeRpcEndpoints)
                {
                    var result = await QueryEndpointAsync(address, networkConfig, endpoint, false);
                    if (result != null)
                    {
                        result.QueryTime = DateTime.UtcNow;
                        return result;
                    }
                }

                return new BalanceResult
                {
                    Network = networkConfig.Name,
                    Address = address,
                    Success = false,
                    ErrorMessage = "All RPC endpoints failed. Service may be temporarily unavailable.",
                    QueryTime = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                return new BalanceResult
                {
                    Network = networkConfig.Name,
                    Address = address,
                    Success = false,
                    ErrorMessage = $"Error checking balance: {ex.Message}",
                    QueryTime = DateTime.UtcNow
                };
            }
        }

        /// <summary>
        /// Queries a specific RPC endpoint for balance information.
        /// </summary>
        private async Task<BalanceResult> QueryEndpointAsync(string address, NetworkConfig config, string endpoint, bool isPremium)
        {
            try
            {
                switch (config.Type)
                {
                    case NetworkType.Bitcoin:
                        return await QueryBitcoinAsync(address, endpoint);
                    case NetworkType.Ethereum:
                    case NetworkType.BinanceSmartChain:
                        return await QueryEVMAsync(address, endpoint, config.Name);
                    case NetworkType.Litecoin:
                        return await QueryLitecoinAsync(address, endpoint);
                    case NetworkType.Dogecoin:
                        return await QueryDogecoinAsync(address, endpoint);
                    case NetworkType.TRON:
                        return await QueryTRONAsync(address, endpoint);
                    case NetworkType.XRP:
                        return await QueryXRPAsync(address, endpoint);
                    case NetworkType.Solana:
                        return await QuerySolanaAsync(address, endpoint);
                    default:
                        return null;
                }
            }
            catch (HttpRequestException)
            {
                return null;
            }
            catch (TaskCanceledException)
            {
                return null;
            }
        }

        /// <summary>
        /// Queries premium API endpoint if available.
        /// </summary>
        private async Task<BalanceResult> QueryNetworkAsync(string address, NetworkConfig config, bool usePremium)
        {
            if (!usePremium || string.IsNullOrEmpty(config.PremiumApiKey))
            {
                return null;
            }

            var endpoint = config.PremiumRpcEndpoint.Replace("{KEY}", config.PremiumApiKey);
            return await QueryEndpointAsync(address, config, endpoint, true);
        }

        /// <summary>
        /// Queries Bitcoin balance via Blockstream API.
        /// </summary>
        private async Task<BalanceResult> QueryBitcoinAsync(string address, string endpoint)
        {
            var url = $"{endpoint}/address/{address}";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return null;

            var content = await response.Content.ReadAsStringAsync();
            using var json = JsonDocument.Parse(content);
            var root = json.RootElement;

            if (!root.TryGetProperty("chain_stats", out var chainStats))
                return null;

            if (!chainStats.TryGetProperty("funded_txo_sum", out var fundedTxoSum))
                return null;

            var balance = fundedTxoSum.GetInt64();
            var balanceBTC = balance / 100_000_000m;

            return new BalanceResult
            {
                Network = "Bitcoin",
                Address = address,
                Balance = balance.ToString(),
                BalanceFormatted = balanceBTC.ToString("F8"),
                Unit = "BTC",
                Success = true
            };
        }

        /// <summary>
        /// Queries Ethereum or BSC balance via JSON-RPC.
        /// </summary>
        private async Task<BalanceResult> QueryEVMAsync(string address, string endpoint, string networkName)
        {
            var payload = new
            {
                jsonrpc = "2.0",
                method = "eth_getBalance",
                @params = new[] { address, "latest" },
                id = 1
            };

            var content = new StringContent(
                System.Text.Json.JsonSerializer.Serialize(payload),
                System.Text.Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync(endpoint, content);

            if (!response.IsSuccessStatusCode)
                return null;

            var responseContent = await response.Content.ReadAsStringAsync();
            using var json = JsonDocument.Parse(responseContent);
            var root = json.RootElement;

            if (root.TryGetProperty("error", out _))
                return null;

            if (!root.TryGetProperty("result", out var resultElement))
                return null;

            var balanceHex = resultElement.GetString();
            if (string.IsNullOrEmpty(balanceHex))
                return null;

            var balance = Convert.ToInt64(balanceHex, 16);
            var balanceETH = balance / 1_000_000_000_000_000_000m;

            var unit = networkName.Contains("Binance") ? "BNB" : "ETH";

            return new BalanceResult
            {
                Network = networkName,
                Address = address,
                Balance = balance.ToString(),
                BalanceFormatted = balanceETH.ToString("F18"),
                Unit = unit,
                Success = true
            };
        }

        /// <summary>
        /// Queries Litecoin balance via Blockstream API.
        /// </summary>
        private async Task<BalanceResult> QueryLitecoinAsync(string address, string endpoint)
        {
            var url = $"{endpoint}/address/{address}";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return null;

            var responseContent = await response.Content.ReadAsStringAsync();
            using var json = JsonDocument.Parse(responseContent);
            var root = json.RootElement;

            if (!root.TryGetProperty("chain_stats", out var chainStats))
                return null;

            if (!chainStats.TryGetProperty("funded_txo_sum", out var fundedTxoSum))
                return null;

            var balance = fundedTxoSum.GetInt64();
            var balanceLTC = balance / 100_000_000m;

            return new BalanceResult
            {
                Network = "Litecoin",
                Address = address,
                Balance = balance.ToString(),
                BalanceFormatted = balanceLTC.ToString("F8"),
                Unit = "LTC",
                Success = true
            };
        }

        /// <summary>
        /// Queries Dogecoin balance via DogeChain API.
        /// </summary>
        private async Task<BalanceResult> QueryDogecoinAsync(string address, string endpoint)
        {
            var url = $"{endpoint}/address/{address}/balance";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return null;

            var responseContent = await response.Content.ReadAsStringAsync();
            using var json = JsonDocument.Parse(responseContent);
            var root = json.RootElement;

            if (!root.TryGetProperty("balance", out var balanceElement))
                return null;

            var balance = balanceElement.GetDecimal();

            return new BalanceResult
            {
                Network = "Dogecoin",
                Address = address,
                Balance = balance.ToString(),
                BalanceFormatted = balance.ToString("F8"),
                Unit = "DOGE",
                Success = true
            };
        }

        /// <summary>
        /// Queries TRON balance via TronGrid API.
        /// </summary>
        private async Task<BalanceResult> QueryTRONAsync(string address, string endpoint)
        {
            var url = $"{endpoint}/v1/accounts/{address}";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return null;

            var responseContent = await response.Content.ReadAsStringAsync();
            using var json = JsonDocument.Parse(responseContent);
            var root = json.RootElement;

            if (!root.TryGetProperty("data", out var dataElement))
                return null;

            if (dataElement.GetArrayLength() == 0)
                return null;

            if (!dataElement[0].TryGetProperty("balance", out var balanceElement))
                return null;

            var balance = balanceElement.GetInt64();
            var balanceTRX = balance / 1_000_000m;

            return new BalanceResult
            {
                Network = "TRON",
                Address = address,
                Balance = balance.ToString(),
                BalanceFormatted = balanceTRX.ToString("F6"),
                Unit = "TRX",
                Success = true
            };
        }

        /// <summary>
        /// Queries XRP Ledger balance via JSON-RPC.
        /// </summary>
        private async Task<BalanceResult> QueryXRPAsync(string address, string endpoint)
        {
            var payload = new
            {
                method = "account_info",
                @params = new object[]
                {
                    new { account = address }
                }
            };

            var content = new StringContent(
                System.Text.Json.JsonSerializer.Serialize(payload),
                System.Text.Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync(endpoint, content);

            if (!response.IsSuccessStatusCode)
                return null;

            var responseContent = await response.Content.ReadAsStringAsync();
            using var json = JsonDocument.Parse(responseContent);
            var root = json.RootElement;

            if (!root.TryGetProperty("result", out var resultElement))
                return null;

            if (!resultElement.TryGetProperty("account_data", out var accountData))
                return null;

            if (!accountData.TryGetProperty("Balance", out var balanceElement))
                return null;

            var balance = balanceElement.GetString();
            if (string.IsNullOrEmpty(balance) || !long.TryParse(balance, out var balanceLong))
                return null;

            var balanceXRP = balanceLong / 1_000_000m;

            return new BalanceResult
            {
                Network = "XRP Ledger",
                Address = address,
                Balance = balance,
                BalanceFormatted = balanceXRP.ToString("F6"),
                Unit = "XRP",
                Success = true
            };
        }

        /// <summary>
        /// Queries Solana balance via JSON-RPC.
        /// </summary>
        private async Task<BalanceResult> QuerySolanaAsync(string address, string endpoint)
        {
            var payload = new
            {
                jsonrpc = "2.0",
                method = "getBalance",
                @params = new object[] { address },
                id = 1
            };

            var content = new StringContent(
                System.Text.Json.JsonSerializer.Serialize(payload),
                System.Text.Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync(endpoint, content);

            if (!response.IsSuccessStatusCode)
                return null;

            var responseContent = await response.Content.ReadAsStringAsync();
            using var json = JsonDocument.Parse(responseContent);
            var root = json.RootElement;

            if (root.TryGetProperty("error", out _))
                return null;

            if (!root.TryGetProperty("result", out var resultElement))
                return null;

            if (!resultElement.TryGetProperty("value", out var valueElement))
                return null;

            var balance = valueElement.GetInt64();
            var balanceSOL = balance / 1_000_000_000m;

            return new BalanceResult
            {
                Network = "Solana",
                Address = address,
                Balance = balance.ToString(),
                BalanceFormatted = balanceSOL.ToString("F9"),
                Unit = "SOL",
                Success = true
            };
        }

        /// <summary>
        /// Checks balances for multiple addresses across all networks.
        /// </summary>
        /// <param name="addresses">List of wallet addresses to check.</param>
        /// <param name="networks">Networks to check (defaults to all if null).</param>
        /// <returns>Dictionary with addresses as keys and balance results as values.</returns>
        public async Task<Dictionary<string, List<BalanceResult>>> CheckBalancesAsync(List<string> addresses, List<string> networks = null)
        {
            if (addresses == null || !addresses.Any())
            {
                throw new ArgumentException("At least one address must be provided.", nameof(addresses));
            }

            var tasks = addresses.Select(address => CheckBalanceAsync(address, networks))
                .ToList();

            var results = await Task.WhenAll(tasks);

            return addresses
                .Zip(results, (address, balances) => new { address, balances })
                .ToDictionary(x => x.address, x => x.balances);
        }
    }
}
