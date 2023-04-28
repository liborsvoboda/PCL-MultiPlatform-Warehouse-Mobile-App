using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SQLite;
using Terminal.DbModels;
using Terminal.Database;
using Xamarin.Forms;
using Terminal.Singleton;
using static Terminal.Database.SapDefinitions;

namespace Terminal
{
    public class DatabaseCommunication
    {
        static readonly Lazy<SQLiteAsyncConnection> lazyInitializer = new Lazy<SQLiteAsyncConnection>(() =>
        {
            return new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        });


        static SQLiteAsyncConnection Database => lazyInitializer.Value;
        static bool initialized = false;


        //Initialize DB
        public DatabaseCommunication()
        {
            InitializeAsync().SafeFireAndForget(false);
        }

        async Task InitializeAsync()
        {

            if (!initialized)
            {
                if (!Database.TableMappings.Any(m => m.MappedType == typeof(Settings)))
                {
                    Database.CreateTablesAsync(CreateFlags.None, typeof(Settings)).ConfigureAwait(true).GetAwaiter().GetResult();
                }
                if (!Database.TableMappings.Any(m => m.MappedType == typeof(UserHistory)))
                {
                    Database.CreateTablesAsync(CreateFlags.None, typeof(UsersList)).ConfigureAwait(true).GetAwaiter().GetResult();
                    Database.CreateTablesAsync(CreateFlags.None, typeof(UserHistory)).ConfigureAwait(true).GetAwaiter().GetResult();
                }
                if (!Database.TableMappings.Any(m => m.MappedType == typeof(SapRequests)))
                {
                    Database.CreateTablesAsync(CreateFlags.None, typeof(SapRequests)).ConfigureAwait(true).GetAwaiter().GetResult();
                    Database.CreateTablesAsync(CreateFlags.None, typeof(SapConnections)).ConfigureAwait(true).GetAwaiter().GetResult();
                    checkSapDefinitions();
                    checkSapConnections(); App.SapConnections = GetSapConnections(true)[0];
                }
                if (!Database.TableMappings.Any(m => m.MappedType == typeof(LabelDefinitions)))
                {
                    Database.CreateTablesAsync(CreateFlags.None, typeof(LabelDefinitions)).ConfigureAwait(true).GetAwaiter().GetResult();
                    checkLabelDefinitions(); App.LabelDefinitions = GetLabelDefinitions(false);
                }
                if (!Database.TableMappings.Any(m => m.MappedType == typeof(UIIDDefinitions)))
                {
                    Database.CreateTablesAsync(CreateFlags.None, typeof(UIIDDefinitions)).ConfigureAwait(true).GetAwaiter().GetResult();
                    checkUIIDDefinitions(); App.UIIDDefinitions = GetUIIDDefinitions();
                }
                if (!Database.TableMappings.Any(m => m.MappedType == typeof(SupplierCodesDefinitions)))
                {
                    Database.CreateTablesAsync(CreateFlags.None, typeof(SupplierCodesDefinitions)).ConfigureAwait(true).GetAwaiter().GetResult();
                    checkSupplierCodesDefinitions(); App.SupplierCodesDefinitions = GetSupplierCodesDefinitions();
                }

                if (!Database.TableMappings.Any(m => m.MappedType == typeof(SapFormats)))
                {
                    Database.CreateTablesAsync(CreateFlags.None, typeof(SapFormats)).ConfigureAwait(true).GetAwaiter().GetResult();
                    checkSapFormats(); App.SapFormats = GetSapFormats();
                }

                if (!Database.TableMappings.Any(m => m.MappedType == typeof(SapResponses)))
                {
                    Database.CreateTablesAsync(CreateFlags.None, typeof(SapResponses)).ConfigureAwait(true).GetAwaiter().GetResult();
                    checkSapResponses(); App.SapResponses = GetSapResponses();
                }

                if (!Database.TableMappings.Any(m => m.MappedType == typeof(SapWareHouses)))
                {
                    Database.CreateTablesAsync(CreateFlags.None, typeof(SapWareHouses)).ConfigureAwait(true).GetAwaiter().GetResult();
                    checkSapWareHouses(); App.SapWareHouses = GetSapWareHouses();
                }

                if (!Database.TableMappings.Any(m => m.MappedType == typeof(SapAreas)))
                {
                    Database.CreateTablesAsync(CreateFlags.None, typeof(SapAreas)).ConfigureAwait(true).GetAwaiter().GetResult();
                    checkSapAreas(); App.SapAreas = GetSapAreas();
                }

                if (!Database.TableMappings.Any(m => m.MappedType == typeof(MenuConfigurations)))
                {
                    Database.CreateTablesAsync(CreateFlags.None, typeof(MenuConfigurations)).ConfigureAwait(true).GetAwaiter().GetResult();
                    CheckMenuConfigurations(); App.MenuConfigurations = GetMenuConfigurations();
                }

                if (!Database.TableMappings.Any(m => m.MappedType == typeof(AdvanceMenuConfigurations)))
                {
                    Database.CreateTablesAsync(CreateFlags.None, typeof(AdvanceMenuConfigurations)).ConfigureAwait(true).GetAwaiter().GetResult();
                    CheckAdvanceMenuConfigurations(); App.AdvanceMenuConfigurations = GetAdvanceMenuConfigurations();
                }

                if (!Database.TableMappings.Any(m => m.MappedType == typeof(MpPnUiidHistory)))
                {
                    Database.CreateTablesAsync(CreateFlags.None, typeof(MpPnUiidHistory)).ConfigureAwait(true).GetAwaiter().GetResult();
                }
                if (!Database.TableMappings.Any(m => m.MappedType == typeof(SnHistory)))
                {
                    Database.CreateTablesAsync(CreateFlags.None, typeof(SnHistory)).ConfigureAwait(true).GetAwaiter().GetResult();
                }
                if (!Database.TableMappings.Any(m => m.MappedType == typeof(SiteHistory)))
                {
                    Database.CreateTablesAsync(CreateFlags.None, typeof(SiteHistory)).ConfigureAwait(true).GetAwaiter().GetResult();
                }

                initialized = true;
            }
        }
        //Initialize DB


