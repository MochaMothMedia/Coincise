# Coincise
Coincise is an open-source financial application that is intended to be highly extendable and relatively easy to use.

> Coincise is in active development and is not intended for active use.

# Projects
## Core
This project contains the system's abstract ideas without any hard implementation. You can get an idea of what this system does by looking through the Core project without worrying about the details. Every other project is dependent on the Core, and Core is entirely abstract to accommodate this.

## Console Launcher
A Console-based launcher for the backend of the system. This is also where Dependency Injection is applied.

## Mongo Database
MongoDB implementation of the Core's database abstraction.

## Web API
ASPNET Web API that manages requests to the backend via endpoints.

## API Model
Defined Inputs and Outputs for the Web API.

## Blazor App
Blazor-based WASM application that houses the primary front-end view. Can be ported to multiple systems or used via the web.

> Any of these projects can be replaced with one or more new projects that collectively implement the required interfaces. The only change required would be in the Launcher when creating the service stack.

