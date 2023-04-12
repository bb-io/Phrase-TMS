using Apps.PhraseTMS.Models.Quotes.Requests;
using Apps.PhraseTms;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apps.PhraseTMS.Models.Vendors.Requests;
using Apps.PhraseTMS.Dtos;
using Apps.PhraseTMS.Models.Responses;
using Apps.PhraseTMS.Models.Vendors.Response;

namespace Apps.PhraseTMS.Actions
{
    [ActionList]
    public class VendorActions
    {
        [Action("Add new vendor", Description = "Add new vendor")]
        public void AddVendor(string url, AuthenticationCredentialsProvider authenticationCredentialsProvider,
            [ActionParameter] AddVendorRequest input)
        {
            var client = new PhraseTmsClient(url);
            var request = new PhraseTmsRequest($"/api2/v1/vendors", Method.Post, authenticationCredentialsProvider.Value);
            request.AddJsonBody(new
            {
                vendorToken = input.VendorToken,
                priceList = new { uid = input.PriceListUId }
            });
            client.Execute(request);
        }

        [Action("List all vendors", Description = "List all vendors")]
        public ListVendorsResponse ListVendors(string url, AuthenticationCredentialsProvider authenticationCredentialsProvider)
        {
            var client = new PhraseTmsClient(url);
            var request = new PhraseTmsRequest($"/api2/v1/vendors", Method.Get, authenticationCredentialsProvider.Value);
            return new ListVendorsResponse()
            {
                Vendors = client.Get<ResponseWrapper<List<VendorDto>>>(request).Content
            };
        }

        [Action("Get vendor", Description = "Get vendor")]
        public VendorDto GetVendor(string url, AuthenticationCredentialsProvider authenticationCredentialsProvider,
            GetVendorRequest input)
        {
            var client = new PhraseTmsClient(url);
            var request = new PhraseTmsRequest($"/api2/v1/vendors/{input.VendorId}", Method.Get, authenticationCredentialsProvider.Value);
            return client.Get<VendorDto>(request);
        }
    }
}
