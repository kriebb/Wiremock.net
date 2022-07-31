using System.Net;
using WireMock;
using WireMock.Logging;
using WireMock.Net.Xunit;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Settings;
using Xunit.Abstractions;

namespace WireMockNetSampleTests
{
    public class GivenAWireMockServer
    {

        [Fact]
        public async Task WhenSendingAGetRequestTo_foo_ReceiveAResponse_bar()
        {
            //Arrange
            var wireMockServer = WireMock.Server.WireMockServer.Start();
            wireMockServer.Given(Request.Create()
                .UsingGet()
                .WithPath("/foo")
            ).RespondWith(Response.Create()
                .WithStatusCode(HttpStatusCode.OK)
                .WithBody("bar"));

            var httpClient = wireMockServer.CreateClient();
            
            //Act
            var barResponse = await httpClient.GetAsync("foo");
            var body = await barResponse.Content.ReadAsStringAsync();
            
            //Assert
            Assert.Equal(HttpStatusCode.OK, barResponse.StatusCode);
            Assert.Equal("bar", body);

        }
    }
}