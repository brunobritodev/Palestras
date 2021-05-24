using Microsoft.IdentityModel.Tokens;
using NetDevPack.Security.JwtSigningCredentials.Model;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace JOSE.Algoritmos
{
    // ReSharper disable once InconsistentNaming
    internal class HmacSignature : IJsonWebKey
    {
        private static RandomNumberGenerator _rng = RandomNumberGenerator.Create();
        private string MyJwkLocation() => Path.Combine(Environment.CurrentDirectory, $"mysupersecrethmac_{Keysize}.json");

        public string Algorithm { get; set; }
        public string AlgoritmType { get; set; }
        public int Keysize { get; }

        public HmacSignature(int keysize = 64)
        {
            Algorithm = "HMACSHA256";
            AlgoritmType = "HMAC";
            Keysize = keysize;
        }

        private JsonWebKey CreateJWK()
        {
            var key = (HMAC)new HMACSHA256(CreateRandomKey(64));
            var jwk = JsonWebKeyConverter.ConvertFromSymmetricSecurityKey(new SymmetricSecurityKey(key.Key));
            SaveKey(jwk);
            return jwk;
        }

        private void SaveKey(JsonWebKey key)
        {
            File.WriteAllText(MyJwkLocation(), JsonSerializer.Serialize(key));
        }

        public string JwaDetails()
        {
            return $@"KeySize: {Keysize}";
        }

        // ReSharper disable once InconsistentNaming
        public byte[] Sign(string content)
        {
            var key = Get();
            var cryptoProv = new CryptoProviderFactory();
            var provider = cryptoProv.CreateForSigning(key, Algorithm);

            return provider.Sign(Encoding.UTF8.GetBytes(content));
        }
        public string PublicKey()
        {
            var jsonWebKey = JsonSerializer.Deserialize<JsonWebKey>(File.ReadAllText(MyJwkLocation()));

            return JsonSerializer.Serialize(new PublicJsonWebKey(jsonWebKey), new JsonSerializerOptions() { WriteIndented = true, IgnoreNullValues = true });
        }

        public string PrivateKey()
        {
            var jsonWebKey = JsonSerializer.Deserialize<JsonWebKey>(File.ReadAllText(MyJwkLocation()));

            return JsonSerializer.Serialize(jsonWebKey, new JsonSerializerOptions() { WriteIndented = true, IgnoreNullValues = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }

        private JsonWebKey Get()
        {
            if (File.Exists(MyJwkLocation()))
            {
                return JsonSerializer.Deserialize<JsonWebKey>(File.ReadAllText(MyJwkLocation()));
            }

            return CreateJWK();

        }

        /// <summary>Creates a random key byte array.</summary>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        internal byte[] CreateRandomKey(int length)
        {
            byte[] data = new byte[length];
            _rng.GetBytes(data);
            return data;
        }

    }
}