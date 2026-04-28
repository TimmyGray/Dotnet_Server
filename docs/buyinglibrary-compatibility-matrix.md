# BuyingLibrary v2 Compatibility Matrix

## Source alignment

Latest reference analyzed from: `TimmyGray/BuyingLibrary` (`v2.0.0`, `net10.0`).

## Breaking changes mapped

| Area | Previous usage | v2 expectation | Migration decision |
|---|---|---|---|
| Framework | net6 app + old library | net10 library | Upgraded host to net10 |
| Model IDs | `_id` fields in app code | `Id` properties | Replaced app usage with `Id` |
| Order client property | `order.client` | `order.Client` | Updated mail/controller code |
| Price property | `Itemofprice` | `ItemOfPrice` | Updated price mapping |
| Coil signal property | `typeofsignal` | `TypeOfSignal` | Library-aligned model usage |
| Mail options port | `string` | `int` | Converted app config and sender logic |
| Service signatures | no cancellation tokens | cancellation token overloads | Added token flow in controllers |
| Order query methods | old `GetAsync(clientid, ...)` semantics | `GetByClientAsync`, `GetByClientAndOrderAsync` | Updated controller to explicit methods |
| Image service method | `GetOne` | `GetOneAsync` | Updated image endpoint |

## Contract strategy

- Keep main API goal and route surface compatible for client-facing operations.
- Use direct model reuse from BuyingLibrary for domain payloads.
- Introduce DTO (`ClientUpsertRequest`) where validation was needed immediately.

## Remaining enhancement candidates

- Expand DTO layer for all write operations to fully decouple API contracts from persistence models.
- Add richer OpenAPI examples and response schemas.