        //Settings
        public Task<List<Settings>> GetDefaultSettingsAsync(string searchedParam, bool check)
        {
            if (check)
            {
                if (Database.QueryAsync<Settings>("SELECT * FROM " + DbTables.Settings + " WHERE [pkName] = ?", searchedParam).Result.Count() == 0)
                {
                    Settings defaultSettings = new Settings()
                    {
                        pkName = DbParams.Settings,
                        selectedLanguage = "cs,en,sk".Contains(Thread.CurrentThread.CurrentUICulture.Name) ? Thread.CurrentThread.CurrentUICulture.Name : "en"

                    };
                    int res = SaveSettingAsync(defaultSettings).Result;
                }
            }
            return Database.QueryAsync<Settings>("SELECT * FROM " + DbTables.Settings + " WHERE [pkName] = ?", searchedParam);
        }

        public Task<List<Settings>> GetItemsAsync()
        {
            return Database.Table<Settings>().ToListAsync();
        }

        public Task<Settings> GetSettingAsync(string pkName)
        {
            return Database.Table<Settings>().Where(i => i.pkName == pkName).FirstOrDefaultAsync();
        }

        public Task<int> SaveSettingAsync(Settings item)
        {
            return Database.InsertAsync(item);
        }

        public async static void UpdateSettingsAsync(Settings item)
        {
            await Database.UpdateAsync(item);
        }

        public Task<int> DeleteItemAsync(Settings item)
        {
            return Database.DeleteAsync(item);
        }
        //Settings


        //User
        public static Task<int> SaveUserAsync(UserHistory item)
        {
            if (item.role != null) { //offline user list
                try
                {
                    UsersList uniqueUser = new UsersList() { username = item.username, password = item.password, costCenter = item.costCenter, wbs = item.wbs, role = item.role, location = item.location, mail = item.mail, mobile = item.mobile, loginTime = item.loginTime };
                    Database.InsertAsync(uniqueUser).GetAwaiter().GetResult();

                } catch (Exception) { }
            }
            return Database.InsertAsync(item);
        }
        //User


        //SapDefinitions Import on Start
        public static bool checkSapDefinitions()
        {
            geteratedSapDefinitions().ForEach(delegate (SapRequests item) {
                if (Database.Table<SapRequests>().Where(i => i.callType == item.callType).CountAsync().GetAwaiter().GetResult() == 0) {
                    Database.InsertAsync(item).GetAwaiter().GetResult();
                }
            });
            return true;
        }

        public static SapRequests GetSapDefinition(string callType)
        {
            return Database.Table<SapRequests>().Where(i => i.callType == callType).FirstOrDefaultAsync().GetAwaiter().GetResult();
        }

