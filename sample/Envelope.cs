using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Avro.Generic;
using Avro.IO;

namespace DigitalSignature101
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
            var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(publicKey);
            var rsaDeformatter = new RSAPKCS1SignatureDeformatter(rsa);
            rsaDeformatter.SetHashAlgorithm(hashAlgorithm);
            var mySHA256 = SHA256.Create();
            //The hash value to sign.
            byte[] messageBytes = mySHA256.ComputeHash(message.value);

            if(rsaDeformatter.VerifySignature(messageBytes, System.Convert.FromBase64String(signature)))
            {
                Console.WriteLine("The signature is valid.");
                //Optionally decode it 
                var schema = Avro.Schema.Parse(File.ReadAllText("twitter.avsc"));
                var datumReader = new GenericDatumReader<GenericRecord>(schema, schema);   
                var ms = new MemoryStream(message.value);
                var decoder = new BinaryDecoder(ms);
                GenericRecord rec = datumReader.Read(null, decoder);
                System.Console.WriteLine(rec);                

            }
            else
            {
                Console.WriteLine("The signature is not valid.");
            }
        }
    }


}
