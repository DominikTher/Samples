# Samples

- **Dependency Injection:** Some things from Pluralsight, [Dependency Injection](https://docs.google.com/document/d/1aNtQjJEbRGTZU_z9rpe4OQ9U5NtPwdxzIqOwQU0W4uY/)

- **Simple API:** Example of custom middleware and the possibility to create a response without controllers.
This is not a real example, just for learning new things. For response I use ```System.Text.Json``` because [Microsoft, migration 22 to 30](https://docs.microsoft.com/en-us/aspnet/core/migration/22-to-30?view=aspnetcore-3.1&tabs=visual-studio#newtonsoftjson-jsonnet-support)

- **GraphQLAuth.Api**
```TokenController``` generate tokens for GraphQL schemas and also generate token for itself, check code.
Check project dependencies, mainly GraphQL
  - [x] Multiple GraphQL schema endpoint
  - [x] Support [Authorize] and [AllowAnonymous]
  - [x] Each GraphQL schema has own AuthenticationScheme
  - [x] Support IValidationRules
  - [x] Claims - CustomRequirements ```MinimumAgeRequirement```, JWT Bearer Token Claims ```AddGraphQLAuth```