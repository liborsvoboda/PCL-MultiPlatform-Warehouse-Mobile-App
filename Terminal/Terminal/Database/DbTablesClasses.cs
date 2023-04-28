using SQLite;
using System;
using System.Collections.Generic;

namespace Terminal.DbModels
{

    [Table("Settings")]
    public class Settings
    {
        [PrimaryKey, MaxLength(20), Column("pkName")]
        public string pkName { get; set; }
        public string selectedLanguage { get; set; }

        [MaxLength(15)]
        public string ldapServerIp { get; set; }
        [MaxLength(5)]
        public Nullable<int> ldapPort { get; set; } 

        [MaxLength(255)]
        public string ldapDN { get; set; } 
        [MaxLength(255)]
        public string roleDN { get; set; } 
        public Nullable<int> refreshInterval { get; set; }

        [MaxLength(15)]
        public string reportPrinterIp { get; set; }
        [MaxLength(5)]
        public Nullable<int> reportPrinterPort { get; set; }

        [MaxLength(15)]
        public string labelPrinterIp { get; set; }
        [MaxLength(5)]
        public Nullable<int> labelPrinterPort { get; set; }
    }

    [Table("MenuConfigurations")]
    public class MenuConfigurations
    {
        [PrimaryKey, MaxLength(50), Column("pkPageName")]
        public string pkPageName { get; set; }
        public bool autoStartCamera { get; set; } = true;
        public bool hiddenCameraView { get; set; } = true;
        public bool startInputAutoFocus { get; set; } = true;
        public bool cameraInputAutoFocus { get; set; } = true;
        public bool cameraAutoSelect { get; set; } = false;
        public bool cameraAutoCommit { get; set; } = false;
        public bool whisperAutoCommit { get; set; } = false;
    }

    [Table("AdvanceMenuConfigurations")]
    public class AdvanceMenuConfigurations
    {
        [PrimaryKey, MaxLength(50), Column("pkAdvancePageName")]
        public string pkAdvancePageName { get; set; }
        public int daysCountOldTimeDefinition { get; set; } = 365;
        public bool olderwhisperItemsClean { get; set; } = false;
        public bool olderwhisperItemsAutoClean { get; set; } = false;
    }

    [Table("UserHistory")]
    public class UserHistory
    {

        [Indexed, Column("username")]
        public string username { get; set; }
        public string ldapName { get; set; }
        public string ldapSurname { get; set; }
        public string password { get; set; }
        public string role { get; set; }
        public string location { get; set; }
        public string wbs { get; set; }
        public string costCenter { get; set; }
        public string WorkCenter { get; set; }
        public string cid { get; set; }
        public string mobile { get; set; }
        public string mail { get; set; }
        public DateTime loginTime { get; set; }
    }

    [Table("UsersList")]
    public class UsersList
    {
        [Indexed(Name = "UQ_users", Order = 1, Unique = true)]
        public string username { get; set; }
        public string ldapName { get; set; }
        public string ldapSurname { get; set; }
        [Indexed(Name = "UQ_users", Order = 2, Unique = true)]
        public string password { get; set; }
        [Indexed(Name = "UQ_users", Order = 3, Unique = true)]
        public string role { get; set; }
        public string location { get; set; }
        public string wbs { get; set; }
        public string costCenter { get; set; }
        public string mobile { get; set; }
        public string mail { get; set; }
        public DateTime loginTime { get; set; }
    }

    [Table("SapRequests")]
    public class SapRequests
    {
        [PrimaryKey, MaxLength(50), Column("callType")]
        public string callType { get; set; }
        public string ppf { get; set; }
        public string qpf { get; set; }
        public string apf { get; set; }
        public string xmlMessage { get; set; }
        public string separator { get; set; }
        public string itemSplitter { get; set; }
        public DateTime lastUpdate { get; set; }
    }

    [Table("SapResponses")]
    public class SapResponses
    {
        [Indexed(Name = "UQ_callType", Order = 1, Unique = true)]
        public string callType { get; set; }
        [Indexed(Name = "UQ_callType", Order = 2, Unique = true)]
        public string name { get; set; }
        public string route { get; set; }
        public bool required { get; set; } 
        public int valueType { get; set; }
        public bool reverseCount { get; set; }
        public bool sum { get; set; }
        public string valueForCheck { get; set; }
        public string prefixForCheck { get; set; }
        public bool removePrefix { get; set; }
        public Nullable<int> level { get; set; } 
        public bool cannotExist { get; set; }
        public DateTime lastUpdate { get; set; }
        public string realValue { get; set; }
        public bool status { get; set; }

    }

    [Table("SapConnections")]
    public class SapConnections
    {
        [PrimaryKey, MaxLength(50), Column("ConnectionName")]
        public string ConnectionName { get; set; } 
        public string type { get; set; }
        public bool setDefault { get; set; }
        public string navColor { get; set; }
        public DateTime lastUpdate { get; set; }

    }

