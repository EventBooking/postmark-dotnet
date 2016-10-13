namespace PostmarkDotNet.Model
{
    public class PostmarkBasicDomain : PostmarkBaseDomain
    {
        public bool SPFVerified { get; set; }
        public bool ReturnPathDomainVerified { get; set; }
    }
}