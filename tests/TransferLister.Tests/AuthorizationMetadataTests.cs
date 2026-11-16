using Microsoft.AspNetCore.Authorization;
using Transfer_ListerWebApp.Controllers;

namespace TransferLister.Tests;

public class AuthorizationMetadataTests
{
    [Fact]
    public void TransferListController_RequiresAuthorizationByDefault()
    {
        var authorizeAttribute = Attribute.GetCustomAttribute(
            typeof(TransferListerWebAppController),
            typeof(AuthorizeAttribute));

        Assert.NotNull(authorizeAttribute);
    }

    [Fact]
    public void IndexAction_AllowsAnonymousAccess()
    {
        var indexAction = typeof(TransferListerWebAppController)
            .GetMethod(nameof(TransferListerWebAppController.Index));

        Assert.NotNull(indexAction);

        var allowAnonymousAttribute = Attribute.GetCustomAttribute(
            indexAction,
            typeof(AllowAnonymousAttribute));

        Assert.NotNull(allowAnonymousAttribute);
    }
}