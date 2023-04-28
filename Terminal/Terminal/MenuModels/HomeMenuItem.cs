using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal.Models
{
    public enum MenuItemType
    {
        Browse,
        Settings,
        About,
        Logout,
        Login
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
    }
}
