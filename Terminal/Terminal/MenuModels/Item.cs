using System;
using System.Drawing;

namespace Terminal.Models
{
    public class Item
    {
        public string Id { get; set; }
        public Int16 Sequence { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string xamlPage { get; set; } = null;
        public bool isEnabled { get; set; }
        public bool onlineOnly { get; set; }
        public Color BackgroundColor { get; set; }
    }
}