# WalletAuditor

A clean, simple C# wallet auditing application built with .NET 6.0 Windows Forms.

## Overview

WalletAuditor is a straightforward desktop application for managing and auditing cryptocurrency wallets. Built from the ground up with simplicity and maintainability in mind.

## Features

- **Simple Wallet Management**: View and manage cryptocurrency wallets
- **Wallet Auditing**: Perform basic audits on wallet data
- **Clean Architecture**: Organized with Core models and Services
- **Async-Ready**: Audit operations support asynchronous execution

## Project Structure

```
WalletAuditor/
├── Core/
│   ├── Wallet.cs          # Simple wallet data model
│   └── AuditResult.cs     # Audit result model
├── Services/
│   ├── WalletService.cs   # Wallet operations
│   └── AuditService.cs    # Audit operations
├── Program.cs             # Application entry point
└── WalletAuditor.csproj   # Clean .NET 6.0 project file
```

## Requirements

- .NET 6.0 SDK or later
- Windows OS (for Windows Forms)

## Building and Running

### Build the project:
```bash
dotnet build
```

### Run the application:
```bash
dotnet run
```

## Usage

1. Launch the application
2. The main window displays a list of sample wallets
3. Select a wallet from the list
4. Click "Audit" to perform a basic audit
5. Click "Refresh" to reload the wallet list

## Design Principles

- ✅ Clean, simple code structure
- ✅ No auto-generated Designer files
- ✅ No external JSON dependencies (uses System.Text.Json if needed)
- ✅ Minimal dependencies
- ✅ Command-line buildable and runnable
- ✅ Ready for real development

## Development

This project is designed to be:
- Easy to understand
- Easy to extend
- Easy to maintain
- Built without Visual Studio Designer
- Compiled and run from command line

## License

MIT License

## Support

For issues or questions, please open an issue in the repository.

- API keys for supported blockchain networks (Infura, Alchemy, or similar)

#### Install via npm
```bash
npm install wallet-auditor
```

#### Install via yarn
```bash
yarn add wallet-auditor
```

### Basic Usage

#### Initialize WalletAuditor
```javascript
const WalletAuditor = require('wallet-auditor');

const auditor = new WalletAuditor({
  apiKey: 'your-api-key',
  network: 'ethereum', // or 'polygon', 'bsc', etc.
});

await auditor.initialize();
```

#### Get Wallet Balance
```javascript
const walletAddress = '0x742d35Cc6634C0532925a3b844Bc9e7595f42e1e';
const balance = await auditor.getBalance(walletAddress);
console.log(`Balance: ${balance.balance} ETH`);
```

#### Get Transaction History
```javascript
const transactions = await auditor.getTransactions(walletAddress, {
  limit: 100,
  offset: 0,
});

transactions.forEach(tx => {
  console.log(`${tx.hash}: ${tx.value} ETH`);
});
```

#### Create a New Audit
```javascript
const audit = await auditor.createAudit({
  addresses: ['0x742d35Cc6634C0532925a3b844Bc9e7595f42e1e'],
  period: '30d', // last 30 days
  includeMetadata: true,
});

console.log(audit.id);
```

### Configuration File

Create a `wallet-auditor.config.js` file in your project root:

```javascript
module.exports = {
  network: 'ethereum',
  apiKey: process.env.AUDITOR_API_KEY,
  cacheExpiry: 3600, // 1 hour in seconds
  maxRetries: 3,
  timeout: 30000, // 30 seconds
};
```

---

## API Reference

### Classes and Methods

#### WalletAuditor Class

##### Constructor
```javascript
new WalletAuditor(options)
```

**Parameters:**
- `options` (Object)
  - `apiKey` (string, required): API key for blockchain provider
  - `network` (string): Blockchain network ('ethereum', 'polygon', 'bsc', 'arbitrum')
  - `cacheExpiry` (number): Cache expiration time in seconds (default: 3600)
  - `maxRetries` (number): Maximum retry attempts (default: 3)
  - `timeout` (number): Request timeout in milliseconds (default: 30000)

##### Methods

###### `initialize()`
Initializes the auditor and establishes connection to the blockchain provider.

```javascript
await auditor.initialize();
```

**Returns:** Promise<void>

###### `getBalance(address, [options])`
Retrieves the current balance of a wallet address.

```javascript
const balance = await auditor.getBalance('0x742d35Cc6634C0532925a3b844Bc9e7595f42e1e', {
  includeTokens: true,
  includeNFTs: false,
});
```

