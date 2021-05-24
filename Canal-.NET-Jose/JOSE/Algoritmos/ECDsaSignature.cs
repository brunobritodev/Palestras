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
    internal class ECDsaSignature : IJsonWebKey
    {
        private static RandomNumberGenerator _rng = RandomNumberGenerator.Create();
        private string MyJwkLocation() => Path.Combine(Environment.CurrentDirectory, $"mysupersecretecdsa_{Algorithm}.json");
        public ECCurve Curve { get; set; }
        public string Algorithm { get; set; }
        public string AlgoritmType { get; set; }

        public ECDsaSignature(ECCurve? curve = null, string algorithm = "ES256", string algoritmType = "ECDsa")
        {
            Curve = curve ?? ECCurve.NamedCurves.nistP256;
            Algorithm = algorithm;
            AlgoritmType = algoritmType;
        }

        private JsonWebKey CreateJWK()
        {
            var key = new ECDsaSecurityKey(ECDsa.Create(Curve))
            {
                KeyId = Guid.NewGuid().ToString()
            };
            var jwk = JsonWebKeyConverter.ConvertFromECDsaSecurityKey(key);
            SaveKey(jwk);
            return jwk;
        }

        private void SaveKey(JsonWebKey key)
        {
            File.WriteAllText(MyJwkLocation(), JsonSerializer.Serialize(key));
        }

        public string JwaDetails()
        {
            return $@"Curve: {Curve.Oid.Value}
CurveType: {Curve.Oid.FriendlyName}
";
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
        byte[] CreateRandomKey(int length)
        {
            byte[] data = new byte[length];
            _rng.GetBytes(data);
            return data;
        }
    }
}