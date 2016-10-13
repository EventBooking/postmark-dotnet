namespace PostmarkDotNet.Model
{
    public class PostmarkBaseDomain
    {
        public string Name { get; set; }
        public bool DKIMVerified { get; set; }
        public bool WeakDKIM { get; set; }
        public int ID { get; set; }
    }
}