**Parameters:**
- `address` (string): Wallet address
- `options` (Object, optional)
  - `includeTokens` (boolean): Include ERC-20 token balances (default: true)
  - `includeNFTs` (boolean): Include NFT holdings (default: false)
  - `tokenFilter` (Array): Filter specific token addresses

**Returns:** Promise<BalanceResponse>

###### `getTransactions(address, [options])`
Retrieves transaction history for a wallet address.

```javascript
const transactions = await auditor.getTransactions(address, {
  limit: 100,
  offset: 0,
  from: 1609459200, // Unix timestamp
  to: 1640995200,
  type: 'all', // 'incoming', 'outgoing', 'all'
});
```

**Parameters:**
- `address` (string): Wallet address
- `options` (Object, optional)
  - `limit` (number): Results per page (default: 50, max: 10000)
  - `offset` (number): Pagination offset (default: 0)
  - `from` (number): Start time (Unix timestamp)
  - `to` (number): End time (Unix timestamp)
  - `type` (string): Transaction type filter

**Returns:** Promise<TransactionArray>

###### `createAudit(config)`
Creates a comprehensive audit report for specified addresses.

```javascript
const audit = await auditor.createAudit({
  addresses: ['0x742d35Cc6634C0532925a3b844Bc9e7595f42e1e'],
  period: '30d',
  includeMetadata: true,
  format: 'json',
});
```

**Parameters:**
- `config` (Object)
  - `addresses` (Array<string>): Wallet addresses to audit
  - `period` (string): Time period ('7d', '30d', '90d', '1y', or custom dates)
  - `includeMetadata` (boolean): Include additional metadata (default: true)
  - `format` (string): Output format ('json', 'csv', 'pdf')

**Returns:** Promise<AuditResponse>

###### `getAuditStatus(auditId)`
Gets the status of an existing audit.

```javascript
const status = await auditor.getAuditStatus('audit-id-123');
```

**Parameters:**
- `auditId` (string): Audit ID

**Returns:** Promise<AuditStatus>

###### `exportAudit(auditId, format)`
Exports a completed audit in the specified format.

```javascript
const exportedData = await auditor.exportAudit('audit-id-123', 'csv');
```

**Parameters:**
- `auditId` (string): Audit ID
- `format` (string): Export format ('json', 'csv', 'excel', 'pdf')

**Returns:** Promise<Buffer>

###### `setAlert(address, config)`
Sets up alerts for specific wallet addresses.

```javascript
await auditor.setAlert('0x742d35Cc6634C0532925a3b844Bc9e7595f42e1e', {
  type: 'balance_change',
  threshold: 10, // 10 ETH
  direction: 'decrease',
  notificationMethod: 'email',
  email: 'user@example.com',
});
```

**Parameters:**
- `address` (string): Wallet address
- `config` (Object)
  - `type` (string): Alert type ('balance_change', 'transaction', 'gas_price')
  - `threshold` (number): Alert threshold value
  - `direction` (string): 'increase' or 'decrease'
  - `notificationMethod` (string): 'email', 'webhook', 'sms'
  - Additional notification details (email, webhookUrl, etc.)

**Returns:** Promise<AlertResponse>

---

## Configuration

### Environment Variables

Create a `.env` file in your project root:

```env
# API Configuration
AUDITOR_API_KEY=your-api-key
AUDITOR_NETWORK=ethereum
AUDITOR_PROVIDER_URL=https://eth-mainnet.g.alchemy.com/v2/

# Cache Configuration
CACHE_ENABLED=true
CACHE_EXPIRY=3600

# Performance Settings
MAX_CONCURRENT_REQUESTS=5
REQUEST_TIMEOUT=30000
MAX_RETRIES=3

# Alert Configuration
ALERT_ENABLED=true
NOTIFICATION_EMAIL=alerts@example.com

# Logging
LOG_LEVEL=info
LOG_FILE=./logs/auditor.log
```

### Configuration File Format

