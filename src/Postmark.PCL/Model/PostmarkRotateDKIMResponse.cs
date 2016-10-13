namespace PostmarkDotNet.Model
{
    public class PostmarkRotateDKIMResponse : PostmarkBaseDomain
    {
        public string DKIMHost { get; set; }
        public string DKIMTextValue { get; set; }
        public string DKIMPendingHost { get; set; }
        public string DKIMPendingTextValue { get; set; }
        public string DKIMRevokedHost { get; set; }
        public string DKIMRevokedTextValue { get; set; }
        public bool SafeToRemoveRevokedKeyFromDNS { get; set; }
        public string DKIMUpdateStatus { get; set; }
    }
}