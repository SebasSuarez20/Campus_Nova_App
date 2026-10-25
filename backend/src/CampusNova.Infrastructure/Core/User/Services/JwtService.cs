using CampusNova.Application.Common.Interfaces;
using CampusNova.Domain.User.Model;
using CampusNova.Domain.User.Model.ReadModel;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Experimental;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CampusNova.Infrastructure.Core.User.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _config;

        public JwtService(IConfiguration config)
            => _config = config;

        /// <summary>
        /// Genera el access token (firmado con Jwt:Key).
        /// </summary>
        public string GeneratorToken(LoginAuthView user)
        {
            var claims = new[]
           {
                new Claim("authId", user.Identification!.ToString() ?? string.Empty),
                new Claim("institution",user.Institution_Code?.ToString() ?? string.Empty),
                new Claim(ClaimTypes.Email, user.Email?.ToString() ?? string.Empty),
                new Claim(ClaimTypes.Role, user.Rol?.ToString() ?? string.Empty)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));

            var credentials = new SigningCredentials(
                key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Genera el refresh token firmado con una clave DIFERENTE (Jwt:Key + sufijo).
        /// Esto garantiza que un access token NUNCA pase la validación de refresh,
        /// porque la firma no coincidirá. No se necesita ningún claim "type".
        /// </summary>
        public string GenerateRefreshToken(LoginAuthView user)
        {
            var claims = new[]
            {
                 new Claim("authId", user.Identification!.ToString() ?? string.Empty),
                new Claim("institution",user.Institution_Code?.ToString() ?? string.Empty),
                new Claim(ClaimTypes.Email, user.Email?.ToString() ?? string.Empty),
                new Claim(ClaimTypes.Role, user.Rol?.ToString() ?? string.Empty)
            };

            var test = _config["Jwt:Key"];

            // Clave diferente: la clave principal + un sufijo fijo.
            // Esto hace que la firma sea incompatible con el access token.
            var refreshKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]! + "-refresh-secret"));

            var credentials = new SigningCredentials(
                refreshKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                notBefore: DateTime.UtcNow,           // agregar esto
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Valida el refresh token usando la clave de refresh.
        /// Si llega un access token, la firma NO coincide y se rechaza automáticamente.
        /// </summary>
        public ClaimsPrincipal? ValidateRefreshToken(string token)
        {
            try
            {
                var refreshKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_config["Jwt:Key"]! + "-refresh-secret"));

                var validations = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = refreshKey,
                    ClockSkew = TimeSpan.Zero
                };

                // 1. Solo validar la firma — si lanza excepción, el token es inválido
                JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
                var handler = new JwtSecurityTokenHandler { MapInboundClaims = false };
                handler.ValidateToken(token, validations, out _); // ← solo para validar firma

                // 2. Leer claims manualmente del payload
                var parts = token.Split('.');
                var payloadJson = System.Text.Encoding.UTF8.GetString(
                    Convert.FromBase64String(
                        parts[1].PadRight(parts[1].Length + (4 - parts[1].Length % 4) % 4, '=')));

                var payloadDict = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(payloadJson);
                var claims = payloadDict!.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()!)).ToList();

                var identity = new ClaimsIdentity(claims, "jwt");
                return new ClaimsPrincipal(identity);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ValidateRefreshToken] Error: {ex.Message}");
                return null;
            }
        }
    }
}
