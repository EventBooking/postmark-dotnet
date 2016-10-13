using System.Collections.Generic;

namespace PostmarkDotNet.Model
{
    public class PostmarkDomainList
    {
        public int TotalCount { get; set; }
        public IEnumerable<PostmarkBasicDomain> Domains { get; set; }
    }
}