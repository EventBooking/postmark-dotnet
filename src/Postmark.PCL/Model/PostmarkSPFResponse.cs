namespace PostmarkDotNet.Model
{
    public class PostmarkSPFResponse
    {
        public bool SPFVerified { get; set; }
        public string SPFHost { get; set; }
        public string SPFTextValue { get; set; }
    }
}