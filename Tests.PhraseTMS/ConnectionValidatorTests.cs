using PhraseTMSTests.Base;
using Apps.PhraseTMS.Connections;
using Blackbird.Applications.Sdk.Common.Authentication;

namespace Tests.PhraseTMS;

[TestClass]
public class ConnectionValidatorTests : TestBaseMultipleConnections
{
    [TestMethod]
    public async Task ValidatesCorrectConnection()
    {
        // Arrange
        var validator = new ConnectionValidator();

        // Act
        var tasks = CredsGroups.Select(x => validator.ValidateConnection(x, CancellationToken.None).AsTask());
        var results = await Task.WhenAll(tasks);

        // Assert
        Assert.IsTrue(results.All(x => x.IsValid));
    }

    [TestMethod]
    public async Task DoesNotValidateIncorrectConnection()
    {
        // Arrange
        var validator = new ConnectionValidator();
        var newCreds = CredsGroups.First().Select(
            x => new AuthenticationCredentialsProvider(x.KeyName, x.Value + "_incorrect")
        );

        // Act
        var result = await validator.ValidateConnection(newCreds, CancellationToken.None);

        // Assert
        Assert.IsFalse(result.IsValid);
    }
}