    [Table("SapWareHouses")]
    public class SapWareHouses
    {
        [Indexed(Name = "UQ_warehouse", Order = 1, Unique = true)]
        public string plant { get; set; }
        [Indexed(Name = "UQ_warehouse", Order = 2, Unique = true)]
        public string location { get; set; }
        public string description { get; set; }
        public DateTime lastUpdate { get; set; }
    }

    [Table("SapAreas")]
    public class SapAreas
    {
        [Indexed(Name = "UQ_area", Order = 1, Unique = true)]
        public string plant { get; set; }
        [Indexed(Name = "UQ_area", Order = 2, Unique = true)]
        public string location { get; set; }
        public string description { get; set; }
        public DateTime lastUpdate { get; set; }
    }

    [Table("SupplierCodesDefinitions")]
    public class SupplierCodesDefinitions
    {
        [PrimaryKey, MaxLength(50), Column("supplier")]
        public string supplier { get; set; }
        public string codePrefix { get; set; }
        public string codeType { get; set; }
        public string separator { get; set; }
        public bool mnExist { get; set; }
        public Nullable<int> mnSegment { get; set; }
        public string mnPrefixSeparator { get; set; } = "";
        public string mnSuffixSeparator { get; set; } = "";
        public bool mnRemoveCharacterOnStart { get; set; }
        [MaxLength(1)]
        public string mnRemoveCharacter { get; set; }
        public bool pnExist { get; set; }
        public Nullable<int> pnSegment { get; set; }
        public string pnPrefixSeparator { get; set; } = "";
        public string pnSuffixSeparator { get; set; } = "";
        public bool pnRemoveCharacterOnStart { get; set; }
        [MaxLength(1)]
        public string pnRemoveCharacter { get; set; }
        public bool snExist { get; set; }
        public Nullable<int> snSegment { get; set; }
        public string snPrefixSeparator { get; set; } = "";
        public string snSuffixSeparator { get; set; } = "";
        public bool snRemoveCharacterOnStart { get; set; }
        [MaxLength(1)]
        public string snRemoveCharacter { get; set; }
        public Nullable<int> segmentLength { get; set; }
        public bool enabled { get; set; } = true;
        public DateTime lastUpdate { get; set; }
    }

    [Table("UIIDDefinitions")]
    public class UIIDDefinitions
    {
        [PrimaryKey, MaxLength(50), Column("uuidCodeFormat")]
        public string uuidCodeFormat { get; set; }
        public string detectCode { get; set; }
        public string prefix { get; set; }
        public string snPrefix { get; set; }
        public Nullable<int> snLength { get; set; }
        [MaxLength(1)]
        public string snfillCharacter { get; set; }
        public string mnPrefix { get; set; }
        public Nullable<int> mnLength { get; set; }
        [MaxLength(1)]
        public string mnfillCharacter { get; set; }
        public bool defaultRequest { get; set; }
        public DateTime lastUpdate { get; set; }
        
    }

    [Table("LabelDefinitions")]
    public class LabelDefinitions
    {
        [PrimaryKey, MaxLength(50), Column("labelName")]
        public string labelName { get; set; }
        public string definition { get; set; }
        public string wsdlValue1 { get; set; } = "";
        [MaxLength(1)]
        public string removeStartChar1 { get; set; }
        public string wsdlValue2 { get; set; } = "";
        [MaxLength(1)]
        public string removeStartChar2 { get; set; }
        public string wsdlValue3 { get; set; } = "";
        [MaxLength(1)]
        public string removeStartChar3 { get; set; }
        public string wsdlValue4 { get; set; } = "";
        [MaxLength(1)]
        public string removeStartChar4 { get; set; }
        public string wsdlValue5 { get; set; } = "";
        [MaxLength(1)]
        public string removeStartChar5 { get; set; }
        public bool setDefault { get; set; } = false;
        public DateTime lastUpdate { get; set; }

    }

    [Table("SapFormats")]
    public class SapFormats
    {
        [PrimaryKey, MaxLength(50), Column("fieldName")]
        public string fieldName { get; set; }
        public bool removeCharacterOnStart { get; set; }
        [MaxLength(1)]
        public string removeCharacter { get; set; }
        public Nullable<int> length { get; set; }
        [MaxLength(1)]
        public string fillCharacter { get; set; }
        public DateTime lastUpdate { get; set; }
    }


    [Table("MpPnUiidHistory")]
    public class MpPnUiidHistory
    {
        [PrimaryKey, MaxLength(100), Column("mpPnUiid")]
        public string mpPnUiid { get; set; }
    }

    [Table("SnHistory")]
    public class SnHistory
    {
        [Indexed(Name = "UQ_sn", Order = 1, Unique = true)]
        public string mpPnUiid { get; set; }
        [Indexed(Name = "UQ_sn", Order = 2, Unique = true)]
        public string sn { get; set; }
    }

    [Table("SiteHistory")]
    public class SiteHistory
    {
        [PrimaryKey, MaxLength(100), Column("site")]
        public string site { get; set; }
    }

    public enum ValueTypes
    {
        Bool,
        Int,
        Float,
        String,
        List
    }
}