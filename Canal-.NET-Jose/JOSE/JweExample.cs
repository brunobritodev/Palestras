using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.Json;

namespace JOSE
{
    class JweExample
    {
        public static void Run()
        {
            var key = new RsaSecurityKey(RSA.Create(2048))
            {
                KeyId = Guid.NewGuid().ToString()
            };
            var jweKey = new EncryptingCredentials(key, SecurityAlgorithms.RsaOAEP, SecurityAlgorithms.Aes128CbcHmacSha256);
            var payloadRepresentation = new List<Claim>()
            {
                new( "claim1", "10" ),
                new( "claim2", "claim2-value"),
                new ( "name", "Bruno Brito" ),
                new ( "given_name", "Bruno" ),
                new ("logins", "brunohbrito"),
                new ("logins", "bhdebrito"),
                new ("logins", "bruno_hbrito"),
            };

            var handler = new JsonWebTokenHandler();
            var now = DateTime.Now;
            var jwt = new SecurityTokenDescriptor
            {
                Issuer = "me",
                Audience = "you",
                IssuedAt = now,
                NotBefore = now,
                Expires = now.AddMinutes(5),
                Subject = new ClaimsIdentity(payloadRepresentation),
                EncryptingCredentials = jweKey
            };

            var jwe = handler.CreateToken(jwt);

            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("JWE: ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(jwe);
            Console.ResetColor();


            var result = handler.ValidateToken(jwe,
                new TokenValidationParameters
                {
                    ValidIssuer = "me",
                    ValidAudience = "you",
                    RequireSignedTokens = false,
                    TokenDecryptionKey = jweKey.Key
                });
            var claims = JsonSerializer.Serialize(result.Claims, new JsonSerializerOptions() { WriteIndented = true });

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Claims: ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(claims);
            Console.ResetColor();
        }
    }
}
