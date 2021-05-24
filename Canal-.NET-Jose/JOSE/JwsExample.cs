using JOSE.Algoritmos;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace JOSE
{
    static class JwsExample
    {
        //private static readonly Assinaturas Assinatura = new(new ECDsaSignature(ECCurve.NamedCurves.nistP384, "ES384"));
        private static readonly Assinaturas Assinatura = new(new RSASignature());
        public static void Run()
        {
            var headerSegment = @"{
    ""typ"":""JWT"", 
    ""alg"":""ES256""
}";
            ShowHeader(headerSegment);

            var payloadRepresentation = new Dictionary<string, object>
            {
                { "claim1", 10 },
                { "claim2", "claim2-value" },
                { "name", "Bruno Brito" },
                { "given_name", "Bruno" },
                { "social", new Dictionary<string, string>()
                    {
                        { "facebook", "brunohbrito" },
                        { "google", "bhdebrito" }
                    }
                },
                { "logins", new[] {"brunohbrito", "bhdebrito", "bruno_hbrito"} },

            };
            var payloadSegment = JsonSerializer.Serialize(payloadRepresentation, new JsonSerializerOptions() { WriteIndented = true });

            ShowPayload(payloadSegment);

            // Convert para UTF-8
            var headerBytes = Encoding.UTF8.GetBytes(headerSegment);
            var payloadBytes = Encoding.UTF8.GetBytes(payloadSegment);
            ShowBytes(headerBytes, payloadBytes);

            // Converte para Base64Url
            var header = Base64UrlEncoder.Encode(headerBytes);
            var payload = Base64UrlEncoder.Encode(payloadBytes);
            ShowBase64Parts(header, payload);
            // Assinatura
            var signatureSegment = $"{header}.{payload}";

            ShowSignatureParts(header, payload);

            // Assina
            var signatureBytes = Assinatura.Selected.Sign(signatureSegment);

            // Transforma assinatura em Base64Url
            var signature = Base64UrlEncoder.Encode(signatureBytes);
            ShowSignatureFinals(signatureBytes, signature);

            // Gera o JWS
            ShowJws(signature, header, payload);

            ShowJwtIoInfo();
        }

        private static void ShowJwtIoInfo()
        {
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine("#######################      Validando no jwt.io      #######################");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Public Key: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(Assinatura.Selected.PublicKey());
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Private Key: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(Assinatura.Selected.PrivateKey());
            Console.ResetColor();
        }

        private static void ShowSignatureFinals(byte[] signatureBytes, string signature)
        {
            Console.WriteLine();
            Console.WriteLine("#######################      PASSO 4: Assinando      #######################");
            Console.WriteLine();
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Criptografia: ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(Assinatura.Selected.AlgoritmType);

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Algoritmo: ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(Assinatura.Selected.Algorithm);

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Detalhes Algoritmo: ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(Assinatura.Selected.JwaDetails());

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Assinatura: ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(PrintByteArray(signatureBytes));

            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine("#######################      PASSO 5: Transforma assinatura em Base64Url      #######################");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Assinatura: ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(signature);
            Console.ResetColor();
        }

        private static void ShowBytes(byte[] headerBytes, byte[] payloadBytes)
        {
            Console.WriteLine();
            Console.WriteLine("#######################      PASSO 1: Convertendo para UTF-8      #######################");
            Console.WriteLine();
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Header: ");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(PrintByteArray(headerBytes));
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Payload: ");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(PrintByteArray(payloadBytes));
            Console.ResetColor();
        }

        private static void ShowJws(string signature, string header, string payload)
        {
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine("#######################      PASSO 6: Criando o JWS      #######################");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Formato JWS: ");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("<header-base64url>.<payload-base64url>.<assinatura-base64url");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("JWS: ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(header);
            Console.ResetColor();
            Console.Write(".");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(payload);
            Console.ResetColor();
            Console.Write(".");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(signature);
            Console.ResetColor();
        }

        private static void ShowBase64Parts(string header, string payload)
        {
            Console.WriteLine();
            Console.WriteLine("#######################      PASSO 2: Transforma em Base64Url      #######################");
            Console.WriteLine();
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Header: ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(header);
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Payload: ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(payload);
            Console.ResetColor();
        }

        private static void ShowSignatureParts(string header, string payload)
        {

            Console.WriteLine();
            Console.WriteLine("#######################      PASSO 3: Gera a assinatura      #######################");
            Console.WriteLine();

            Console.ResetColor();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Formato da Assinatura: ");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("<header-base64url>.<payload-base64url>");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("O que sera assinado: ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(header);
            Console.ResetColor();
            Console.Write(".");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(payload);
            Console.ResetColor();
        }

        private static void ShowPayload(string payloadSegment)
        {
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Payload:");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(payloadSegment);
            Console.ResetColor();
        }

        private static void ShowHeader(string headerSegment)
        {
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Header: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(headerSegment);
            Console.ResetColor();
            Console.WriteLine();
            Console.ResetColor();
        }
        public static string PrintByteArray(byte[] bytes)
        {
            var sb = new StringBuilder("new byte[] { ");
            foreach (var b in bytes)
            {
                sb.Append(b + ", ");
            }
            sb.Append("}");
            return sb.ToString();
        }
    }
}
