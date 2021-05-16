using System;
using System.Net.Http;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Notion.SDK.Tests
{
    public class NotionClientTest
    {
        [Fact]
        public async Task TestGetPage()
        {
            var (client, authToken) = GetClientWithToken();
            var page = await client.GetPage("be32c99dddb349609fd086d38babd537", authToken);

            page.Id.ShouldNotBe(Guid.Empty);
            page.Object.ShouldBe("page");
            page.CreatedTime.ShouldBeGreaterThan(new DateTime(2021, 05, 01));
            page.LastEditedTime.ShouldBeGreaterThan(new DateTime(2021, 05, 01));
            page.Archived.ShouldBeFalse();
            page.Parent.Type.ShouldBe("workspace");
            var titlePageProperty = page.Properties["title"].ShouldBeOfType<TitlePropertyValue>();
            titlePageProperty.Id.ShouldBe("title");
            titlePageProperty.Type.ShouldBe("title");
            titlePageProperty.Text.ShouldNotBeEmpty();
        }

        [Fact]
        public async Task TestGetBlockChildren()
        {
            var (client, authToken) = GetClientWithToken();
            var objectList = await client.GetBlockChildren("be32c99dddb349609fd086d38babd537", authToken);
            objectList.Object.ShouldBe("list");
            objectList.HasMore.ShouldBeFalse();
            objectList.NextCursor.ShouldBeNull();
            objectList.Results.ShouldNotBeEmpty();
        }

        [Fact]
        public async Task TestGetParagraphBlock()
        {
            var (client, authToken) = GetClientWithToken();
            var objectList = await client.GetBlockChildren("be32c99dddb349609fd086d38babd537", authToken);
            var paragraphBlock = Assert.IsType<ParagraphBlock>(objectList.Results[0]);
            paragraphBlock.Id.ShouldNotBe(Guid.Empty);
            paragraphBlock.Object.ShouldBe("block");
            paragraphBlock.CreatedTime.ShouldBeGreaterThan(new DateTime(2021, 05, 01));
            paragraphBlock.LastEditedTime.ShouldBeGreaterThan(new DateTime(2021, 05, 01));
            paragraphBlock.HasChildren.ShouldBeFalse();
        }

        private static (NotionClient, string) GetClientWithToken()
        {
            var authToken = Environment.GetEnvironmentVariable("NOTION_API_KEY");
            var client = new NotionClient(new HttpClient(new SocketsHttpHandler()));
            return (client, authToken);
        }
    }
}