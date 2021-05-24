namespace JOSE
{
    public interface IJsonWebKey
    {
        byte[] Sign(string content);
        string PublicKey();
        string PrivateKey();
        string JwaDetails();
        string Algorithm { get; }
        string AlgoritmType { get; set; }
    }
}