using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Notion.SDK.Tests
{
    //todo test each block
    //todo test serialization/deserialization
    public class NotionClientTest
    {
        [Fact]
        public async Task TestGetPage()
        {
            var (client, authToken) = GetClientWithToken();
            var response = await client.GetPage("be32c99dddb349609fd086d38babd537", authToken);

            var page = response.GetValue();
            page.Id.ShouldNotBe(Guid.Empty);
            page.Object.ShouldBe("page");
            page.CreatedTime.ShouldBeGreaterThan(new DateTime(2021, 05, 01));
            page.LastEditedTime.ShouldBeGreaterThan(new DateTime(2021, 05, 01));
            page.Archived.ShouldBeFalse();
            page.Parent.Type.ShouldBe(ParentType.Workspace);
            page.Url.ShouldBe("https://www.notion.so/PageInWorkspace-be32c99dddb349609fd086d38babd537");
            var titlePageProperty = page.Properties["title"].ShouldBeOfType<TitlePropertyValue>();
            titlePageProperty.Id.ShouldBe("title");
            titlePageProperty.Type.ShouldBe(PropertyValueType.Title);
            var text = titlePageProperty.Text.Single().ShouldBeOfType<Text>();
            text.Content.ShouldBe("PageInWorkspace");
            text.Link.ShouldBeNull();
        }

        [Fact]
        public async Task TestGetBlockChildren()
        {
            var (client, authToken) = GetClientWithToken();
            var response = await client.GetBlockChildren("be32c99dddb349609fd086d38babd537", authToken);

            var objectList = response.GetValue();
            objectList.Object.ShouldBe("list");
            objectList.HasMore.ShouldBeFalse();
            objectList.NextCursor.ShouldBeNull();
            objectList.Results.ShouldNotBeEmpty();
        }

        [Fact]
        public async Task TestGetParagraphBlock()
        {
            var (client, authToken) = GetClientWithToken();
            var response = await client.GetBlockChildren("be32c99dddb349609fd086d38babd537", authToken);

            var objectList = response.GetValue();
            var paragraphBlock = objectList.Results[0].ShouldBeOfType<Paragraph>();
            paragraphBlock.Id.ShouldNotBe(Guid.Empty);
            paragraphBlock.Object.ShouldBe("block");
            paragraphBlock.Type.ShouldBe(BlockType.Paragraph);
            paragraphBlock.CreatedTime.ShouldBeGreaterThan(new DateTime(2021, 05, 01));
            paragraphBlock.LastEditedTime.ShouldBeGreaterThan(new DateTime(2021, 05, 01));
            paragraphBlock.HasChildren.ShouldBeFalse();
            var text = paragraphBlock.Text.Single().ShouldBeOfType<Text>();
            text.Content.ShouldBe("Just text block");
            text.Link.ShouldBeNull();
            text.Href.ShouldBeNull();
            text.PlainText.ShouldBe("Just text block");
            text.Type.ShouldBe(RichTextType.Text);
            text.Annotations.Bold.ShouldBe(false);
            text.Annotations.Italic.ShouldBe(false);
            text.Annotations.Code.ShouldBe(false);
            text.Annotations.Color.ShouldBe("default");
            text.Annotations.Strikethrough.ShouldBe(false);
            text.Annotations.Underline.ShouldBe(false);
        }

        [Fact]
        public async Task TestSearch()
        {
            var (client, authToken) = GetClientWithToken();
            var response = await client.Search(authToken, filterObjectType: SearchFilterObjectType.Page);
            var list = response.GetValue();
            list.Results.ShouldNotBeEmpty();
            list.Results[0].Parent.Type.ShouldBe(ParentType.Workspace);
        }

        private static (NotionClient, string) GetClientWithToken()
        {
            // var authToken = Environment.GetEnvironmentVariable("NOTION_API_KEY");
            var authToken = "secret_zc0AbZDoam2lgjoothl0Oe9EaL7e9TgkxdZLf1e9Bbi";
            var client = new NotionClient(new HttpClient(new SocketsHttpHandler()));
            return (client, authToken);
        }
    }
}