        public static bool checkSapConnections()
        {

            if (SapConnections().Count > GetSapConnectionsCount())
            {
                Database.InsertAllAsync(SapConnections()).GetAwaiter().GetResult();
            }
            return true;
        }

        public static bool setSapConnectionDefault(string connectionName)
        {
            Database.ExecuteAsync("UPDATE " + DbTables.SapConnections + " SET setDefault = CASE WHEN ConnectionName = ? THEN true ELSE false END ", connectionName);
            return true;
        }

        public static int GetSapConnectionsCount()
        {
            return Database.Table<SapConnections>().CountAsync().GetAwaiter().GetResult();
        }

        public static List<SapConnections> GetSapConnections(bool defaultOnly)
        {
            if (defaultOnly)
            {
                List<SapConnections> res = new List<SapConnections>() {
                    Database.Table<SapConnections>().Where(i => i.setDefault == true).FirstOrDefaultAsync().GetAwaiter().GetResult()
                };
                if (res[0] != null) { GlobalResources.Current.SapConnectionTypeColor = Color.FromHex(res[0].navColor); }
                return res;
            } else
            {
                return Database.Table<SapConnections>().ToListAsync().GetAwaiter().GetResult();
            }
        }

        // Labels
        public static bool checkLabelDefinitions()
        {

            if (LabelDefinitions().Count > GetLabelDefinitionsCount())
            {
                Database.InsertAllAsync(LabelDefinitions()).GetAwaiter().GetResult();
            }
            return true;
        }

        public static int GetLabelDefinitionsCount()
        {
            return Database.Table<LabelDefinitions>().CountAsync().GetAwaiter().GetResult();
        }

        public static List<LabelDefinitions> GetLabelDefinitions(bool defaultOnly)
        {
            if (defaultOnly)
            {
                List<LabelDefinitions> res = new List<LabelDefinitions>() {
                    Database.Table<LabelDefinitions>().Where(i => i.setDefault == true).FirstOrDefaultAsync().GetAwaiter().GetResult()
                };
                return res;
            }
            else
            {
                return Database.Table<LabelDefinitions>().ToListAsync().GetAwaiter().GetResult();
            }
        }

        public static bool checkUIIDDefinitions()
        {

            if (UIIDDefinitions().Count > GetUIIDDefinitionsCount())
            {
                Database.InsertAllAsync(UIIDDefinitions()).GetAwaiter().GetResult();
            }
            return true;
        }

        public static int GetUIIDDefinitionsCount()
        {
            return Database.Table<UIIDDefinitions>().CountAsync().GetAwaiter().GetResult();
        }

        public static List<UIIDDefinitions> GetUIIDDefinitions()
        {
            return Database.Table<UIIDDefinitions>().ToListAsync().GetAwaiter().GetResult();
        }


        public static bool checkSupplierCodesDefinitions()
        {

            if (SupplierCodesDefinitions().Count > GetSupplierCodesDefinitionsCount())
            {
                Database.InsertAllAsync(SupplierCodesDefinitions()).GetAwaiter().GetResult();
            }
            return true;
        }

        public static int GetSupplierCodesDefinitionsCount()
        {
            return Database.Table<SupplierCodesDefinitions>().CountAsync().GetAwaiter().GetResult();
        }

        public static List<SupplierCodesDefinitions> GetSupplierCodesDefinitions()
        {
            return Database.Table<SupplierCodesDefinitions>().Where(item => item.enabled == true).ToListAsync().GetAwaiter().GetResult();
        }

        public static bool checkSapFormats()
        {

            if (SapFormats().Count > GetSapFormatsCount())
            {
                Database.InsertAllAsync(SapFormats()).GetAwaiter().GetResult();
            }
            return true;
        }

        public static int GetSapFormatsCount()
        {
            return Database.Table<SapFormats>().CountAsync().GetAwaiter().GetResult();
        }

        public static List<SapFormats> GetSapFormats()
        {
            return Database.Table<SapFormats>().ToListAsync().GetAwaiter().GetResult();
        }

        public static bool checkSapResponses()
        {

            if (SapResponses().Count > GetSapResponsesCount())
            {
                Database.InsertAllAsync(SapResponses()).GetAwaiter().GetResult();
            }
            return true;
        }

        public static int GetSapResponsesCount()
        {
            return Database.Table<SapResponses>().CountAsync().GetAwaiter().GetResult();
        }

        public static List<SapResponses> GetSapResponses()
        {
            return Database.Table<SapResponses>().ToListAsync().GetAwaiter().GetResult();
        }