**wallet-auditor.config.js:**
```javascript
module.exports = {
  // API Configuration
  apiKey: process.env.AUDITOR_API_KEY,
  providerUrl: process.env.AUDITOR_PROVIDER_URL,
  network: 'ethereum',

  // Cache Settings
  cache: {
    enabled: true,
    expiry: 3600,
    backend: 'memory', // 'memory', 'redis'
    redisUrl: process.env.REDIS_URL,
  },

  // Performance Tuning
  performance: {
    maxConcurrentRequests: 5,
    batchSize: 100,
    requestTimeout: 30000,
    maxRetries: 3,
    retryDelay: 1000,
  },

  // Logging
  logging: {
    level: 'info',
    file: './logs/auditor.log',
    maxFileSize: '10mb',
    maxFiles: 10,
  },

  // Alert Settings
  alerts: {
    enabled: true,
    channels: {
      email: {
        enabled: true,
        provider: 'sendgrid',
      },
      webhook: {
        enabled: true,
        url: process.env.WEBHOOK_URL,
      },
    },
  },

  // Supported Networks and RPC Endpoints
  networks: {
    ethereum: 'https://eth-mainnet.g.alchemy.com/v2/',
    polygon: 'https://polygon-mainnet.g.alchemy.com/v2/',
    arbitrum: 'https://arb-mainnet.g.alchemy.com/v2/',
    bsc: 'https://bsc-dataseed.binance.org/',
  },
};
```

---

## Performance Optimization

### Caching Strategies

#### Enable Redis Caching
For large-scale deployments, use Redis to cache frequently accessed data:

```javascript
const auditor = new WalletAuditor({
  apiKey: 'your-api-key',
  cache: {
    enabled: true,
    backend: 'redis',
    redisUrl: 'redis://localhost:6379',
    expiry: 3600,
  },
});
```

#### Cache Configuration
```javascript
const cacheConfig = {
  // Cache balance data for 1 hour
  balanceCacheTTL: 3600,
  // Cache transaction data for 30 minutes (data might update frequently)
  transactionCacheTTL: 1800,
  // Cache audit reports for 24 hours
  auditCacheTTL: 86400,
};
```

### Batch Processing

Process multiple addresses efficiently:

```javascript
const addresses = [
  '0x742d35Cc6634C0532925a3b844Bc9e7595f42e1e',
  '0x8ba1f109551bD432803012645Ac136ddd64DBA72',
  // ... more addresses
];

const results = await auditor.batchGetBalance(addresses, {
  concurrency: 5,
  timeout: 30000,
});
```

### Request Optimization

#### Pagination
```javascript
// Retrieve large datasets in chunks
async function getAllTransactions(address) {
  let allTransactions = [];
  let offset = 0;
  const limit = 10000;
  let hasMore = true;

  while (hasMore) {
    const batch = await auditor.getTransactions(address, {
      limit,
      offset,
    });

    if (batch.length === 0) {
      hasMore = false;
    } else {
      allTransactions = allTransactions.concat(batch);
      offset += limit;
    }
  }

  return allTransactions;
}
```

#### Connection Pooling
```javascript
const auditor = new WalletAuditor({
  apiKey: 'your-api-key',
  connectionPool: {
    max: 10,
    min: 2,
    idleTimeoutMillis: 30000,
  },
});
```

### Monitoring Performance

```javascript
auditor.on('performance', (metrics) => {
  console.log(`Request took ${metrics.duration}ms`);
  console.log(`Cache hit rate: ${metrics.cacheHitRate}%`);
});
```

---

## Troubleshooting

### Common Issues and Solutions

#### Issue: "Invalid API Key" Error
**Solution:**
1. Verify your API key is correct and active
2. Check environment variables are properly loaded
3. Ensure API key has appropriate permissions

```bash
# Verify configuration
node -e "console.log(process.env.AUDITOR_API_KEY)"
```

#### Issue: Rate Limiting (429 Error)
**Solution:**
1. Implement exponential backoff retry logic
2. Reduce concurrent requests
3. Use batch operations more efficiently

```javascript
const auditor = new WalletAuditor({
  apiKey: 'your-api-key',
  maxRetries: 5,
  retryDelay: 2000, // Start with 2 second delay
  exponentialBackoff: true,
});
```

#### Issue: Timeout Errors
**Solution:**
1. Increase timeout threshold
2. Check network connectivity
3. Reduce request payload size

```javascript
const auditor = new WalletAuditor({
  apiKey: 'your-api-key',
  timeout: 60000, // 60 seconds
});
```

#### Issue: Inconsistent Balance Results
**Solution:**
1. Clear cache and fetch fresh data
2. Use different blockchain provider
3. Verify address is valid

```javascript
// Bypass cache
const balance = await auditor.getBalance(address, {
  useCache: false,
});

// Verify address format
const isValid = auditor.validateAddress(address);
```

#### Issue: Missing or Incomplete Transaction Data
**Solution:**
1. Verify address has sufficient history
2. Check time period filters
3. Ensure all token contracts are properly indexed

