using Apps.PhraseTms;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using RestSharp;
using Apps.PhraseTMS.Models.Vendors.Requests;
using Apps.PhraseTMS.Dtos;
using Apps.PhraseTMS.Models.Responses;
using Apps.PhraseTMS.Models.Vendors.Response;
using Blackbird.Applications.Sdk.Common.Actions;

namespace Apps.PhraseTMS.Actions
{
    [ActionList]
    public class VendorActions
    {
        [Action("Add new vendor", Description = "Add new vendor")]
        public VendorDto AddVendor(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] AddVendorRequest input)
        {
            var client = new PhraseTmsClient(authenticationCredentialsProviders);
            var request = new PhraseTmsRequest($"/api2/v1/vendors", Method.Post, authenticationCredentialsProviders);
            request.AddJsonBody(new
            {
                vendorToken = input.VendorToken,
                priceList = new { uid = input.PriceListUId }
            });
            return client.Execute<VendorDto>(request).Data;
        }

        [Action("List all vendors", Description = "List all vendors")]
        public ListVendorsResponse ListVendors(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders)
        {
            var client = new PhraseTmsClient(authenticationCredentialsProviders);
            var request = new PhraseTmsRequest($"/api2/v1/vendors", Method.Get, authenticationCredentialsProviders);
            return new ListVendorsResponse()
            {
                Vendors = client.Get<ResponseWrapper<List<VendorDto>>>(request).Content
            };
        }

        [Action("Get vendor", Description = "Get vendor")]
        public VendorDto GetVendor(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            GetVendorRequest input)
        {
            var client = new PhraseTmsClient(authenticationCredentialsProviders);
            var request = new PhraseTmsRequest($"/api2/v1/vendors/{input.VendorId}", Method.Get, authenticationCredentialsProviders);
            return client.Get<VendorDto>(request);
        }
    }
}
