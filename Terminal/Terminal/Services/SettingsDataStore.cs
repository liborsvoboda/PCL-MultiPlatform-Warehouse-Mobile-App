using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Terminal.Languages;
using Terminal.Models;

namespace Terminal.Services
{
    public class SettingsDataStore : IDataStore<Item>
    {
        List<Item> items;

        public SettingsDataStore()
        {
            items = new List<Item>();
            var mockItems = new List<Item>
            {
                new Item { Id = Guid.NewGuid().ToString(), Text = LangResources.Language, Description= App.Settings.selectedLanguage == "en"  ? LangResources.en : App.Settings.selectedLanguage == "cs" ? LangResources.cs : LangResources.sk   },
                new Item { Id = Guid.NewGuid().ToString(), Text = LangResources.ldapServerIp, Description= App.Settings.ldapServerIp + " " + App.Settings.ldapPort },
                new Item { Id = Guid.NewGuid().ToString(), Text = LangResources.SapSettings, Description= App.SapConnections.ConnectionName }
            };

            foreach (var item in mockItems)
            {
                items.Add(item);
            }
        }

        public async Task<bool> AddItemAsync(Item item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Item item)
        {
            var oldItem = items.Where((Item arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = items.Where((Item arg) => arg.Id == id).FirstOrDefault();
            items.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<Item> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Item>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }
    }
}