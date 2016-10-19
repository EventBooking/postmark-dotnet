using NUnit.Framework;
using PostmarkDotNet;
using PostmarkDotNet.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Postmark.PCL.Tests
{
    [TestFixture]
    public class AdminClientDomainTests : ClientBaseFixture
    {
        private PostmarkAdminClient _adminClient;
        private string _domainName;
        private string _returnPath;

        protected override async Task SetupAsync()
        {
            _adminClient = new PostmarkAdminClient(WRITE_ACCOUNT_TOKEN);
            _domainName = WRITE_TEST_DOMAIN_NAME;
            _returnPath = "return." + _domainName;
            await CompletionSource;
        }


        [TestFixtureTearDown]
        [TestFixtureSetUp]
        public void Cleanup()
        {
            try
            {
                var domains = Task.Run(async () => await _adminClient.GetDomainsAsync()).Result;
                var pendingDeletes = new List<Task>();
                foreach (var f in domains.Domains)
                {
                    if (Regex.IsMatch(f.Name, _domainName))
                    {
                        var deleteTask = _adminClient.DeleteDomainAsync(f.ID);
                        pendingDeletes.Add(deleteTask);
                    }
                }
                Task.WaitAll(pendingDeletes.ToArray());
            }
            catch
            {
                //don't fail the test run if deleting all these wasn't possible.
            }
        }

        [TestCase]
        public async void AdminClient_CanGetDomain()
        {
            var domainResult = await _adminClient.GetDomainsAsync();
            var retrievedDomain = await _adminClient.GetDomainAsync(domainResult.Domains.First().ID);

            Assert.IsNotNull(retrievedDomain);
            Assert.AreEqual(retrievedDomain.ID, domainResult.Domains.First().ID);
        }


        [TestCase]
        public async void AdminClient_CanCreateDomain()
        {
            var domain = await _adminClient
                .CreateDomainAsync(_domainName, _returnPath);

            Assert.NotNull(domain);
            Assert.AreEqual(_domainName, domain.Name);
            Assert.AreEqual(_returnPath, domain.ReturnPathDomain);
        }


        [TestCase]
        public async void AdminClient_ShouldProduceErrorStatusForInvalidDomainName()
        {
            var threwExpectedException = false;
            try
            {
                await _adminClient.CreateDomainAsync("thisnamehasnodot");
            }
            catch (PostmarkValidationException ex)
            {
                threwExpectedException = true;
                Assert.AreEqual(PostmarkStatus.UserError, ex.Response.Status);
            }

            Assert.True(threwExpectedException);
        }


        [TestCase]
        public async void AdminClient_CanDeleteDomain()
        {
            var deleteTestDomain = "delete-" + _domainName;
            var domain = await _adminClient.CreateDomainAsync(deleteTestDomain);

            var response = await _adminClient.DeleteDomainAsync(domain.ID);
            Assert.AreEqual(PostmarkStatus.Success, response.Status);
            Assert.AreEqual(0, response.ErrorCode);
        }


        [TestCase]
        public async void AdminClient_CanListDomains()
        {
            var domainResult = await _adminClient.GetDomainsAsync();
            Assert.Greater(domainResult.Domains.Count(), 0);
        }

        [TestCase]
        public async void AdminClient_CanUpdateDomains()
        {
            var updatePrefix = "update-";
            var updateDomain = updatePrefix + _domainName;

            var initialReturnPath = "return." + updateDomain;

            var domain = await _adminClient.CreateDomainAsync(updateDomain, initialReturnPath);

            var updateResult = await _adminClient.UpdateDomainAsync(domain.ID, updatePrefix + initialReturnPath);

            var updatedDomain = await _adminClient.GetDomainAsync(domain.ID);

            Assert.AreEqual(updateResult.Name, updatedDomain.Name);
            Assert.AreEqual(updatePrefix + domain.ReturnPathDomain, updatedDomain.ReturnPathDomain);
        }

        [TestCase]
        [Ignore("DKIM rotation requires initially verified DKIM")]
        public async void AdminClient_CanRequestNewDKIMForDomain()
        {
            var domain = await _adminClient.CreateDomainAsync(_domainName);
            var response = await _adminClient.RotateDomainDKIMAsync(domain.ID);
            Assert.AreNotEqual(domain.DKIMPendingHost, response.DKIMPendingHost);
        }

        [TestCase]
        public async void AdminClient_CanVerifySPF()
        {
            var spfDomainName = "spf-verify-" + _domainName;
            var domain = await _adminClient.CreateDomainAsync(spfDomainName);
            var response = await _adminClient.VerifyDomainSPF(domain.ID);
            Assert.NotNull(response);
        }
    }
}
