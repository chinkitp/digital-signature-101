# Background 

The fundamental goals of a digital signature is to provide

- data integrity (knowing it has not been tampered with)
- authentication (knowing who the message is from)
- non-repudiation (knowing that the sender cannot deny having sent it)

```csharp
var message = "Greetings from Alice"

//Signature process
var key = CreateAsymmetricKey()
var secureHash = SHA256(message)
var signature = sign(secureHash, key.private)

//verification process
var secureHash = SHA256(message)
var isValid = verify(secureHash, key.public, signature)

```

# Overview of the solution
- An avro message as a ```byte[]``` is hashed with ```SHA256``` to produce a message digest.
- Using the **private key** of an asymmetric key pair; and the message digest a digital signature is produce. For convenience this is converted to ```Base64```
- An envelope is prepared with the digital signature, the **public key** and the original ```byte[]``` message
- The envelope is then verified by recomputing the message digest, and by using the public key and the signature.

## Avro Files
- ```twitter.avsc``` Avro schema of the example 
- ```twitter.avro``` data records in uncompressed binary Avro format 
data

# Technique
## Secure Hash
The secure hash function takes a stream of data and reduces it to a fixed size through a one-way mathematical function. The result is called a message digest and can be thought of as a fingerprint of the data. The message digest can be reproduced by any party with the same stream of data, but it is virtually impossible to create a different stream of data that produces the same message digest. A message digest can be used to provide integrity.

## Digital Signature 

Asymmetric key cryptography, also known as public key cryptography, uses a class of algorithms in which Alice has a private key, and Bob (and others) have her public key. The public and private keys are generated at the same time.

Digital Signatures. Alice can generate a digital signature for a message using a message digest and her private key. To authenticate Alice as the sender, Bob generates the message digest as well and uses Alice’s public key to validate the signature. If a different private key was used to generate the signature, the validation will fail.

In contrast to handwritten signatures, a digital signature also verifies the integrity of the data. If the data has been changed since the signature was applied, a different digest would be produced. This would result in a different signature. Therefore, if the data does not have integrity, the validation will fail.
In some circumstances, the digital signature can be used to establish non-repudiation. 

For more information visit [NIST: Introduction to Public Key Technology](https://nvlpubs.nist.gov/nistpubs/Legacy/SP/nistspecialpublication800-32.pdf)

# FAQs
## Why to sign hash instead of the original data? 

Digital signatures are usually applied to hash values that represent larger data. The reason for encrypting the hash instead of the entire message or document is that a hash function can convert an arbitrary input into a fixed length value, which is usually much shorter. This saves time as hashing is much faster than signing.

If the decrypted hash matches a second computed hash of the same data, it proves that the data hasn't changed since it was signed. If the two hashes don't match, the data has either been tampered with in some way (integrity) or the signature was created with a private key that doesn't correspond to the public key presented by the signer (authentication).
[Microsoft Cryptographic Signatures](https://docs.microsoft.com/en-us/dotnet/standard/security/cryptographic-signatures)

# Credits
[miguno/avro-cli-examples](https://github.com/miguno/avro-cli-examples)
[fearofcode/avro-byte-array-serialization](https://github.com/fearofcode/avro-byte-array-serialization/blob/master/src/main/java/org/wkh/learningavro/UserSerDe.java)