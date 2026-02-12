using PhraseTMSTests.Base;
using Apps.PhraseTMS.Connections;
using Apps.PhraseTMS.Constants;
using Blackbird.Applications.Sdk.Common.Authentication;

namespace Tests.PhraseTMS;

[TestClass]
public class ConnectionValidatorTests : TestBaseMultipleConnections
{
    [TestMethod]
    public async Task ValidatesCorrectConnection()
    {
        // Arrange
        var validator = new ConnectionValidator(InvocationContexts.First());

        // Act
        var tasks = CredsGroups.Select(x => validator.ValidateConnection(x, CancellationToken.None).AsTask());
        var results = await Task.WhenAll(tasks);

        // Assert
        PrintResult(results);
        Assert.IsTrue(results.All(x => x.IsValid));
    }

    [TestMethod]
    public async Task DoesNotValidateIncorrectConnection()
    {
        // Arrange
        var validator = new ConnectionValidator(InvocationContexts.First());
        var newCreds = CredsGroups.First().Select(
            x =>
            {
                if (x.KeyName == CredsNames.Url)
                {
                    return x;
                }

                return new AuthenticationCredentialsProvider(x.KeyName, x.Value + "_incorrect");
            }
        );

        // Act
        var result = await validator.ValidateConnection(newCreds, CancellationToken.None);

        // Assert
        PrintResult(result);
        Assert.IsFalse(result.IsValid);
    }
}