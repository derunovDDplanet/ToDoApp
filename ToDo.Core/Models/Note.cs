using System;
namespace ToDo.Core.Models
{
    public class Note
    {

        public string Header { get; set; }
        public string Content { get; set; }

        public Note(string header, string content)
        {
            Header = header;
            Content = content;
        }

        public override string ToString()
        {
            return Header;
        }

    }
}
