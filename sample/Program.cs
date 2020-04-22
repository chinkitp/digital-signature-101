using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Avro.File;
using Avro.Generic;
using Avro.IO;

namespace DigitalSignature101
{
    class Program
    {
        static void Main(string[] args)
        {    
            var messages = new List<Message>();

            //Read & parse the avro schema
            var schema = Avro.Schema.Parse(File.ReadAllText("twitter.avsc"));
            DatumReader<GenericRecord> datumReader = new GenericDatumReader<GenericRecord>(schema, schema);

            //Open a file reader on the avro binary file with data
            var dataFileReader = Avro.File.DataFileReader<GenericRecord>.OpenReader("twitter.avro", schema);
            while (dataFileReader.HasNext()) 
            {
                var tweet = dataFileReader.Next();
                var writer = new GenericDatumWriter<GenericRecord>(schema);
                MemoryStream iostr = new MemoryStream();
                Avro.IO.Encoder e = new BinaryEncoder(iostr);
                writer.Write(tweet,e);
                var record = iostr.ToArray();
                //System.Console.WriteLine(record.Length);
                messages.Add(new Message(record));

            }

            foreach (var item in messages)
            {
                var envelope = Sign(item);
                envelope.Verify();
            }       
        }

        static Envelope Sign(Message m)
        {
            var hashAlgorithm = "SHA256";

            //The hash value to sign.
            SHA256 mySHA256 = SHA256.Create();
            byte[] hashBytes = mySHA256.ComputeHash(m.value);

            //Generate a public/private key pair.
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            //Store the public key
            string publicKey = rsa.ToXmlString(false);

            //Create an RSAPKCS1SignatureFormatter object and pass it the
            //RSACryptoServiceProvider to transfer the private key.
            RSAPKCS1SignatureFormatter rsaFormatter = new RSAPKCS1SignatureFormatter(rsa);

            //Set the hash algorithm.
            rsaFormatter.SetHashAlgorithm(hashAlgorithm);

            //Create a digital signature for hashValue
            var signedHashValue = rsaFormatter.CreateSignature(hashBytes);

            //Store the digital signature as Base64 string
            string digitalSignature = System.Convert.ToBase64String(signedHashValue);

            //Create the final envelope object.
            return new Envelope(m,digitalSignature, publicKey ,hashAlgorithm);        
        }
        
    }


}
