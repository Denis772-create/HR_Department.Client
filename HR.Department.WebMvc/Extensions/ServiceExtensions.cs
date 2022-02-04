using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;

namespace HR.Department.WebMvc.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureHttpClient(this IServiceCollection services) =>
            services.AddHttpClient("MvcClient", client =>
            {
                client.BaseAddress = new Uri("https://localhost:5001/api/");
                client.DefaultRequestHeaders.Add(HeaderNames.Accept, "*/*");
                client.DefaultRequestHeaders.Add(HeaderNames.UserAgent, "HR.Department.MVC.Client");
            });

        public static void ConfigureOIdCAuthentication(this IServiceCollection services) =>
            services.AddAuthentication(config =>
                {
                    config.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    config.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                })
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, config =>
                {
                    config.Authority = "https://localhost:6001";
                    config.ClientId = "hr_department_mvc_id";
                    config.ClientSecret = "hr_department_mvc_secret";
                    config.SaveTokens = true;
                    config.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false
                    };

                    config.ResponseType = "code";

                    config.Scope.Add("DepartmentAPI");

                    config.GetClaimsFromUserInfoEndpoint = true;

                    config.ClaimActions.MapJsonKey(ClaimTypes.DateOfBirth, ClaimTypes.DateOfBirth);
                });

        public static void ConfigureAuthorization(this IServiceCollection services) =>
            services.AddAuthorization(config =>
                config.AddPolicy("HasDateOfBirth", builder =>
                    builder.RequireClaim(ClaimTypes.DateOfBirth)));
    }
}
