using System;
using System.Security.Cryptography;
using System.Text;

namespace stamping
{
    public struct Envelope
    {
        public Envelope(Message message, string signature, string publicKey, string hashAlgorithm)
        {
            this.message = message;
            this.signature = signature;
            this.publicKey = publicKey;
            this.hashAlgorithm = hashAlgorithm;
        }

        Message message;
        string signature;
        string publicKey;
        string hashAlgorithm;

        public void Verify()
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(publicKey);
            RSAPKCS1SignatureDeformatter rsaDeformatter = new RSAPKCS1SignatureDeformatter(rsa);
            rsaDeformatter.SetHashAlgorithm(hashAlgorithm);
            SHA256 mySHA256 = SHA256.Create();
            //The hash value to sign.
            byte[] messageBytes = mySHA256.ComputeHash(Encoding.UTF8.GetBytes(message.value));

            if(rsaDeformatter.VerifySignature(messageBytes, System.Convert.FromBase64String(signature)))
            {
                Console.WriteLine("The signature is valid.");
            }
            else
            {
                Console.WriteLine("The signature is not valid.");
            }
        }
    }


}
