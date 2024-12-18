# **AspireApp1**

This is a **.NET Aspire** solution that demonstrates a secure API protected with an **API Key middleware**. The solution includes a **Blazor frontend**, an **ASP.NET Core Web API**, Aspire orchestration, and integration tests.

---

## **Features**

- **API Key Protection**: The `/api/weatherforecast` endpoint requires a valid `x-api-key` header.
- **Blazor Frontend**: A simple Blazor app fetches weather data from the API.
- **Service-Oriented Architecture**: Proper separation of concerns using controllers and services.
- **Aspire Orchestration**: Handles local orchestration of API, Blazor frontend, and supporting services like Redis.
- **Integration Tests**: Tests validate the API behavior (valid/invalid API keys).

---

## **Solution Structure**

```plaintext
AspireApp1/
│
├── AspireApp1.ApiService       // API backend (Weather Forecast)
├── AspireApp1.AppHost          // Aspire orchestrator
├── AspireApp1.ServiceDefaults  // Shared Aspire configurations/extensions
├── AspireApp1.Tests            // Integration tests for API key validation
└── AspireApp1.Web              // Blazor frontend app
```

---

## **Setup Instructions**

### **1. Prerequisites**

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- Docker (required for Aspire orchestration)

---

### **2. Run the Solution**

1. Open the solution in **Visual Studio** or **Visual Studio Code**.
2. Set **AspireApp1.AppHost** as the startup project.
3. Run the solution:
   ```bash
   dotnet run --project AspireApp1.AppHost
   ```

4. The Aspire Dashboard will open at:  
   `https://localhost:17011`

---

### **3. Accessing the Services**

| **Service**        | **URL**                                | **Description**                              |
|---------------------|---------------------------------------|---------------------------------------------|
| Aspire Dashboard    | `https://localhost:17011`            | Orchestrated Aspire services                |
| Blazor Frontend     | `https://localhost:7261`             | Blazor WebAssembly/Server app               |
| API Service         | `https://localhost:7563`             | Weather Forecast API                        |
| Redis Cache         | `tcp://localhost:61893`              | Redis container for caching                 |

---

### **4. API Key Middleware**

The API service requires a valid `x-api-key` header for secured endpoints:

#### **Default API Key**:
The API key is configured in `appsettings.json`:

```json
{
  "ApiKeys": {
    "DefaultKey": "my-secure-api-key"
  }
}
```

#### **Testing the API**:

- **Valid Key**:
   ```bash
   curl -H "x-api-key: my-secure-api-key" https://localhost:7563/api/weatherforecast
   ```

- **Missing or Invalid Key**:
   - Returns `401 Unauthorized` with the message: `Invalid or missing API Key`.

---

### **5. Integration Tests**

The solution includes integration tests under `AspireApp1.Tests` to validate:

1. Requests with a **valid API key** return `200 OK`.
2. Requests with a **missing API key** return `401 Unauthorized`.
3. Requests with an **invalid API key** return `401 Unauthorized`.

#### **Run Tests**:

Execute the tests with:

```bash
dotnet test
```

---


## **Key Components**

### **AspireApp1.ApiService**
- **WeatherForecastController**: Exposes `/api/weatherforecast` secured by API key middleware.
- **WeatherForecastService**: Generates weather forecast data.
- **Middleware**: Custom API key validation middleware added in `Program.cs`.

### **AspireApp1.Web**
- A simple Blazor frontend that consumes the Weather API.
- Uses `WeatherApiClient` to send requests with the required `x-api-key` header.

### **AspireApp1.Tests**
- Integration tests use **Aspire orchestration** to validate API behavior.

### **AspireApp1.AppHost**
- Orchestrates:
  - API service (`apiservice`)
  - Blazor frontend (`webfrontend`)
  - Redis cache (`cache`)

---

## **Testing Locally**

1. Run the solution (`AspireApp1.AppHost`).
2. Use Swagger UI to test endpoints:
   - `https://localhost:7563/swagger`
3. Use the Blazor app to fetch weather data:
   - `https://localhost:7261`

---

## **Notes**

- **Aspire Dashboard**: The dashboard helps monitor running projects and their states.
- **Middleware Exclusion**: The API key middleware excludes OpenAPI and Aspire routes (e.g., `/swagger` and `/openapi`).

---

## **Troubleshooting**

1. **API Key Issues**:
   - Ensure the `x-api-key` header is sent in requests.
   - Confirm the API key matches the value in `appsettings.json`.

2. **Aspire Services**:
   - Restart the Aspire environment:
     ```bash
     dotnet run --project AspireApp1.AppHost
     ```

---

## **Future Enhancements**

- Add **rate limiting** to protect endpoints from abuse.
- Integrate **Azure Key Vault** for secure storage of API keys.
- Add **authentication/authorization** using Azure AD or OAuth2.

---

## **Credits**

This project uses:
- **.NET Aspire** for orchestration.
- **ASP.NET Core** for the API backend.
- **Blazor** for the frontend.
- **Redis** for caching.

