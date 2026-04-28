# Architecture and Flow Documentation

## Component diagram

```mermaid
graph TD
  A[Client App] --> B[ASP.NET Core API]
  B --> C[Controllers]
  C --> D[BuyingLibrary Services]
  D --> E[(MongoDB)]
  C --> F[MailSender]
  F --> G[SMTP Provider]
  B --> H[Health Checks]
  H --> E
  H --> F
```

## Sequence: create order

```mermaid
sequenceDiagram
  participant U as Client
  participant API as OrdersController
  participant S as OrderService
  participant DB as MongoDB
  participant M as MailSender
  participant SMTP as SMTP

  U->>API: POST /Orders
  API->>S: PostAsync(order, ct)
  S->>DB: Insert order
  DB-->>S: saved order
  S-->>API: saved order
  API->>M: SendOrderCreatedAsync(order)
  M->>SMTP: Connect/Auth/Send
  SMTP-->>M: accepted
  API-->>U: 201 Created
```

## Sequence: fetch catalog prices

```mermaid
sequenceDiagram
  participant U as Client
  participant API as PricesController
  participant PS as PriceService
  participant DA as DeserAction
  participant DB as MongoDB

  U->>API: GET /Prices
  API->>PS: GetAsync(ct)
  PS->>DB: find all prices
  DB-->>PS: BSON documents
  PS-->>API: BSON docs
  loop each document
    API->>DA: DeserBson(itemofprice)
    DA-->>API: Item (Coil/Connector)
  end
  API-->>U: 200 List<Price>
```

## Deployment/config flow

```mermaid
flowchart LR
  S[appsettings + env vars] --> O[Options binding + validation]
  O --> P[Program startup]
  P --> API[HTTP API]
  P --> HC[/health endpoint]
  API --> M[(MongoDB)]
  API --> SMTP[(SMTP)]
```

## Error model

- Centralized exception handling enabled (`UseExceptionHandler`).
- Validation/guard failures return `400` with problem details payload.
- Missing resources return `404`.

## Versioning approach

- Current implementation keeps existing route set.
- Future recommended: add explicit route versioning (`/api/v1/...`) and non-breaking deprecation policy.
