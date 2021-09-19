namespace Notion.Serialization
{
    internal static class Constants
    {
        public static class BlockType
        {
            public const string Paragraph = "paragraph";
            public const string Heading1 = "heading_1";
            public const string Heading2 = "heading_2";
            public const string Heading3 = "heading_3";
            public const string BulletedListItem = "bulleted_list_item";

            public const string Unsupported = "unsupported";
            //todo
        }

        public static class PropertyValueType
        {
            public const string Title = "title";
            //todo
        }

        public static class ParentType
        {
            public const string Workspace = "workspace";
            public const string Page = "page_id";
            public const string Database = "database_id";
        }

        public static class ParentProperty
        {
            public const string Type = "type";
        }

        public static class RichTextType
        {
            public const string Text = "text";
        }

        public static class BlockProperty
        {
            public const string Id = "id";
            public const string Object = "object";
            public const string CreatedTime = "created_time";
            public const string LastEditedTime = "last_edited_time";
            public const string Archived = "archived";
            public const string HasChildren = "has_children";
            public const string Type = "type";
        }

        public static class PropertyValueProperty
        {
            public const string Id = "id";
            public const string Type = "type";
        }

        public static class PageProperty
        {
            public const string Id = "id";
            public const string Object = "object";
            public const string CreatedTime = "created_time";
            public const string LastEditedTime = "last_edited_time";
            public const string Archived = "archived";
            public const string Icon = "icon";
            public const string Cover = "cover";
            public const string Properties = "properties";
            public const string Parent = "parent";
            public const string Url = "url";
        }

        public static class RichTextProperty
        {
            public const string PlainText = "plain_text";
            public const string Href = "href";
            public const string Annotations = "annotations";
            public const string Type = "type";
        }

        public static class TextProperty
        {
            public const string Content = "content";
            public const string Link = "link";
        }

        public static class ParagraphProperty
        {
            public const string Text = "text";
            public const string Children = "children";
        }
    }
}