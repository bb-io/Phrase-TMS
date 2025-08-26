using Apps.PhraseTMS.Dtos;
using Apps.PhraseTMS.Models.Users.Requests;
using Apps.PhraseTMS.Models.Users.Response;
using Apps.PhraseTMS.Polling.Models;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Polling;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.PhraseTMS.Polling
{
    [PollingEventList]
    public class UserPollingList(InvocationContext invocationContext) : PhraseInvocable(invocationContext)
    {
        [PollingEvent("On users created", "Triggered when new users are created")]
        public async Task<PollingEventResponse<PollingMemory, ListAllUsersResponse>> OnUsersCreated(
        PollingEventRequest<PollingMemory> request,
        [PollingEventParameter] ListAllUsersQuery query)
        {
            if (request.Memory is null)
            {
                return new()
                {
                    FlyBird = false,
                    Memory = new()
                    {
                        LastPollingTime = DateTime.UtcNow
                    }
                };
            }

            var req = new RestRequest("/api2/v1/users", Method.Get);

            if (query is not null)
            {
                if (!string.IsNullOrWhiteSpace(query.firstName)) req.AddQueryParameter("firstName", query.firstName);
                if (!string.IsNullOrWhiteSpace(query.lastName)) req.AddQueryParameter("lastName", query.lastName);
                if (!string.IsNullOrWhiteSpace(query.name)) req.AddQueryParameter("name", query.name);
                if (!string.IsNullOrWhiteSpace(query.userName)) req.AddQueryParameter("userName", query.userName);
                if (!string.IsNullOrWhiteSpace(query.email)) req.AddQueryParameter("email", query.email);
                if (!string.IsNullOrWhiteSpace(query.nameOrEmail)) req.AddQueryParameter("nameOrEmail", query.nameOrEmail);

                if (query.role?.Any() == true)
                    foreach (var r in query.role)
                        req.AddQueryParameter("role", r);

                if (query.includeDeleted == true)
                    req.AddQueryParameter("includeDeleted", "true");

                if (query.sort?.Any() == true)
                    foreach (var s in query.sort)
                        req.AddQueryParameter("sort", s);

                if (query.order?.Any() == true)
                    foreach (var o in query.order)
                        req.AddQueryParameter("order", o);
            }

            var users = await Client.Paginate<UserDto>(req);

            var fromUtc = request.Memory.LastPollingTime;
            var nowUtc = DateTime.UtcNow;

            var createdNow = users
                .Where(u => u.DateCreated.HasValue)
                .Where(u =>
                {
                    var dc = u.DateCreated!.Value.ToUniversalTime();
                    return dc >= fromUtc && dc < nowUtc;
                })
                .OrderBy(u => u.DateCreated)
                .ToList();

            return new()
            {
                FlyBird = createdNow.Count > 0,
                Result = new ListAllUsersResponse { Users = createdNow },
                Memory = new()
                {
                    LastPollingTime = nowUtc
                }
            };
        }
    }
}
