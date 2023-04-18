using Apps.PhraseTMS.Models.Files.Requests;
using Apps.PhraseTMS.Models.Files.Responses;
using Apps.PhraseTms;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apps.PhraseTMS.Models.Quotes.Request;
using System.Collections;
using Apps.PhraseTMS.Dtos;
using Apps.PhraseTMS.Models.Quotes.Requests;

namespace Apps.PhraseTMS.Actions
{
    [ActionList]
    public class QuoteActions
    {
        [Action("Get quote", Description = "Get quote")]
        public QuoteDto GetQuote(string url, AuthenticationCredentialsProvider authenticationCredentialsProvider,
            [ActionParameter] GetQuoteRequest input)
        {
            var client = new PhraseTmsClient(url);
            var request = new PhraseTmsRequest($"/api2/v1/quotes/{input.QuoteUId}", Method.Get, authenticationCredentialsProvider.Value);
            var response = client.Get<QuoteDto>(request);
            return response;
        }

        [Action("Create quote", Description = "Create quote")]
        public QuoteDto CreateQuote(string url, AuthenticationCredentialsProvider authenticationCredentialsProvider,
            [ActionParameter] CreateQuoteRequest input)
        {
            var client = new PhraseTmsClient(url);
            var request = new PhraseTmsRequest($"/api2/v2/quotes", Method.Post, authenticationCredentialsProvider.Value);
            request.AddJsonBody(new
            {
                analyse = new { id = input.AnalyseUId },
                name = input.Name,
                priceList = new { id = input.PriceListUId },
                project = new { uid = input.ProjectUId }
            });
            return client.Execute<QuoteDto>(request).Data;
        }

        [Action("Delete quote", Description = "Delete quote")]
        public void DeleteQuote(string url, AuthenticationCredentialsProvider authenticationCredentialsProvider,
            [ActionParameter] GetQuoteRequest input)
        {
            var client = new PhraseTmsClient(url);
            var request = new PhraseTmsRequest($"/api2/v1/quotes/{input.QuoteUId}", Method.Delete, authenticationCredentialsProvider.Value);
            client.Execute(request);       
        }
    }
}
