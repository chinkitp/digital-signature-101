using System.Security.Cryptography;
using System.Text;

namespace DigitalSignature101
{
    class Program
    {
        static void Main(string[] args)
        {
            var message = new Message("This is a good message");

            var envelope = Sign(message);

            envelope.Verify();

            
        }

        static Envelope Sign(Message m)
        {
            SHA256 mySHA256 = SHA256.Create();
            //The hash value to sign.
            byte[] messageBytes = mySHA256.ComputeHash(Encoding.UTF8.GetBytes(m.value));

            //Generate a public/private key pair.
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();

            //Create an RSAPKCS1SignatureFormatter object and pass it the
            //RSACryptoServiceProvider to transfer the private key.
            RSAPKCS1SignatureFormatter rsaFormatter = new RSAPKCS1SignatureFormatter(rsa);

            //Set the hash algorithm to SHA1.
            rsaFormatter.SetHashAlgorithm("SHA256");

            //Create a signature for hashValue and assign it to
            //signedHashValue.
            var signedHashValue = rsaFormatter.CreateSignature(messageBytes);

            return new Envelope(m,System.Convert.ToBase64String(signedHashValue), rsa.ToXmlString(false),"SHA256");        
        }
        
    }


}
