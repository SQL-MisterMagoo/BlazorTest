# BlazorSolidLogin

This is an experimental Solid Login component for the experimental Blazor project.
Not for use in production environments.

## Solid
Distributed Web : https://solid.inrupt.com/

### How to use this component

- [x] Add a reference to the BlazorSolidLogin project (or nupkg) to your existing Blazor Project.
- [x] Add a reference to the BlazorSolidLogin taghelper to your *_ViewImports.cshtml*

    ```@addTagHelper *, BlazorSolidLogin```
- [x] Add the BlazorSolidLogin Component to one of your Razor views - this is the login/logout control.

    ```<BlazorSolidLogin />```
- [x] Add the SolidIdentityService to your DI container in *startup.cs*

    ```services.AddTransient<ILoginNotifier>((a)=> new SolidIdentityService());```

### Runtime

- [x] Enter your WebId in the text input and click Login.

    e.g. ```https://mistermagoo.inrupt.net/profile/card#me```
