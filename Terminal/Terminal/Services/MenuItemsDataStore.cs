using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Terminal.Languages;
using Terminal.Models;
using Terminal.Images;
using System.Drawing;

namespace Terminal.Services
{
    public class MenuItemsDataStore : IDataStore<Item>
    {
        List<Item> items;

        public MenuItemsDataStore()
        {
            items = new List<Item>();
            var mockItems = new List<Item>
            {
                new Item { Id = Guid.NewGuid().ToString(), Text = LangResources.FastGoodsInfo, Sequence =1, Image = Images.Images.NrEmpty,xamlPage="FastGoodsInfoPage", isEnabled=true, BackgroundColor = Color.Transparent },
                new Item { Id = Guid.NewGuid().ToString(), Text = LangResources.GoodsIssue, Sequence =2, Image = Images.Images.NrEmpty, xamlPage="GoodsIssuePage", isEnabled=true, BackgroundColor = Color.Transparent},
                new Item { Id = Guid.NewGuid().ToString(), Text = LangResources.GoodsReceipt, Sequence =3, Image = Images.Images.NrEmpty ,xamlPage="GoodsReceiptPage", isEnabled=true, BackgroundColor = Color.Transparent},
                new Item { Id = Guid.NewGuid().ToString(), Text = LangResources.TransferRelease, Sequence =4, Image = Images.Images.NrEmpty, xamlPage="TransferReleasePage", isEnabled=true, BackgroundColor = Color.Transparent},
                new Item { Id = Guid.NewGuid().ToString(), Text = LangResources.TransferReceipt, Sequence =5, Image = Images.Images.NrEmpty , xamlPage="TransferReceiptPage", isEnabled=true, BackgroundColor = Color.Transparent},
                new Item { Id = Guid.NewGuid().ToString(), Text = LangResources.ReportSite, Sequence =6, Image = Images.Images.NrEmpty, xamlPage="ReportSitePage", isEnabled=true, BackgroundColor = Color.Transparent},
                new Item { Id = Guid.NewGuid().ToString(), Text = LangResources.ReportLocation, Sequence =7, Image = Images.Images.NrEmpty, xamlPage="ReportLocationPage", isEnabled=true , BackgroundColor=Color.Transparent },
                new Item { Id = Guid.NewGuid().ToString(), Text = LangResources.LabelPrinting, Sequence =8, Image = Images.Images.NrEmpty, xamlPage="LabelPrintPage", isEnabled=true, BackgroundColor = Color.Transparent, onlineOnly=true},
                new Item { Id = Guid.NewGuid().ToString(), Text = LangResources.FaultReportPrint , Sequence =9, Image = Images.Images.NrEmpty, xamlPage="FaultReportPrintPage", isEnabled=true, BackgroundColor = Color.Transparent},
                new Item { Id = Guid.NewGuid().ToString(), Text = LangResources.GoodReceiptFromPO, Sequence =10, Image = Images.Images.NrEmpty, xamlPage="GoodReceiptFromPOPage", isEnabled=true, BackgroundColor = Color.Transparent},
                new Item { Id = Guid.NewGuid().ToString(), Text = LangResources.StockTaking, Sequence =11, Image = Images.Images.NrEmpty, xamlPage="StockTakingPage" , BackgroundColor=Color.Gray},
                new Item { Id = Guid.NewGuid().ToString(), Text = LangResources.DataTransfer, Sequence =12, Image = Images.Images.NrEmpty, xamlPage="DataTransferPage" , BackgroundColor=Color.Gray},

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