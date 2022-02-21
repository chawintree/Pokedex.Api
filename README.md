# Pokedex API

A simple API which returns Pokemon information

## Prerequisites

The user should have the `.NET 6.0` SDKs installed and optionally `docker`

## Building the application

Run the following command in the root directory to create a Docker image called `pokedex-api`

``` cmd
docker build -t pokedex-api -f src/Pokedex.Api/Dockerfile .
```

Alternatively, the application can be built within Visual Studio.

## Running the application

If built using Docker, the API can be run with the following command:

``` cmd
docker run -it --rm -p 5000:80 --name pokedex-api pokedex-api
```

If using Visual Studio, the API can be run using the `Pokedex.Api` profile

## Using the application

Navigating to http://localhost:5000/swagger will allow you to use a Swagger UI to access the application.

Alternatively,  the following curl commands can be used:

### Get Pokemon

``` cmd
curl -X 'GET' 'http://localhost:5000/pokemon/ditto'
```

### Get Pokemon (Translated)

``` cmd
curl -X 'GET' 'http://localhost:5000/pokemon/translated/mewtwo'
```

## Moving forward

Prior to moving this application to a Production-like environment, there are a number of additional features I'd require:

* Authentication / Authorization
* Logging
  * Middleware for response codes, routes and metadata
  * Wrapper for dependant API clients calls
  * Exception logging
* Metrics
  * Response times
  * Response codes
  * Dependant API response times & codes
* Separating domain models and response models
* Input validation
* Refactor `TranslationService` to inject rulesets for different translations
