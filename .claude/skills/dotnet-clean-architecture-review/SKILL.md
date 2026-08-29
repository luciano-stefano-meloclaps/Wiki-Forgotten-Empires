---
name: dotnet-clean-architecture-review
description: Senior/Staff .NET architecture review for this repo's backend (Domain/Application/Infrastructure/WebApplication1, Notion-only data flow). Use before proposing or applying changes to repositories, services, DTOs, DI registrations, controllers, or design patterns in backend/. Not for frontend-only or single-line changes.
---

# .NET Clean Architecture Review — Wiki-Forgotten-Empires

Gate for backend architecture changes. Audit the proposed change against this repo's
actual structure before writing code. Produce findings, risks, a simpler alternative
if one exists, and an explicit recommendation — do not just generate code.

## When to apply this skill

Apply when the task touches: `backend/Domain/Interfaces`, `backend/Application/Services`,
`backend/Infrastructure/Repositories`, `backend/Application/Models/Dto|Request`,
`backend/WebApplication1/Program.cs` (DI), `backend/WebApplication1/Controllers`, or
introduces a new entity/repository/service/design pattern.

Skip it for: frontend-only work, single-line fixes, changes confined to one file that
add no new abstraction.

## This repo's actual layering (verify against current code, don't assume)

- `Domain` — entities, enums, `Relations/` join entities, `Interfaces/` repository
  contracts. No dependencies on other layers.
- `Application` — service interfaces/impls, DTOs (`Models/Dto`), request models
  (`Models/Request`), `NotionDataStore` (in-memory singleton, source of truth at runtime).
- `Infrastructure` — Notion-backed repository implementations. `ApplicationContext`
  (EF Core) and `Migrations/` are **vestigial**: not registered in DI, never
  instantiated at runtime. Don't propose changes that assume they're live.
- `WebApplication1` (assembly `ForgottenEmpire`) — `Program.cs` DI/CORS/JWT wiring,
  thin `Controllers/`.

Dependency direction: Domain → nothing. Application → Domain only. Infrastructure →
implements Domain/Application interfaces. WebApplication1 → depends on all layers;
nothing depends on it.

## Mandatory rules

1. **Notion is the only data source.** The backend is read-only against Notion —
   repositories throw `NotSupportedException` on Create/Update/Delete. Never propose
   real writes/persistence unless the user explicitly asks to reintroduce it. Don't
   "reactivate" `ApplicationContext`/EF migrations by accident.
2. **No duplicate abstractions.** Before adding an interface/service, check whether
   `Domain/Interfaces`, `Application/Services`, or `Infrastructure/Repositories`
   already cover the case.
3. **No pattern without justification.** Don't introduce Factory/Strategy/Decorator/etc.
   without naming the concrete variability problem it solves today, backed by ≥2 real
   (not hypothetical) cases in the current code.
4. **DTOs stay in `Application/Models/Dto`.** Controllers never return Domain entities
   directly.
5. **Repositories hold no business logic.** That belongs in Services.
6. **Every new DI registration in `Program.cs` states its lifetime explicitly and why**
   (e.g. `NotionDataStore` is Singleton because it's a shared in-memory store).

## Inspect before deciding (read the actual files, don't recall from memory)

- `backend/WebApplication1/Program.cs` — real current DI registrations.
- `backend/Domain/Interfaces/*` — existing contracts.
- `backend/Infrastructure/Repositories/*`, `backend/Application/Services/*` — existing
  implementations to reuse or extend.
- `backend/Application/Models/Dto`, `Models/Request` — established DTO shape.
- `backend/Application/Services/NotionDataStore.cs`,
  `backend/WebApplication1/HostedServices/NotionSyncHostedService.cs` — if the change
  touches data or sync.
- Related `backend/WebApplication1/Controllers/*`.
- `CLAUDE.md` at repo root — don't contradict decisions already documented there.
- Recent `git log`/`git blame` on the area being touched.

## Questions to ask when information is missing

- If the change implies a real write path: is reintroducing persistence intentional,
  or a scope misunderstanding?
- If asked to "add pattern X" with no context: what concrete problem does it solve,
  and are there ≥2 real cases today that justify it?
- If the change spans multiple entities (Age/Battle/Character/Civilization/Territory):
  should it generalize (as the frontend already does via `app/[entity]/[id]`) or stay
  per-entity like the current backend repositories?
- If DI lifetime isn't obvious: Scoped, Singleton, or Transient, and why?

## Avoiding hallucination and over-engineering

- Never describe a class/file without having read it in the current session. If
  something referenced doesn't exist, say so explicitly instead of inventing content.
- Prefer extending existing code over introducing new abstractions.
- Before approving a new interface/layer, check: does more than one real implementation
  today justify it? Does anything else in the repo actually use it, or is it only for
  this one case? Could a method on an existing service solve it instead?

## Evaluating reuse, DI, DTOs, patterns

- **Reuse**: before creating something new, find the existing equivalent (e.g. a new
  repository request should be compared against `AgeRepository` as the established
  pattern).
- **DI**: lifetime consistent with the rest of `Program.cs`, no duplicate registrations,
  no service-locator anti-pattern.
- **DTOs**: should mirror what the frontend actually consumes (real contract in
  `frontend/mappers/*.ts`), never expose raw Notion fields.
- **Patterns**: only when they solve real, documented variability in the current code;
  otherwise write direct, simple code.
