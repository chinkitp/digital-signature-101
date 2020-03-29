namespace DigitalSignature101
{
    public struct Message
    {
        public Message(byte[] value)
        {
            this.value = value;
        }
        public byte[] value;
    }
}