        public static bool checkSapWareHouses()
        {

            if (SapWareHouses().Count > GetSapWareHousesCount())
            {
                Database.InsertAllAsync(SapWareHouses()).GetAwaiter().GetResult();
            }
            return true;
        }

        public static int GetSapWareHousesCount()
        {
            return Database.Table<SapWareHouses>().CountAsync().GetAwaiter().GetResult();
        }

        public static List<SapWareHouses> GetSapWareHouses()
        {
            return Database.Table<SapWareHouses>().ToListAsync().GetAwaiter().GetResult();
        }

        public static List<MpPnUiidHistory> GetMpPnUiidHistory(MpPnUiidHistory searchedMpPnUiid)
        {
            if (!String.IsNullOrWhiteSpace(searchedMpPnUiid.mpPnUiid))
            {
                return Database.Table<MpPnUiidHistory>().Where(item => item.mpPnUiid.StartsWith(searchedMpPnUiid.mpPnUiid)).ToListAsync().GetAwaiter().GetResult();
            } else { return new List<MpPnUiidHistory>(); }
        }

        public static bool SaveMpPnUiidHistory(MpPnUiidHistory foundedMpPnUiid)
        {
            try
            {
                Database.InsertAsync(foundedMpPnUiid).GetAwaiter().GetResult(); }
            catch (Exception) { }
            return true;
        }

        public static List<SiteHistory> GetSiteHistory(SiteHistory searchedSite)
        {
            if (!String.IsNullOrWhiteSpace(searchedSite.site))
            {
                return Database.Table<SiteHistory>().Where(item => item.site.StartsWith(searchedSite.site)).ToListAsync().GetAwaiter().GetResult();
            }
            else { return new List<SiteHistory>(); }
        }

        public static bool SaveSiteHistory(SiteHistory foundedSite)
        {
            try
            {
                Database.InsertAsync(foundedSite).GetAwaiter().GetResult();
            }
            catch (Exception) { }
            return true;
        }


        public static List<SnHistory> GetSnHistory(SnHistory searchedSn)
        {
            if (!String.IsNullOrWhiteSpace(searchedSn.sn))
            {
                return Database.Table<SnHistory>().Where(item => item.mpPnUiid == searchedSn.mpPnUiid).Where(item => item.sn.StartsWith(searchedSn.sn)).ToListAsync().GetAwaiter().GetResult();
            }
            else { return new List<SnHistory>(); }
        }

        public static bool SaveSnHistory(SnHistory foundedSn)
        {
            try
            {
                Database.InsertAsync(foundedSn).GetAwaiter().GetResult();
            }
            catch (Exception) { }
            return true;
        }

        public static bool checkSapAreas()
        {

            if (SapAreas().Count > GetSapAreasCount())
            {
                Database.InsertAllAsync(SapAreas()).GetAwaiter().GetResult();
            }
            return true;
        }

        public static int GetSapAreasCount()
        {
            return Database.Table<SapAreas>().CountAsync().GetAwaiter().GetResult();
        }

        public static List<SapAreas> GetSapAreas()
        {
            return Database.Table<SapAreas>().ToListAsync().GetAwaiter().GetResult();
        }

        public static bool CheckMenuConfigurations()
        {
            if (MenuConfigurations().Count > GetMenuConfigurationsCount())
            {
                Database.InsertAllAsync(MenuConfigurations()).GetAwaiter().GetResult();
            }
            return true;
        }

        public static int GetMenuConfigurationsCount()
        {
            return Database.Table<MenuConfigurations>().CountAsync().GetAwaiter().GetResult();
        }

        public static List<MenuConfigurations> GetMenuConfigurations()
        {
            return Database.Table<MenuConfigurations>().ToListAsync().GetAwaiter().GetResult();
        }

        public static bool CheckAdvanceMenuConfigurations()
        {
            if (AdvanceMenuConfigurations().Count > GetAdvanceMenuConfigurationsCount())
            {
                Database.InsertAllAsync(AdvanceMenuConfigurations()).GetAwaiter().GetResult();
            }
            return true;
        }

        public static int GetAdvanceMenuConfigurationsCount()
        {
            return Database.Table<AdvanceMenuConfigurations>().CountAsync().GetAwaiter().GetResult();
        }

        public static List<AdvanceMenuConfigurations> GetAdvanceMenuConfigurations()
        {
            return Database.Table<AdvanceMenuConfigurations>().ToListAsync().GetAwaiter().GetResult();
        }

        //SapDefinitions
    }
}