```javascript
// Fetch all available data
const transactions = await auditor.getTransactions(address, {
  limit: 10000,
  from: 0, // From genesis
  includeInternalTransactions: true,
  includeTokenTransfers: true,
});
```

### Debugging

Enable debug logging:

```javascript
const auditor = new WalletAuditor({
  apiKey: 'your-api-key',
  logging: {
    level: 'debug',
    file: './logs/auditor-debug.log',
  },
});

// Or set environment variable
process.env.DEBUG = 'wallet-auditor:*';
```

---

## Security Considerations

### Protecting Your API Keys

#### Best Practices
1. **Never hardcode API keys** in your codebase
2. **Use environment variables** for sensitive configuration
3. **Rotate API keys regularly** (recommended: every 90 days)
4. **Use different keys per environment** (development, staging, production)
5. **Restrict API key permissions** to minimum required scope

#### Example: Secure Configuration
```javascript
// ✓ Good
require('dotenv').config();
const auditor = new WalletAuditor({
  apiKey: process.env.AUDITOR_API_KEY,
});

// ✗ Bad - Never do this
const auditor = new WalletAuditor({
  apiKey: 'sk_live_51234567890abcdef',
});
```

### Data Privacy

#### Private Key Handling
- **Never** transmit private keys to WalletAuditor servers
- **Never** store private keys in configuration files
- **Use read-only** API endpoints when possible
- **Implement encryption** for sensitive wallet data in transit

#### Address Anonymization
```javascript
// For privacy-sensitive operations
const audit = await auditor.createAudit({
  addresses: walletAddresses,
  anonymize: true,
  excludePII: true,
});
```

### Network Security

#### HTTPS Enforcement
```javascript
const auditor = new WalletAuditor({
  apiKey: 'your-api-key',
  https: true,
  certificateVerification: true,
});
```

#### Webhook Validation
```javascript
const crypto = require('crypto');

// Validate webhook signatures
function validateWebhookSignature(payload, signature, secret) {
  const hash = crypto
    .createHmac('sha256', secret)
    .update(JSON.stringify(payload))
    .digest('hex');

  return hash === signature;
}
```

### Compliance

#### GDPR Compliance
- Implement data retention policies
- Enable right-to-be-forgotten functionality
- Document data processing activities

```javascript
// Delete personal data
await auditor.deleteAuditData(auditId, {
  includePII: true,
  soft: false, // Hard delete
});
```

#### SOC 2 Compliance
- Regular security audits
- Access control implementation
- Incident logging and monitoring

---

## Data Export Formats

### JSON Format
```json
{
  "audit_id": "audit-123",
  "created_at": "2025-12-29T00:05:19Z",
  "addresses": ["0x742d35Cc6634C0532925a3b844Bc9e7595f42e1e"],
  "period": {
    "from": "2025-12-01T00:00:00Z",
    "to": "2025-12-29T00:05:19Z"
  },
  "summary": {
    "total_transactions": 156,
    "total_volume": "1250.50",
    "average_transaction": "8.01"
  },
  "transactions": [
    {
      "hash": "0xabc123...",
      "from": "0x742d35Cc6634C0532925a3b844Bc9e7595f42e1e",
      "to": "0x8ba1f109551bD432803012645Ac136ddd64DBA72",
      "value": "1.50",
      "timestamp": "2025-12-28T15:30:00Z",
      "status": "success",
      "gas_used": "21000",
      "gas_price": "50000000000"
    }
  ]
}
```

### CSV Format
```csv
hash,from,to,value,timestamp,status,gas_used,gas_price
0xabc123...,0x742d35Cc6634C0532925a3b844Bc9e7595f42e1e,0x8ba1f109551bD432803012645Ac136ddd64DBA72,1.50,2025-12-28T15:30:00Z,success,21000,50000000000
0xdef456...,0x8ba1f109551bD432803012645Ac136ddd64DBA72,0x742d35Cc6634C0532925a3b844Bc9e7595f42e1e,2.30,2025-12-28T14:20:00Z,success,21000,55000000000
```

### Excel Format
- Multi-sheet workbook with Summary, Transactions, Balances, and Metadata sheets
- Formatted headers and data validation
- Charts and pivot table support

### PDF Format
- Professional audit report layout
- Executive summary
- Detailed transaction listings
- Charts and graphs
- Compliance certifications

