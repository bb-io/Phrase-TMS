using Apps.PhraseTMS.Actions;
using Apps.PhraseTMS.Models.Users.Requests;
using Newtonsoft.Json;
using PhraseTMSTests.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.PhraseTMS
{
    [TestClass]
    public class UserTests :TestBase
    {
        [TestMethod]
        public async Task SearchUsers_IsSuccess()
        {
            var actions = new UserActions(InvocationContext);
            var result = await actions.ListAllUsers(new ListAllUsersQuery { role= ["ADMIN"], includeDeleted=false });
            Assert.IsNotNull(result);
            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
        } 
    }
}
