# Leap Events Assessment

## Section 1

### Getting started

I've used a justfile for this project.

```shell
just install # install dependencies

just backend # run asp.net backend at port 5000
just frontend # run next.js frontend at port 3000

just test # run NUnit tests
```

### Architecture

#### Backend

I followed a loose DDD structure (keeping everything in one project for simplicity).

I have ended up with the EventService being very slim. This is because there isn't really any domain logic and so most lives in either the controllers or repositories.

#### Frontend

Implemented a next.js frontend with a very simple UI.

#### Testing approach

I have kept tests fairly minimal and restricted to the backend.

Unit tests: Only for repositories - as it stands nothing else requires them
Integration tests: Tests API directly to check it all plugs together

Improvements:

- implement proper test DB setup and seeding for integration tests
- consider UI test layer using headless browser testing suite

## Section 2

See documents/caching.md
