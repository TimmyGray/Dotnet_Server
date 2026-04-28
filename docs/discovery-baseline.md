# Discovery + Baseline Freeze

## Baseline smoke checks (before refactor)

- `dotnet restore` failed because solution referenced missing external projects:
  - `../BuyingLibrary/BuyingLibrary.csproj`
  - `../Buying_Client/Staff_buying_client.esproj`
- API had outdated framework target (`net6.0`) and old package versions.

## Current endpoint inventory (post-refactor parity target)

- `GET /` — service alive response
- `GET /health` — health checks (mongodb/mail)
- `GET /Buys`
- `GET /Buys/{id}`
- `GET /Buys/image/{id}`
- `POST /Buys`
- `PUT /Buys`
- `DELETE /Buys/{id}`
- `GET /Clients/{id}`
- `POST /Clients`
- `DELETE /Clients/{id}`
- `GET /Coils`
- `GET /Connectors`
- `GET /Orders`
- `GET /Orders/{clientId}`
- `GET /Orders/{clientId}/{orderId}`
- `POST /Orders`
- `PUT /Orders`
- `DELETE /Orders/{id}`
- `GET /Prices`

## App settings inventory

- `ConnectionStrings:AppUrl`
- `ConnectionStrings:ClientUrl`
- `DataBaseSettings:DataBaseConnection`
- `DataBaseSettings:DataBase`
- `EmailSettings:Email`
- `EmailSettings:Password`
- `EmailSettings:Host`
- `EmailSettings:Port` (int)
- `EmailSettings:Name`

## Identified bugs and risks (pre-refactor)

- Broken route templates like `"id:length(24)"`.
- Leading slash route declarations and inconsistent action naming.
- Non-REST semantics (`BadRequest` used for not-found cases, `NoContent` on invalid payload).
- Side effects in GET handlers (mutating model fields before returning).
- Sync email send path with broad exception swallowing.
- Console logging instead of structured logging.
- Unsafe service resolution in startup and weak options validation.
- Broad CORS setup and configuration fragility.
- External project reference dependency preventing clean restore/build.
