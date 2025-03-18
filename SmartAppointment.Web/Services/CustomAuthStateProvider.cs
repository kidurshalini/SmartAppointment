////using Microsoft.AspNetCore.Components.Authorization;
////using Blazored.LocalStorage;
////using System.Security.Claims;
////using System.Threading.Tasks;

////public class CustomAuthStateProvider : AuthenticationStateProvider
////{
////    private readonly ILocalStorageService _localStorage;

////    public CustomAuthStateProvider(ILocalStorageService localStorage)
////    {
////        _localStorage = localStorage;
////    }

////    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
////    {
////        // Retrieve the token from local storage
////        var token = await _localStorage.GetItemAsync<string>("authToken");

////        if (string.IsNullOrEmpty(token))
////        {
////            // If no token, return an empty authentication state
////            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
////        }

////        // Create claims with the token
////        var claims = new List<Claim>
////        {
////            new Claim(ClaimTypes.NameIdentifier, "userId"), // Replace with actual user ID
////            new Claim("access_token", token) // Store the token in the claims
////        };

////        var identity = new ClaimsIdentity(claims, "Bearer");
////        var principal = new ClaimsPrincipal(identity);

////        return new AuthenticationState(principal);
////    }

////    public async Task NotifyUserAuthentication(string token)
////    {
////        // Save the token to local storage
////        await _localStorage.SetItemAsync("authToken", token);

////        // Create claims with the token
////        var claims = new List<Claim>
////        {
////            new Claim(ClaimTypes.NameIdentifier, "userId"), // Replace with actual user ID
////            new Claim("access_token", token) // Store the token in the claims
////        };

////        var identity = new ClaimsIdentity(claims, "Bearer");
////        var principal = new ClaimsPrincipal(identity);

////        // Notify that the authentication state has changed
////        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
////    }

////    public async Task NotifyUserLogout()
////    {
////        // Remove the token from local storage
////        await _localStorage.RemoveItemAsync("authToken");

////        // Notify that the authentication state has changed (user is logged out)
////        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()))));
////    }
////}

//using Microsoft.AspNetCore.Components.Authorization;
//using Blazored.LocalStorage;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Threading.Tasks;
//using System.Linq;

//public class CustomAuthStateProvider : AuthenticationStateProvider
//{
//    private readonly ILocalStorageService _localStorage;

//    public CustomAuthStateProvider(ILocalStorageService localStorage)
//    {
//        _localStorage = localStorage;
//    }

//    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
//    {
//        // Retrieve the token from local storage
//        var token = await _localStorage.GetItemAsync<string>("authToken");

//        if (string.IsNullOrEmpty(token))
//        {
//            // If no token, return an empty authentication state (user is not authenticated)
//            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
//        }

//        // Decode the token and extract claims
//        var claims = ParseTokenClaims(token);

//        if (claims == null || !claims.Any())
//        {
//            // If the token is invalid or has no claims, return an empty authentication state
//            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
//        }

//        // Create a ClaimsIdentity and ClaimsPrincipal
//        var identity = new ClaimsIdentity(claims, "Bearer");
//        var principal = new ClaimsPrincipal(identity);

//        return new AuthenticationState(principal);
//    }

//    public async Task NotifyUserAuthentication(string token)
//    {
//        // Save the token to local storage
//        await _localStorage.SetItemAsync("authToken", token);

//        // Decode the token and extract claims
//        var claims = ParseTokenClaims(token);

//        if (claims == null || !claims.Any())
//        {
//            // If the token is invalid or has no claims, notify logout
//            await NotifyUserLogout();
//            return;
//        }

//        // Create a ClaimsIdentity and ClaimsPrincipal
//        var identity = new ClaimsIdentity(claims, "Bearer");
//        var principal = new ClaimsPrincipal(identity);

//        // Notify that the authentication state has changed
//        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
//    }

//    public async Task NotifyUserLogout()
//    {
//        // Remove the token from local storage
//        await _localStorage.RemoveItemAsync("authToken");

//        // Notify that the authentication state has changed (user is logged out)
//        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()))));
//    }

//    private IEnumerable<Claim> ParseTokenClaims(string token)
//    {
//        try
//        {
//            // Decode the JWT token
//            var handler = new JwtSecurityTokenHandler();
//            var jwtToken = handler.ReadJwtToken(token);

//            // Extract claims from the token
//            return jwtToken.Claims;
//        }
//        catch (Exception ex)
//        {
//            // Log or handle the error (e.g., invalid token format)
//            Console.WriteLine($"Error parsing token: {ex.Message}");
//            return null;
//        }
//    }
//}
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorage;

    public CustomAuthStateProvider(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await _localStorage.GetItemAsync<string>("authToken");

        if (string.IsNullOrEmpty(token))
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        var claims = ParseTokenClaims(token);
        if (claims == null || !claims.Any())
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        var identity = new ClaimsIdentity(claims, "Bearer");
        var principal = new ClaimsPrincipal(identity);

        return new AuthenticationState(principal);
    }

    public async Task NotifyUserAuthentication(string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            throw new ArgumentException("Invalid token received.");
        }

        await _localStorage.SetItemAsync("authToken", token);

        var claims = ParseTokenClaims(token);
        if (claims == null || !claims.Any())
        {
            await NotifyUserLogout();
            return;
        }

        var identity = new ClaimsIdentity(claims, "Bearer");
        var principal = new ClaimsPrincipal(identity);

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
    }

    public async Task NotifyUserLogout()
    {
        await _localStorage.RemoveItemAsync("authToken");
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()))));
    }

    private IEnumerable<Claim> ParseTokenClaims(string token)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var claims = jwtToken.Claims.ToList();
            claims.Add(new Claim("access_token", token)); // Ensure the token itself is stored as a claim

            return claims;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error parsing token: {ex.Message}");
            return null;
        }
    }
}
