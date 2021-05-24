namespace JOSE
{
    /// <summary>
    /// Generate signatures with minimal dependecies from Microsoft.IdentityModel.Tokens
    /// </summary>
    class Assinaturas
    {
        public Assinaturas(IJsonWebKey jsonWebKey)
        {
            Selected =jsonWebKey;
        }
        public IJsonWebKey Selected { get; set; }

    }
}
