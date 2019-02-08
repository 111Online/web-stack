Use the same configuration for all other web projects.
Copy the IoC folde into any web project that need to implement Dependency Injection with structuremap.
Modify IoC.cs with the proper Registry name  return new Container(c => c.AddRegistry<REGISTRY_NAME>());