#### Export Examples
```javascript
// Export as JSON
const jsonExport = await auditor.exportAudit('audit-123', 'json');
fs.writeFileSync('audit-report.json', jsonExport);

// Export as CSV
const csvExport = await auditor.exportAudit('audit-123', 'csv');
fs.writeFileSync('audit-report.csv', csvExport);

// Export as Excel
const excelExport = await auditor.exportAudit('audit-123', 'excel');
fs.writeFileSync('audit-report.xlsx', excelExport);

// Export as PDF
const pdfExport = await auditor.exportAudit('audit-123', 'pdf');
fs.writeFileSync('audit-report.pdf', pdfExport);
```

---

## Development Information

### Project Structure
```
WalletAuditor/
├── src/
│   ├── core/
│   │   ├── auditor.js          # Main auditor class
│   │   ├── provider.js         # Blockchain provider
│   │   └── cache.js            # Caching logic
│   ├── api/
│   │   ├── endpoints.js        # API route definitions
│   │   └── handlers.js         # Request handlers
│   ├── utils/
│   │   ├── validation.js       # Input validation
│   │   ├── formatting.js       # Data formatting
│   │   └── errors.js           # Error handling
│   └── exports/
│       ├── json.js             # JSON export
│       ├── csv.js              # CSV export
│       └── pdf.js              # PDF export
├── tests/
│   ├── unit/
│   ├── integration/
│   └── fixtures/
├── docs/
│   ├── API.md
│   ├── ARCHITECTURE.md
│   └── CONTRIBUTING.md
├── .github/
│   └── workflows/              # CI/CD workflows
├── package.json
├── .env.example
└── README.md
```

### Setting Up Development Environment

```bash
# Clone repository
git clone https://github.com/janiceestillhowell-lab/WalletAuditor.git
cd WalletAuditor

# Install dependencies
npm install

# Copy environment template
cp .env.example .env
# Edit .env with your configuration

# Run tests
npm test

# Start development server
npm run dev

# Build for production
npm run build
```

### Running Tests

```bash
# Run all tests
npm test

# Run specific test suite
npm test -- tests/unit/auditor.test.js

# Run with coverage
npm test -- --coverage

# Run integration tests
npm run test:integration

# Watch mode for development
npm test -- --watch
```

### Code Style and Linting

```bash
# Check code style
npm run lint

# Fix linting issues
npm run lint:fix

# Format code
npm run format

# Check type definitions
npm run type-check
```

### Building Documentation

```bash
# Generate API documentation
npm run docs:generate

# Serve documentation locally
npm run docs:serve
```

### Contributing

See [CONTRIBUTING.md](./CONTRIBUTING.md) for detailed contribution guidelines.

### Technologies Used

- **Runtime:** Node.js >= 14.0.0
- **Language:** JavaScript (ES6+)
- **Package Manager:** npm or yarn
- **Testing:** Jest, Mocha
- **Linting:** ESLint, Prettier
- **Documentation:** JSDoc, Markdown
- **CI/CD:** GitHub Actions
- **Database:** Optional (Redis for caching)

---

## License

WalletAuditor is licensed under the MIT License. See [LICENSE](./LICENSE) file for details.

### Summary
- You are free to use, modify, and distribute this software
- Include original license and copyright notices in distributions
- No warranty is provided; use at your own risk

---

## Support

### Getting Help

#### Documentation
- [Full API Documentation](./docs/API.md)
- [Architecture Guide](./docs/ARCHITECTURE.md)
- [FAQ](./docs/FAQ.md)

#### Community
- **GitHub Issues:** [Report bugs or request features](https://github.com/janiceestillhowell-lab/WalletAuditor/issues)
- **Discussions:** [Join community discussions](https://github.com/janiceestillhowell-lab/WalletAuditor/discussions)

#### Contact
- **Email:** support@walletauditor.io
- **Twitter:** [@WalletAuditor](https://twitter.com/WalletAuditor)
- **Website:** https://walletauditor.io

### Reporting Security Issues

For security vulnerabilities, please email `security@walletauditor.io` instead of using the issue tracker.

### Version Support

| Version | Status | Support Until |
|---------|--------|---------------|
| 2.x     | Current | 2027-12-31 |
| 1.x     | LTS | 2026-12-31 |
| 0.x     | End of Life | N/A |

### Roadmap

- **Q1 2026:** Multi-signature wallet support
- **Q2 2026:** Advanced machine learning anomaly detection
- **Q3 2026:** Cross-chain portfolio aggregation
- **Q4 2026:** Mobile application release

---

**Last Updated:** 2025-12-29

For the latest information, visit [WalletAuditor GitHub Repository](https://github.com/janiceestillhowell-lab/WalletAuditor)
