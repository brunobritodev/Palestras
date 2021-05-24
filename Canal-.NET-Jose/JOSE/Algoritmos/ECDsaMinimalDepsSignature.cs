using Microsoft.IdentityModel.Tokens;
using NetDevPack.Security.JwtSigningCredentials.Model;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace JOSE
{
    // ReSharper disable once InconsistentNaming
    internal class ECDsaMinimalDepsSignature : IJsonWebKey
    {
        private static RandomNumberGenerator _rng = RandomNumberGenerator.Create();
        private static readonly string MyJwkLocation = Path.Combine(Environment.CurrentDirectory, $"mysupersecretecdsa.json");

        public ECCurve Curve { get; set; }
        public string Algorithm { get; set; }
        public string AlgoritmType { get; set; }

        public ECDsaMinimalDepsSignature(ECCurve? curve = null, string algorithm = "ES256", string algoritmType = "ECDsa")
        {
            Curve = curve ?? ECCurve.NamedCurves.nistP256;
            Algorithm = algorithm;
            AlgoritmType = algoritmType;
        }

        private ECDsa CreateJWK()
        {
            var key = ECDsa.Create(Curve);
            SaveKey(key);
            return key;
        }

        private void SaveKey(ECDsa key)
        {
            var parameters = key.ExportParameters(true);
            var id = CreateUniqueId();
            var jwk = new JsonWebKey()
            {
                Kty = JsonWebAlgorithmsKeyTypes.EllipticCurve,
                Use = "sig",
                Kid = id,
                KeyId = id,
                X = Base64UrlEncoder.Encode(parameters.Q.X),
                Y = Base64UrlEncoder.Encode(parameters.Q.Y),
                D = Base64UrlEncoder.Encode(parameters.D),
                Crv = JsonWebKeyECTypes.P256,
                Alg = Algorithm
            };
            File.WriteAllText(MyJwkLocation, JsonSerializer.Serialize(jwk));
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
            return key.SignData(Encoding.UTF8.GetBytes(content), HashAlgorithmName.SHA256);
        }
        public string PublicKey()
        {
            var jsonWebKey = JsonSerializer.Deserialize<JsonWebKey>(File.ReadAllText(MyJwkLocation));

            return JsonSerializer.Serialize(new PublicJsonWebKey(jsonWebKey), new JsonSerializerOptions() { WriteIndented = true, IgnoreNullValues = true });
        }

        public string PrivateKey()
        {
            var jsonWebKey = JsonSerializer.Deserialize<JsonWebKey>(File.ReadAllText(MyJwkLocation));

            return JsonSerializer.Serialize(jsonWebKey, new JsonSerializerOptions() { WriteIndented = true, IgnoreNullValues = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }

        private ECDsa Get()
        {
            if (File.Exists(MyJwkLocation))
            {
                var jsonWebKey = JsonSerializer.Deserialize<JsonWebKey>(File.ReadAllText(MyJwkLocation));
                var parameters = new ECParameters
                {
                    Curve = ECCurve.NamedCurves.nistP256,
                    D = Base64UrlEncoder.DecodeBytes(jsonWebKey.D),
                    Q = new ECPoint()
                    {
                        X = Base64UrlEncoder.DecodeBytes(jsonWebKey.X),
                        Y = Base64UrlEncoder.DecodeBytes(jsonWebKey.Y),
                    }
                };
                return ECDsa.Create(parameters);
            }

            return CreateJWK();

        }

        string CreateUniqueId(int length = 16)
        {
            return Base64UrlEncoder.Encode(CreateRandomKey(length));
        }
        byte[] CreateRandomKey(int length)
        {
            byte[] data = new byte[length];
            _rng.GetBytes(data);
            return data;
        }
    }
}