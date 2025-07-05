using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

public class JwtAuthenticationHandler : DelegatingHandler
{
    private readonly byte[] _secretKey;

    public JwtAuthenticationHandler(byte[] secretKey)
    {
        _secretKey = secretKey;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var authHeader = request.Headers.Authorization;

        if (authHeader != null && authHeader.Scheme == "Bearer")
        {
            var token = authHeader.Parameter;
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(_secretKey),
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out _);

                Thread.CurrentPrincipal = principal;
                if (System.Web.HttpContext.Current != null)
                    System.Web.HttpContext.Current.User = principal;
            }
            catch (Exception)
            {
                return request.CreateResponse(HttpStatusCode.Unauthorized, "Invalid or expired token.");
            }
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
