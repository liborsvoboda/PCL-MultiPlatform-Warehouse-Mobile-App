using System;
using System.Collections.Generic;
using System.Text;
using Terminal.DbModels;
using Terminal.Functions;

namespace Terminal.Database
{
    public class SapDefinitions
    {
        // data lists
        public static List<SapRequests> geteratedSapDefinitions()
        {
            List<SapRequests> defaultList = new List<SapRequests>();
            defaultList.Add(MaterialGetByFilter());
            defaultList.Add(EquipmentGetByFilter());
            defaultList.Add(TaskListGetByFilter());
            defaultList.Add(CSOrderGetByFilter());
            defaultList.Add(FunctionalLocationGetByFilter());
            defaultList.Add(MaterialDocumentCreateOrChange());
            defaultList.Add(MaterialCheckAvailability());
            defaultList.Add(CSOrderCreateOrChange());
            defaultList.Add(ServiceNotificationCreateOrChange());

            return defaultList;
        }

        public static List<MenuConfigurations> MenuConfigurations()
        {
            List<MenuConfigurations> defaultList = new List<MenuConfigurations>() {
                new MenuConfigurations() { pkPageName="FastGoodsInfoPage", autoStartCamera = false, hiddenCameraView=true, cameraAutoCommit = false, whisperAutoCommit = false, startInputAutoFocus = true},
                new MenuConfigurations() { pkPageName="GoodsIssuePage", autoStartCamera = false, hiddenCameraView=true, cameraAutoCommit = false, whisperAutoCommit = false, startInputAutoFocus = true},
                new MenuConfigurations() { pkPageName="GoodsReceiptPage", autoStartCamera = false, hiddenCameraView=true, cameraAutoCommit = false, whisperAutoCommit = false, startInputAutoFocus = true},
                new MenuConfigurations() { pkPageName="TransferReleasePage", autoStartCamera = false, hiddenCameraView=true, cameraAutoCommit = false, whisperAutoCommit = false, startInputAutoFocus = true},
                new MenuConfigurations() { pkPageName="TransferReceiptPage", autoStartCamera = false, hiddenCameraView=true, cameraAutoCommit = false, whisperAutoCommit = false, startInputAutoFocus = true},
                new MenuConfigurations() { pkPageName="ReportSitePage", autoStartCamera = false, hiddenCameraView=true, cameraAutoCommit = false, whisperAutoCommit = false, startInputAutoFocus = true},
                new MenuConfigurations() { pkPageName="ReportLocationPage", autoStartCamera = false, hiddenCameraView=true, cameraAutoCommit = false, whisperAutoCommit = false, startInputAutoFocus = true},
                new MenuConfigurations() { pkPageName="LabelPrintPage", autoStartCamera = false, hiddenCameraView=true, cameraAutoCommit = false, whisperAutoCommit = false, startInputAutoFocus = true},
                new MenuConfigurations() { pkPageName="FaultReportPrintPage", autoStartCamera = false, hiddenCameraView=true, cameraAutoCommit = false, whisperAutoCommit = false, startInputAutoFocus = true},
                new MenuConfigurations() { pkPageName="GoodReceiptFromPOPage", autoStartCamera = false, hiddenCameraView=true, cameraAutoCommit = false, whisperAutoCommit = false, startInputAutoFocus = true},
                new MenuConfigurations() { pkPageName="StockTakingPage", autoStartCamera = false, hiddenCameraView=true, cameraAutoCommit = false, whisperAutoCommit = false, startInputAutoFocus = true},
                new MenuConfigurations() { pkPageName="DataTransferPage", autoStartCamera = false, hiddenCameraView=true, cameraAutoCommit = false, whisperAutoCommit = false, startInputAutoFocus = true},
            };
            return defaultList;
        }

        public static List<AdvanceMenuConfigurations> AdvanceMenuConfigurations()
        {
            List<AdvanceMenuConfigurations> defaultList = new List<AdvanceMenuConfigurations>() {
                new AdvanceMenuConfigurations() { pkAdvancePageName="FastGoodsInfoPage", daysCountOldTimeDefinition = 365, olderwhisperItemsClean = false, olderwhisperItemsAutoClean = false},
                new AdvanceMenuConfigurations() { pkAdvancePageName="GoodsIssuePage", daysCountOldTimeDefinition = 365, olderwhisperItemsClean = false, olderwhisperItemsAutoClean = false},
                new AdvanceMenuConfigurations() { pkAdvancePageName="GoodsReceiptPage", daysCountOldTimeDefinition = 365, olderwhisperItemsClean = false, olderwhisperItemsAutoClean = false},
                new AdvanceMenuConfigurations() { pkAdvancePageName="TransferReleasePage", daysCountOldTimeDefinition = 365, olderwhisperItemsClean = false, olderwhisperItemsAutoClean = false},
                new AdvanceMenuConfigurations() { pkAdvancePageName="TransferReceiptPage", daysCountOldTimeDefinition = 365, olderwhisperItemsClean = false, olderwhisperItemsAutoClean = false},
                new AdvanceMenuConfigurations() { pkAdvancePageName="ReportSitePage", daysCountOldTimeDefinition = 365, olderwhisperItemsClean = false, olderwhisperItemsAutoClean = false},
                new AdvanceMenuConfigurations() { pkAdvancePageName="ReportLocationPage", daysCountOldTimeDefinition = 365, olderwhisperItemsClean = false, olderwhisperItemsAutoClean = false},
                new AdvanceMenuConfigurations() { pkAdvancePageName="LabelPrintPage", daysCountOldTimeDefinition = 365, olderwhisperItemsClean = false, olderwhisperItemsAutoClean = false},
                new AdvanceMenuConfigurations() { pkAdvancePageName="FaultReportPrintPage", daysCountOldTimeDefinition = 365, olderwhisperItemsClean = false, olderwhisperItemsAutoClean = false},
                new AdvanceMenuConfigurations() { pkAdvancePageName="GoodReceiptFromPOPage", daysCountOldTimeDefinition = 365, olderwhisperItemsClean = false, olderwhisperItemsAutoClean = false},
                new AdvanceMenuConfigurations() { pkAdvancePageName="StockTakingPage", daysCountOldTimeDefinition = 365, olderwhisperItemsClean = false, olderwhisperItemsAutoClean = false},
                new AdvanceMenuConfigurations() { pkAdvancePageName="DataTransferPage", daysCountOldTimeDefinition = 365, olderwhisperItemsClean = false, olderwhisperItemsAutoClean = false},
            };
            return defaultList;
        }



        public static List<SapConnections> SapConnections()
        {
            List<SapConnections> defaultList = new List<SapConnections>() {
                new SapConnections() {ConnectionName = "SAP PPF", type = "ppf", setDefault = false, navColor = "#d7dadd", lastUpdate=DateTime.Parse("2021-05-17")},
                new SapConnections() {ConnectionName = "SAP QPF", type = "qpf", setDefault = true, navColor = "#FFAD5E", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapConnections() {ConnectionName = "SAP APF", type = "apf", setDefault = false, navColor = "#FF0000", lastUpdate=DateTime.Parse("2021-05-17")}
            };
            return defaultList;
        }

        public static List<LabelDefinitions> LabelDefinitions()
        {
            List<LabelDefinitions> defaultList = new List<LabelDefinitions>() {
                new LabelDefinitions() { labelName = "PMS", definition="$K1P*1*#M*2*#S*3*", wsdlValue1="pnInserted",wsdlValue2="mnInserted", removeStartChar2="0",wsdlValue3="snInserted", setDefault = true, lastUpdate=DateTime.Parse("2021-05-17")},
                new LabelDefinitions() { labelName = "PM", definition="$K1P*1*#M*2*", wsdlValue1="pnInserted",wsdlValue2="mnInserted", removeStartChar2="0", setDefault = false, lastUpdate=DateTime.Parse("2021-05-17")}

            };
            return defaultList;
        }

        public static List<UIIDDefinitions> UIIDDefinitions()
        {
            List<UIIDDefinitions> defaultList = new List<UIIDDefinitions>() {
                new UIIDDefinitions() { uuidCodeFormat="CZTMXXXXXXXX", detectCode="CZTM", lastUpdate=DateTime.Parse("2021-05-17")},
                new UIIDDefinitions() { uuidCodeFormat="$K1*PN*#M*MN*#S*SN*", detectCode="$K1P", prefix="CZUI", snPrefix="-", mnPrefix="-", mnLength=18, mnfillCharacter="0", defaultRequest=true, lastUpdate=DateTime.Parse("2021-05-17")}, //K1P AND CZUI
            };
            return defaultList;
        }

        public static List<SupplierCodesDefinitions> SupplierCodesDefinitions()
        {
            List<SupplierCodesDefinitions> defaultList = new List<SupplierCodesDefinitions>() {
                //new SupplierCodesDefinitions() { supplier="nokia", codeType="DATA_MATRIX", separator="", pnExist=true, pnSegment=1, pnPrefixSeparator="1P", snExist=true, snSegment=2, snPrefixSeparator="S", snRemoveCharacterOnStart= true, snRemoveCharacter= "0", segmentLength=9, enabled=true, lastUpdate=DateTime.Parse("2021-05-17") }, 
                //new SupplierCodesDefinitions() { supplier="nokia1", codeType="PDF_417", separator="", pnExist=true, pnSegment=4, pnPrefixSeparator="1P", pnSuffixSeparator="", snExist=true, snSegment=3, snPrefixSeparator="S", snRemoveCharacterOnStart= true, snRemoveCharacter= "0", segmentLength=5, enabled=true, lastUpdate=DateTime.Parse("2021-05-17")}, //ok
                //new SupplierCodesDefinitions() { supplier="nec", codeType="PDF_417",separator=",",pnExist=true,pnSegment=0,snExist=true,snSegment=2, snRemoveCharacterOnStart= true, snRemoveCharacter= "0", segmentLength=108, enabled=true, lastUpdate=DateTime.Parse("2021-05-17")}, //ok
                //new SupplierCodesDefinitions() { supplier="delta", codeType="DATA_MATRIX", separator="#", pnExist=true, pnSegment=1, snExist=true, snSegment=3, snRemoveCharacterOnStart= true, snRemoveCharacter= "0", segmentLength=9,enabled=true, lastUpdate=DateTime.Parse("2021-05-17")}, //ok
                //new SupplierCodesDefinitions() { supplier="huawei", codeType="QR_CODE",separator="", pnExist=true, pnSegment=1, pnPrefixSeparator="1P", snExist=true, snSegment=3, snPrefixSeparator="S", snSuffixSeparator="", snRemoveCharacterOnStart= true, snRemoveCharacter= "0", segmentLength=4, enabled=true, lastUpdate=DateTime.Parse("2021-05-17")}, //ok
                //new SupplierCodesDefinitions() { supplier="nec1", codeType="DATA_MATRIX", separator="#", pnExist=true, pnSegment=5, snExist=false,enabled=true},

                new SupplierCodesDefinitions() { supplier="Ericsson1", codeType="PDF_417", separator="#", pnExist=true, pnSegment=0, pnPrefixSeparator="$K1P", mnExist=true, snExist=false, mnSegment=1 , mnPrefixSeparator="M", mnRemoveCharacterOnStart= true, mnRemoveCharacter= "0", segmentLength =2, enabled=true, lastUpdate=DateTime.Parse("2021-10-09")}, //ok                               
                new SupplierCodesDefinitions() { supplier="Plant", codeType="DATA_MATRIX", separator="#", pnExist=true, pnSegment=5, pnPrefixSeparator="", mnExist=false, snExist=false, snSegment=2 , snPrefixSeparator="S", snRemoveCharacterOnStart= true, snRemoveCharacter= "0", segmentLength =13, enabled=true, lastUpdate=DateTime.Parse("2021-10-09")}, //ok                               
                new SupplierCodesDefinitions() { supplier="Delta", codeType="DATA_MATRIX", separator="#", pnExist=true, pnSegment=1, pnPrefixSeparator="", mnExist=false, snExist=true, snSegment=3 , snPrefixSeparator="", snRemoveCharacterOnStart= true, snRemoveCharacter= "0", segmentLength =9, enabled=true, lastUpdate=DateTime.Parse("2021-10-09")}, //ok                               
                new SupplierCodesDefinitions() { supplier="NEC", codeType="PDF_417", separator=",", pnExist=true, pnSegment=0, mnExist=false, snExist=true, snSegment=2 , snPrefixSeparator="", snRemoveCharacterOnStart= true, snRemoveCharacter= "0", segmentLength =108, enabled=true, lastUpdate=DateTime.Parse("2021-10-09")}, //ok                                               
                new SupplierCodesDefinitions() { supplier="Ericsson", codeType="PDF_417", separator="", pnExist=true, pnSegment=2, pnPrefixSeparator="1P", mnExist=false, snExist=true, snSegment=7 , snPrefixSeparator="S", snRemoveCharacterOnStart= true, snRemoveCharacter= "0", segmentLength =9, enabled=true, lastUpdate=DateTime.Parse("2021-10-09")}, //ok                               
                new SupplierCodesDefinitions() { supplier="NOKIA", codeType="DATA_MATRIX", separator="", pnExist=true, pnSegment=1, pnPrefixSeparator="1P", mnExist=false, snExist=true, snSegment=2 , snPrefixSeparator="S", snRemoveCharacterOnStart= true, snRemoveCharacter= "0", segmentLength =9, enabled=true, lastUpdate=DateTime.Parse("2021-10-09")}, //ok                               
                new SupplierCodesDefinitions() { supplier="SamotnySerial2", codeType="CODE_128", separator="", pnExist=false, mnExist=false, snExist=true, snSegment=0 , snPrefixSeparator="S", snRemoveCharacterOnStart= true, snRemoveCharacter= "0", segmentLength = 1, enabled=true, lastUpdate=DateTime.Parse("2021-10-09")}, //ok                               
                new SupplierCodesDefinitions() { supplier="Nokia2", codeType="PDF_417", separator="", pnExist=true, pnSegment=4, pnPrefixSeparator="1P", mnExist=false,mnSegment=2, mnPrefixSeparator="SM", mnRemoveCharacterOnStart= true, mnRemoveCharacter= "0", snExist=true, snSegment=3 , snPrefixSeparator="S", snRemoveCharacterOnStart= true, snRemoveCharacter= "0", segmentLength = 5, enabled=true, lastUpdate=DateTime.Parse("2021-10-09")}, //ok                               
                new SupplierCodesDefinitions() { supplier="Kathrein", codeType="DATA_MATRIX", separator="", pnExist=true, pnSegment=1, pnPrefixSeparator="1P", mnExist=false,mnSegment=2, mnPrefixSeparator="SM", mnRemoveCharacterOnStart= true, mnRemoveCharacter= "0", snExist=true, snSegment=2 , snPrefixSeparator="S", snRemoveCharacterOnStart= true, snRemoveCharacter= "0", segmentLength = 6, enabled=true, lastUpdate=DateTime.Parse("2021-10-09")}, //ok                               
                new SupplierCodesDefinitions() { supplier="PartMaterialSerial6", codeType="PDF_417", separator="#", pnExist=true, pnSegment=0, pnPrefixSeparator="$K1P", mnExist=true, mnSegment=1, mnPrefixSeparator="M", mnRemoveCharacterOnStart= true, mnRemoveCharacter= "0", snExist=true, snSegment=2 , snPrefixSeparator="S", snRemoveCharacterOnStart= true, snRemoveCharacter= "0", segmentLength = 3, enabled=true, lastUpdate=DateTime.Parse("2021-10-09")}, //ok                               
                new SupplierCodesDefinitions() { supplier="PartMaterialSerial4", codeType="PDF_417", separator="#", pnExist=true, pnSegment=0, pnPrefixSeparator="$K1PAMN", mnExist=true, mnSegment=1, mnPrefixSeparator="M", mnRemoveCharacterOnStart= true, mnRemoveCharacter= "0", snExist=true, snSegment=2 , snPrefixSeparator="S", snRemoveCharacterOnStart= true, snRemoveCharacter= "0", segmentLength = 3, enabled=true, lastUpdate=DateTime.Parse("2021-10-09")}, //ok                               
                new SupplierCodesDefinitions() { supplier="PartMaterialSerial2", codeType="PDF_417", separator="#", pnExist=true, pnSegment=0, pnPrefixSeparator="$K1P", mnExist=true, mnSegment=1, mnPrefixSeparator="M", mnRemoveCharacterOnStart= true, mnRemoveCharacter= "0", snExist=false, snSegment=2 , snPrefixSeparator="S", snRemoveCharacterOnStart= true, snRemoveCharacter= "0", segmentLength = 3, enabled=true, lastUpdate=DateTime.Parse("2021-10-09")}, //ok               
                new SupplierCodesDefinitions() { supplier="Samotný serial1", codeType="CODE_39", separator="", pnExist=false, mnExist=false, mnRemoveCharacterOnStart= false, snExist=true, snSegment=0 , snPrefixSeparator="", snRemoveCharacterOnStart= true, snRemoveCharacter= "0", segmentLength = 0, enabled=true, lastUpdate=DateTime.Parse("2021-10-09")}, //ok                
                new SupplierCodesDefinitions() { supplier="Samotný serial", codeType="PDF_417", separator="", pnExist=false, mnExist=false, mnRemoveCharacterOnStart= false, snExist=true, snSegment=0 , snPrefixSeparator="N", snRemoveCharacterOnStart= true, snRemoveCharacter= "0", segmentLength = 0, enabled=true, lastUpdate=DateTime.Parse("2021-10-09")}, //ok
                new SupplierCodesDefinitions() { supplier="6PartSerial – Huawei", codeType="128", separator="", pnExist=false, mnExist=false, mnRemoveCharacterOnStart= false, snExist=true, snPrefixSeparator="S", snRemoveCharacterOnStart= true, snRemoveCharacter= "0", segmentLength = 0, enabled=true, lastUpdate=DateTime.Parse("2021-10-09")}, //ok               
                new SupplierCodesDefinitions() { supplier="10PartSerial - Huawei", codeType="128", separator="", pnExist=false, mnExist=false, mnRemoveCharacterOnStart= false, snExist=true, snPrefixSeparator="S", snRemoveCharacterOnStart= true, snRemoveCharacter= "0", segmentLength = 0, enabled=true, lastUpdate=DateTime.Parse("2021-10-09")}, //ok              
                new SupplierCodesDefinitions() { supplier="Identifikace Part numberem", codeType="PDF_417", separator="#", pnExist=true, pnSegment=0, pnPrefixSeparator="$K1P", mnExist=true, mnSegment=1, mnPrefixSeparator="M", mnRemoveCharacterOnStart= true, mnRemoveCharacter= "0", snExist=false, enabled=true, lastUpdate=DateTime.Parse("2021-10-09")}, //ok               
                new SupplierCodesDefinitions() { supplier="PartMaterialSerial", codeType="PDF_417", separator="#", pnExist=true, pnSegment=0, pnPrefixSeparator="$K1P", mnExist=true, mnSegment=1, mnPrefixSeparator="M", mnRemoveCharacterOnStart= true, mnRemoveCharacter= "0", snExist=true, snPrefixSeparator="S", snRemoveCharacterOnStart= true, snRemoveCharacter= "0",  segmentLength = 3, enabled=true, lastUpdate=DateTime.Parse("2021-10-09")}, //ok
                new SupplierCodesDefinitions() { supplier="PartMaterial", codeType="PDF_417", separator="#", pnExist=false, mnExist=true, mnSegment=1, mnPrefixSeparator="M", mnRemoveCharacterOnStart= true, mnRemoveCharacter= "0", snExist=false, segmentLength = 2, enabled=true, lastUpdate=DateTime.Parse("2021-10-09")}, //ok
                new SupplierCodesDefinitions() { supplier="internal", codeType="PDF_417", separator="#", pnExist=false, mnExist=true, mnSegment=1, mnPrefixSeparator="M", mnRemoveCharacterOnStart= true, mnRemoveCharacter= "0", snExist=true, snSegment=2, snPrefixSeparator="S", snRemoveCharacterOnStart= true, snRemoveCharacter= "0", segmentLength = 3, enabled=true, lastUpdate=DateTime.Parse("2021-05-17")}, //ok
                new SupplierCodesDefinitions() { supplier="internalManual", codePrefix="CZUI",  separator="-", pnExist=false, mnExist=true, mnSegment=2, mnPrefixSeparator="", snExist=true, snSegment=1, snPrefixSeparator="", snRemoveCharacterOnStart= true, snRemoveCharacter= "0", segmentLength = 3, enabled=true, lastUpdate=DateTime.Parse("2021-05-17")} //ok CZUI Manual
            };
            return defaultList;
        }

        public static List<SapFormats> SapFormats()
        {
            List<SapFormats> defaultList = new List<SapFormats>() {
                new SapFormats() { fieldName="mn", removeCharacterOnStart=true, removeCharacter="0", length=18, fillCharacter="0", lastUpdate=DateTime.Parse("2021-05-17") }
            };
            return defaultList;
        }

        public static List<SapResponses> SapResponses()
        {
            List<SapResponses> defaultList = new List<SapResponses>()
            {
                new SapResponses(){ callType="materialGetByFilter", name = "Action", route="Action", required=true, level = 0, valueType= (int)ValueTypes.String, valueForCheck="M", lastUpdate=DateTime.Parse("2021-05-17")},
                new SapResponses(){ callType="materialGetByFilter", name = "Id", route="Id",required=true, level = 0, valueType= (int)ValueTypes.String, lastUpdate=DateTime.Parse("2021-05-17")},
                new SapResponses(){ callType="materialGetByFilter", name = "UnitId", route="Header/UnitId", required=true, level = 0, valueType= (int)ValueTypes.String, lastUpdate=DateTime.Parse("2021-05-17")},
                new SapResponses(){ callType="materialGetByFilter", name = "IsDeleted", route="Header/IsDeleted", cannotExist=true, level = 0, valueType= (int)ValueTypes.Bool, lastUpdate=DateTime.Parse("2021-05-17")},
                new SapResponses(){ callType="materialGetByFilter", name = "Text", route="Header/Descriptions[Language='EN']/Text", required=true, valueType= (int)ValueTypes.List, lastUpdate=DateTime.Parse("2021-05-17")},
                new SapResponses(){ callType="materialGetByFilter", name = "ManufacturerPartNumber", route="Header/ManufacturerPartNumber", required=true, level = 0, valueType= (int)ValueTypes.String, lastUpdate=DateTime.Parse("2021-05-17")},
                new SapResponses(){ callType="materialGetByFilter", name = "SerialNumberProfileId", route="PlantDependentData[Action='M'][PlantId='CZ02']/SerialNumberProfileId", required=true, valueType= (int)ValueTypes.String, lastUpdate=DateTime.Parse("2021-05-17")},
                new SapResponses(){ callType="materialGetByFilter", name = "AssetClassId", route="AssetClasses[Action='M']/AssetClassId", valueType= (int)ValueTypes.List, lastUpdate=DateTime.Parse("2021-05-17")}, //if not serialize not exist
                new SapResponses(){ callType="materialGetByFilter", name = "ValuationCategoryId", route="PlantDependentData[Action='M'][PlantId='CZ02']/ValuationCategoryId", required=true, valueType= (int)ValueTypes.String, lastUpdate=DateTime.Parse("2021-05-17")}, //I-neserialize,S-serialize
                new SapResponses(){ callType="materialGetByFilter", name = "PlantSpecificStatus", route="PlantDependentData[Action='M'][PlantId='CZ02'][PlantSpecificStatus='Z2']", cannotExist=true, valueType= (int)ValueTypes.String, lastUpdate=DateTime.Parse("2021-05-17")},
                new SapResponses(){ callType="materialGetByFilter", name = "PlantSpecificStatus1", route="PlantDependentData[Action='M'][PlantId='CZ02'][PlantSpecificStatus='Z3']", cannotExist=true, valueType= (int)ValueTypes.String, lastUpdate=DateTime.Parse("2021-05-17")},
                new SapResponses(){ callType="materialGetByFilter", name = "PlantSpecificStatus2", route="PlantDependentData[Action='M'][PlantId='CZ02'][PlantSpecificStatus='Z4']", cannotExist=true, valueType= (int)ValueTypes.String, lastUpdate=DateTime.Parse("2021-05-17")},

                new SapResponses(){ callType="equipmentGetByFilter", name = "Action", route="Action", required=true, level = 0, valueType= (int)ValueTypes.String, valueForCheck="M", lastUpdate=DateTime.Parse("2021-05-17")},
                new SapResponses(){ callType="equipmentGetByFilter", name = "Id", route="Id", required=true, level = 0, valueType= (int)ValueTypes.String, lastUpdate=DateTime.Parse("2021-05-17")},
                new SapResponses(){ callType="equipmentGetByFilter", name = "MaterialId", route="Header/MaterialId", required=true, level = 0, valueType= (int)ValueTypes.String, lastUpdate=DateTime.Parse("2021-05-17")},
                new SapResponses(){ callType="equipmentGetByFilter", name = "Text", route="Header/Descriptions[Language='EN']/Text", required=true, valueType= (int)ValueTypes.String, lastUpdate=DateTime.Parse("2021-05-17")},
                new SapResponses(){ callType="equipmentGetByFilter", name = "ManufacturerSerialNumber", route="Header/ManufacturerSerialNumber", required=true, level = 0, valueType= (int)ValueTypes.String, lastUpdate=DateTime.Parse("2021-05-17")},
                new SapResponses(){ callType="equipmentGetByFilter", name = "UniqueItemId", route="Header/UniqueItemId", required=true, level = 0, valueType= (int)ValueTypes.String, lastUpdate=DateTime.Parse("2021-05-17")},
                new SapResponses(){ callType="equipmentGetByFilter", name = "Value", route="Header/Statuses/Value", required=true, prefixForCheck="E", removePrefix=true, valueType= (int)ValueTypes.List, lastUpdate=DateTime.Parse("2021-05-17")},
                new SapResponses(){ callType="equipmentGetByFilter", name = "DAction", route="DateDependentHeader/Action", required=true, level = 0, valueType= (int)ValueTypes.String, valueForCheck="M", lastUpdate=DateTime.Parse("2021-05-17")},
                new SapResponses(){ callType="equipmentGetByFilter", name = "AssetClass", route="DateDependentHeader[Action='M']/AssetClass", level = 0, valueType= (int)ValueTypes.String, lastUpdate=DateTime.Parse("2021-05-17")}, //if not serialize not exist
                new SapResponses(){ callType="equipmentGetByFilter", name = "FunctionalLocationId", route="DateDependentHeader/FunctionalLocationId", required=true, level = 0, valueType= (int)ValueTypes.String, lastUpdate=DateTime.Parse("2021-05-17")},
                new SapResponses(){ callType="equipmentGetByFilter", name = "ManufacturerPartNumber", route="DateDependentHeader/ManufacturerPartNumber", level = 0, valueType= (int)ValueTypes.String, lastUpdate=DateTime.Parse("2021-05-17")},
                new SapResponses(){ callType="equipmentGetByFilter", name = "BatchNumber", route="Header/BatchNumber", required=true, level = 0, valueType= (int)ValueTypes.String, lastUpdate=DateTime.Parse("2021-05-17")},

                new SapResponses(){ callType="materialDocumentCreateOrChange", name = "OldId", route="Results/OldId", required=true, level = 0, valueType= (int)ValueTypes.String, lastUpdate=DateTime.Parse("2021-05-17")},
                new SapResponses(){ callType="materialDocumentCreateOrChange", name = "Type", route="Results/Messages/Type", level = 0, valueType= (int)ValueTypes.String, lastUpdate=DateTime.Parse("2021-05-17")},
                new SapResponses(){ callType="materialDocumentCreateOrChange", name = "Text", route="Results/Messages/Text", level = 0, valueType= (int)ValueTypes.String, lastUpdate=DateTime.Parse("2021-05-17")},
                
               // new SapResponses(){ callType="equipmentGetByFilter", name = "ActionCSOrder", route="Equipments/Header/Statuses[Value='E0002']", required=true, level = 0, valueType= (int)ValueTypes.String, lastUpdate=DateTime.Parse("2021-05-17")},
                
                new SapResponses(){ callType="materialCheckAvailability", name = "QuantityFree", route="Availability/Items[MrpElementTypeId='LB']/Quantity", valueType= (int)ValueTypes.List, sum = true, lastUpdate=DateTime.Parse("2021-05-17")},
                new SapResponses(){ callType="materialCheckAvailability", name = "QuantityBlocked", route="Availability/Items[MrpElementTypeId='AR']/Quantity", valueType= (int)ValueTypes.List, sum = true, reverseCount=true, lastUpdate=DateTime.Parse("2021-05-17")},
                new SapResponses(){ callType="materialCheckAvailability", name = "QuantityInTransfer", route="Availability/Items[MrpElementTypeId='BE']/Quantity", valueType= (int)ValueTypes.List, sum = true, lastUpdate=DateTime.Parse("2021-05-17")},
                new SapResponses(){ callType="materialCheckAvailability", name = "UnitId", route="Availability/Items/UnitId", level = 0, valueType= (int)ValueTypes.String, lastUpdate=DateTime.Parse("2021-05-17")},
                new SapResponses(){ callType="materialCheckAvailability", name = "Type", route="Availability/Messages/Type", level = 0, valueType= (int)ValueTypes.String, lastUpdate=DateTime.Parse("2021-05-17")},
                new SapResponses(){ callType="materialCheckAvailability", name = "Text", route="Availability/Messages/Text", level = 0, valueType= (int)ValueTypes.String, lastUpdate=DateTime.Parse("2021-05-17")},

                new SapResponses(){ callType="functionalLocationGetByFilter", name = "Id", route="FunctionalLocations/Id", valueType= (int)ValueTypes.List, lastUpdate=DateTime.Parse("2021-05-17")},

                new SapResponses(){ callType="taskListGetByFilter", name = "TypeId", route="TaskLists/Id/TypeId", level = 0, valueType= (int)ValueTypes.String, lastUpdate=DateTime.Parse("2021-05-17")},
                new SapResponses(){ callType="taskListGetByFilter", name = "Group", route="TaskLists/Id/Group", level = 0, valueType= (int)ValueTypes.String, lastUpdate=DateTime.Parse("2021-05-17")},
                new SapResponses(){ callType="taskListGetByFilter", name = "GroupCounter", route="TaskLists/Id/GroupCounter", level = 0, valueType= (int)ValueTypes.String, lastUpdate=DateTime.Parse("2021-05-17")},
                new SapResponses(){ callType="taskListGetByFilter", name = "Number", route="TaskLists/Operations/Number", level = 0, valueType= (int)ValueTypes.String, lastUpdate=DateTime.Parse("2021-05-17")},

                new SapResponses(){ callType="csOrderGetByFilter", name = "Action", route="Action", required=true, level = 0, valueType= (int)ValueTypes.String, valueForCheck="M", lastUpdate=DateTime.Parse("2021-05-17")},
                new SapResponses(){ callType="csOrderGetByFilter", name = "Id", route="Id", required=true, level = 0, valueType= (int)ValueTypes.String, lastUpdate=DateTime.Parse("2021-05-17")},
                new SapResponses(){ callType="csOrderGetByFilter", name = "OrderTypeId", route="Header/OrderTypeId", required=true, level = 0, valueType= (int)ValueTypes.String, lastUpdate=DateTime.Parse("2021-05-17")},
                new SapResponses(){ callType="csOrderGetByFilter", name = "MainWorkCenter", route="Header/MainWorkCenter", required=true, level = 0, valueType= (int)ValueTypes.String, lastUpdate=DateTime.Parse("2021-05-17")},
                new SapResponses(){ callType="csOrderGetByFilter", name = "ResponsibleCostCenter", route="Header/ResponsibleCostCenter", required=true, level = 0, valueType= (int)ValueTypes.String, lastUpdate=DateTime.Parse("2021-05-17")},
                new SapResponses(){ callType="csOrderGetByFilter", name = "FunctionalLocationId", route="Header/FunctionalLocationId", required=true, level = 0, valueType= (int)ValueTypes.String, lastUpdate=DateTime.Parse("2021-05-17")},
                new SapResponses(){ callType="csOrderGetByFilter", name = "WbsElementId", route="Header/WbsElementId", required=true, level = 0, valueType= (int)ValueTypes.String, lastUpdate=DateTime.Parse("2021-05-17")},
                new SapResponses(){ callType="csOrderGetByFilter", name = "Value", route="Header/Statuses[Action='M']/Value", required=true, valueType= (int)ValueTypes.List, valueForCheck="I0002", lastUpdate=DateTime.Parse("2021-05-17")},
                new SapResponses(){ callType="csOrderGetByFilter", name = "NotValue", route="Header/Statuses[Action='M']/Value", cannotExist=true, valueType= (int)ValueTypes.List, valueForCheck="I0045", lastUpdate=DateTime.Parse("2021-05-17")},

                new SapResponses(){ callType="csOrderGetByFilter", name = "OperationRoutingNumberInOrder", route="Operations[Action='M']/OperationRoutingNumberInOrder", level = 0, valueType= (int)ValueTypes.String, lastUpdate=DateTime.Parse("2021-05-17")},
                new SapResponses(){ callType="csOrderGetByFilter", name = "GeneralCounterForOrder", route="Operations[Action='M']/GeneralCounterForOrder", level = 0, valueType= (int)ValueTypes.String, lastUpdate=DateTime.Parse("2021-05-17")},
                new SapResponses(){ callType="csOrderGetByFilter", name = "TypeId", route="Operations[Action='M']/TaskListId/TypeId", level = 0, valueType= (int)ValueTypes.String, lastUpdate=DateTime.Parse("2021-05-17")},
                new SapResponses(){ callType="csOrderGetByFilter", name = "Group", route="Operations[Action='M']/TaskListId/Group", level = 0, valueType= (int)ValueTypes.String, lastUpdate=DateTime.Parse("2021-05-17")},
                new SapResponses(){ callType="csOrderGetByFilter", name = "GroupCounter", route="Operations[Action='M']/TaskListId/GroupCounter", level = 0, valueType= (int)ValueTypes.String, lastUpdate=DateTime.Parse("2021-05-17")},
                new SapResponses(){ callType="csOrderGetByFilter", name = "Number", route="Operations[Action='M']/Components[Action='M']/Number", level = 0, valueType= (int)ValueTypes.String, lastUpdate=DateTime.Parse("2021-05-17")},
                new SapResponses(){ callType="csOrderGetByFilter", name = "ReservationHeaderId", route="Operations[Action='M']/Components[Action='M']/ReservationHeaderId", level = 0, valueType= (int)ValueTypes.String, lastUpdate=DateTime.Parse("2021-05-17")},
                new SapResponses(){ callType="csOrderGetByFilter", name = "ReservationItemNumber", route="Operations[Action='M']/Components[Action='M']/ReservationItemNumber", level = 0, valueType= (int)ValueTypes.String, lastUpdate=DateTime.Parse("2021-05-17")},
                new SapResponses(){ callType="csOrderGetByFilter", name = "MaterialId", route="Operations[Action='M']/Components[Action='M']/MaterialId", level = 0, valueType= (int)ValueTypes.String, lastUpdate=DateTime.Parse("2021-05-17")},

                new SapResponses(){ callType="csOrderCreateOrChange", name = "OldId", route="Results/OldId", required=true, level = 0, valueType= (int)ValueTypes.String, lastUpdate=DateTime.Parse("2021-05-17")},
                new SapResponses(){ callType="csOrderCreateOrChange", name = "NewId", route="Results/NewId", level = 0, valueType= (int)ValueTypes.String, lastUpdate=DateTime.Parse("2021-05-17")},
                new SapResponses(){ callType="csOrderCreateOrChange", name = "Type", route="Results/Messages/Type", level = 0, valueType= (int)ValueTypes.String, lastUpdate=DateTime.Parse("2021-05-17")},
                new SapResponses(){ callType="csOrderCreateOrChange", name = "Text", route="Results/Messages/Text", level = 0, valueType= (int)ValueTypes.String, lastUpdate=DateTime.Parse("2021-05-17")},

                new SapResponses(){ callType="serviceNotificationCreateOrChange", name = "OldId", route="Results/OldId", required=true, level = 0, valueType= (int)ValueTypes.String, lastUpdate=DateTime.Parse("2021-05-17")},
                new SapResponses(){ callType="serviceNotificationCreateOrChange", name = "NewId", route="Results/NewId", level = 0, valueType= (int)ValueTypes.String, lastUpdate=DateTime.Parse("2021-05-17")},
                new SapResponses(){ callType="serviceNotificationCreateOrChange", name = "Type", route="Results/Messages/Type", level = 0, valueType= (int)ValueTypes.String, lastUpdate=DateTime.Parse("2021-05-17")},
                new SapResponses(){ callType="serviceNotificationCreateOrChange", name = "Text", route="Results/Messages/Text", level = 0, valueType= (int)ValueTypes.String, lastUpdate=DateTime.Parse("2021-05-17")},

            };
            return defaultList;
        }

        public static List<SapWareHouses> SapWareHouses()
        {
            List<SapWareHouses> defaultList = new List<SapWareHouses>() {
                new SapWareHouses() { plant="CZ02", location="1002", description="GTS RS Č.Budějov", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapWareHouses() { plant="CZ02", location="1003", description="GTS RS Plzeň", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapWareHouses() { plant="CZ02", location="1004", description="GTS RS Ustí n.La", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapWareHouses() { plant="CZ02", location="1005", description="GTS RS Liberec", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapWareHouses() { plant="CZ02", location="1006", description="GTS RS Hr.Králov", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapWareHouses() { plant="CZ02", location="1007", description="GTS RS Brno", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapWareHouses() { plant="CZ02", location="1008", description="GTS RS Ostrava", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapWareHouses() { plant="CZ02", location="1009", description="GTS Inv. Nagano", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapWareHouses() { plant="CZ02", location="1010", description="GTS THP-GT", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapWareHouses() { plant="CZ02", location="1011", description="GTS G-Tech", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapWareHouses() { plant="CZ02", location="1012", description="GTS G-Tech vadné", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapWareHouses() { plant="CZ02", location="1013", description="GTS Zápůjčky", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapWareHouses() { plant="CZ02", location="1014", description="GTS HUAWEI serv.", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapWareHouses() { plant="CZ02", location="1015", description="GTS Cizí zaříz.", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapWareHouses() { plant="CZ02", location="1016", description="GTS In. k a v op", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapWareHouses() { plant="CZ02", location="1017", description="GTS Likvidace", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapWareHouses() { plant="CZ02", location="1018", description="GTS PRODEJ", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapWareHouses() { plant="CZ02", location="1019", description="GTS RS Pha-Lux", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapWareHouses() { plant="CZ02", location="2001", description="Telco", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapWareHouses() { plant="CZ02", location="2002", description="LAM", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapWareHouses() { plant="CZ02", location="2003", description="Telco-Výstavba", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapWareHouses() { plant="CZ02", location="2004", description="Telco I.", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapWareHouses() { plant="CZ02", location="2005", description="Zápůjčky-Telco (if needed)", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapWareHouses() { plant="CZ02", location="3001", description="RN Krtek", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapWareHouses() { plant="CZ02", location="3002", description="Sklad_Mobile", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapWareHouses() { plant="CZ02", location="3003", description="RN Baterie", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapWareHouses() { plant="CZ02", location="3004", description="RN INS", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapWareHouses() { plant="CZ02", location="3005", description="RN Hot stock", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapWareHouses() { plant="CZ02", location="3006", description="RN návrh likvid.", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapWareHouses() { plant="CZ02", location="3007", description="RN Expedis", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapWareHouses() { plant="CZ02", location="3008", description="RN zápůjčka", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapWareHouses() { plant="CZ02", location="3009", description="RN BTS rádia", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapWareHouses() { plant="CZ02", location="3010", description="RN Exped likvid.", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapWareHouses() { plant="CZ02", location="3011", description="Sklad_KJ", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapWareHouses() { plant="CZ02", location="3012", description="RN Vegacom", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapWareHouses() { plant="CZ02", location="3013", description="P&E Expedis", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapWareHouses() { plant="CZ02", location="3014", description="Profinet Prod.", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapWareHouses() { plant="CZ02", location="3015", description="Profinet Develop", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapWareHouses() { plant="CZ02", location="4001", description="FS Brno B2B", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapWareHouses() { plant="CZ02", location="4002", description="FS Praha Malesice", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapWareHouses() { plant="CZ02", location="4003", description="FS Ústí n.Labem", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapWareHouses() { plant="CZ02", location="4004", description="FS Liberec", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapWareHouses() { plant="CZ02", location="4005", description="FS Č.Budějovice", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapWareHouses() { plant="CZ02", location="4006", description="FS Plzeň", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapWareHouses() { plant="CZ02", location="4007", description="FS Ostrava B2B", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapWareHouses() { plant="CZ02", location="4008", description="FS Hr.Králové", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapWareHouses() { plant="CZ02", location="4009", description="FS Brno", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapWareHouses() { plant="CZ02", location="4010", description="FS opravy", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapWareHouses() { plant="CZ02", location="4011", description="FS Ostrava", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapWareHouses() { plant="CZ02", location="4012", description="FS Olomouc", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapWareHouses() { plant="CZ02", location="4013", description="FS Expedis", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapWareHouses() { plant="CZ02", location="4014", description="FS Vyšehrad", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapWareHouses() { plant="CZ02", location="4015", description="FS Nagano", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapWareHouses() { plant="CZ02", location="4016", description="FS St. Město", lastUpdate=DateTime.Parse("2021-05-17") },
            };
            return defaultList;
        }

        public static List<SapAreas> SapAreas()
        {
            List<SapAreas> defaultList = new List<SapAreas>() {
                new SapAreas() { plant="CZ02", location="3001", description="RN Krtek", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapAreas() { plant="CZ02", location="3002", description="Sklad_Mobile", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapAreas() { plant="CZ02", location="3003", description="RN Baterie", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapAreas() { plant="CZ02", location="3004", description="RN INS", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapAreas() { plant="CZ02", location="3005", description="RN Hot stock", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapAreas() { plant="CZ02", location="3006", description="RN návrh likvid.", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapAreas() { plant="CZ02", location="3007", description="RN Expedis", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapAreas() { plant="CZ02", location="3008", description="RN zápůjčka", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapAreas() { plant="CZ02", location="3009", description="RN BTS rádia", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapAreas() { plant="CZ02", location="3010", description="RN Exped likvid.", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapAreas() { plant="CZ02", location="3011", description="Sklad_KJ", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapAreas() { plant="CZ02", location="3012", description="RN Vegacom", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapAreas() { plant="CZ02", location="3013", description="P&E Expedis", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapAreas() { plant="CZ02", location="3014", description="Profinet Prod.", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapAreas() { plant="CZ02", location="3015", description="Profinet Develop", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapAreas() { plant="CZ02", location="4001", description="FS Brno B2B", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapAreas() { plant="CZ02", location="4002", description="FS Praha Malesice", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapAreas() { plant="CZ02", location="4003", description="FS Ústí n.Labem", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapAreas() { plant="CZ02", location="4004", description="FS Liberec", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapAreas() { plant="CZ02", location="4005", description="FS Č.Budějovice", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapAreas() { plant="CZ02", location="4006", description="FS Plzeň", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapAreas() { plant="CZ02", location="4007", description="FS Ostrava B2B", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapAreas() { plant="CZ02", location="4008", description="FS Hr.Králové", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapAreas() { plant="CZ02", location="4009", description="FS Brno", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapAreas() { plant="CZ02", location="4010", description="FS opravy", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapAreas() { plant="CZ02", location="4011", description="FS Ostrava", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapAreas() { plant="CZ02", location="4012", description="FS Olomouc", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapAreas() { plant="CZ02", location="4013", description="FS Expedis", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapAreas() { plant="CZ02", location="4014", description="FS Vyšehrad", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapAreas() { plant="CZ02", location="4015", description="FS Nagano", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapAreas() { plant="CZ02", location="4016", description="FS St. Město", lastUpdate=DateTime.Parse("2021-05-17") },
                new SapAreas() { plant="CZ03", location="3004", description="RN INS", lastUpdate=DateTime.Parse("2021-05-17") },
            };
            return defaultList;
        }

        //data part

        private static SapRequests MaterialGetByFilter() //1505
        {

            SapRequests definition = new SapRequests()
            {
                callType = "materialGetByFilter",
                qpf = "https://10.254.118.59:8643/csdg/rpc/OneERP2-Q/tmcz.telekom.it.architecture.MT/MT/PSLIntMaterial/up",
                ppf = "https://hksbpcalp.cz.tmo:8443/csdg/rpc/ProductionInfrastructure/tmcz.telekom.it.architecture.MT/MT/PSLIntMaterial/up",
                apf = "https://10.254.118.59:8443/csdg/rpc/OneERP1-A/tmcz.telekom.it.architecture.MT/MT/PSLIntMaterial/up",
                separator = "data",
                itemSplitter = "Materials",
                xmlMessage = @"<soapenv:Envelope xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/'>
                          <soapenv:Body>
                            <getByFilter xmlns='http://services.tint.telekom.net/EnterpSupp/EnterpEffAndPrjMgmt/Material_v01.00'>
                              <context xmlns='http://services.tint.telekom.net/EnterpSupp/EnterpEffAndPrjMgmt/Material_v01.00/types'>
                                <technicalContext xmlns='http://schemas.telekom.net/csdg_v01.01'>
                                  <from>tmcz.telekom.it.architecture.MT:MT</from>
                                  <routingInfo>tmcz.telekom.it.architecture.MT:MT:PSLIntMaterial</routingInfo>
                                  <messageId>***messageId***</messageId>
                                  <currentSenderTimestampUTC>***TimeStamp***</currentSenderTimestampUTC>
                                  <expiryOffsetInMillis>60000</expiryOffsetInMillis>
                                </technicalContext>
                                <businessContext xmlns='http://schemas.telekom.net/csdg_v01.01'>
                                  <processId>1</processId>
                                  <processTypeId>Masterdata Material synchronization</processTypeId>
                                </businessContext>
                              </context>
                           <data xmlns='http://services.tint.telekom.net/EnterpSupp/EnterpEffAndPrjMgmt/Material_v01.00/types'>
                                <Filter>
                                  <Criterions xmlns=''>
                                        ***Ids***
                                        ***ManufacturerPartNumbers***
                                        ***MaterialDescriptions***
                                  </Criterions>
                                  <ReturnData xmlns=''>
                                    <Header>true</Header>
                                    <Classification>true</Classification>
                                    <PlantDependentData>true</PlantDependentData>
                                    <AlteranativeMeasureUnits>false</AlteranativeMeasureUnits>
                                    <Valuations>false</Valuations>
                                    <AssetClasses>true</AssetClasses>
                                  </ReturnData>
                                </Filter>
                              </data>   
                            </getByFilter>
                          </soapenv:Body>
                        </soapenv:Envelope>",
                lastUpdate = DateTime.Parse("2021-05-17")
            };
            return definition;
        }

        private static SapRequests EquipmentGetByFilter() //1509
        {

            SapRequests definition = new SapRequests()
            {
                callType = "equipmentGetByFilter",
                qpf = "https://10.254.118.59:8643/csdg/rpc/OneERP2-Q/tmcz.telekom.it.architecture.MT/MT/PSLIntEquipment/up",
                ppf = "https://hksbpcalp.cz.tmo:8443/csdg/rpc/ProductionInfrastructure/tmcz.telekom.it.architecture.MT/MT/PSLIntEquipment/up",
                apf = "https://10.254.118.59:8443/csdg/rpc/OneERP1-A/tmcz.telekom.it.architecture.MT/MT/PSLIntEquipment/up",
                separator = "data",
                itemSplitter = "Equipments",
                xmlMessage = @"<soap:Envelope xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns:soap='http://schemas.xmlsoap.org/soap/envelope/'>
                              <soap:Body>
                                <getByFilter xmlns='http://services.tint.telekom.net/EnterpSupp/EnterpEffAndPrjMgmt/Equipment_v01.00'>
                                  <context xmlns='http://services.tint.telekom.net/EnterpSupp/EnterpEffAndPrjMgmt/Equipment_v01.00/types'>
                                    <technicalContext xmlns='http://schemas.telekom.net/csdg_v01.01'>
                                      <from>tmcz.telekom.it.architecture.MT:MT</from>
                                      <routingInfo>tmcz.telekom.it.architecture.MT:MT:PSLIntEquipment</routingInfo>
                                      <messageId>***messageId***</messageId>
                                      <currentSenderTimestampUTC>***TimeStamp***</currentSenderTimestampUTC>
                                      <expiryOffsetInMillis>60000</expiryOffsetInMillis>
                                    </technicalContext>
                                    <businessContext xmlns='http://schemas.telekom.net/csdg_v01.01'>
                                      <processId>1</processId>
                                      <processTypeId>Equipment getByFilter</processTypeId>
                                    </businessContext>
                                  </context>
                                  <data xmlns='http://services.tint.telekom.net/EnterpSupp/EnterpEffAndPrjMgmt/Equipment_v01.00/types'>
                                    <Filter>
                                      <Criterions xmlns=''>
                                        ***UniqueItemIds***
                                        ***ManufacturerSerialNumbers***
                                        ***ConstructionMaterialIds***
                                        ***CategoryIds***
                                        ***FunctionalLocationIds***
                                      </Criterions>
                                      <ReturnData xmlns=''>
                                        <Header>true</Header>
                                        <DateDependentData>true</DateDependentData>
                                        <Classifications>false</Classifications>
                                      </ReturnData>
                                    </Filter>
                                  </data>
                                </getByFilter>
                              </soap:Body>
                            </soap:Envelope>",
                lastUpdate = DateTime.Parse("2021-05-17")
            };
            return definition;
        }

        private static SapRequests TaskListGetByFilter() //1503
        {
            SapRequests definition = new SapRequests()
            {
                callType = "taskListGetByFilter",
                qpf = "https://10.254.118.59:8643/csdg/rpc/OneERP2-Q/tmcz.telekom.it.architecture.MT/MT/PSLIntTaskList/up",
                ppf = "https://hksbpcalp.cz.tmo:8443/csdg/rpc/ProductionInfrastructure/tmcz.telekom.it.architecture.MT/MT/PSLIntTaskList/up",
                apf = "https://10.254.118.59:8443/csdg/rpc/OneERP1-A/tmcz.telekom.it.architecture.MT/MT/PSLIntTaskList/up",
                separator = "data",
                xmlMessage = @"<soap:Envelope xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns:soap='http://schemas.xmlsoap.org/soap/envelope/'>
                              <soap:Body>
                                <q1:getByFilter xmlns='http://services.tint.telekom.net/EnterpSupp/EnterpEffAndPrjMgmt/Material_v01.00' xmlns:q1='http://services.tint.telekom.net/EnterpSupp/EnterpEffAndPrjMgmt/TaskList_v01.00'>
                                  <context xmlns='http://services.tint.telekom.net/EnterpSupp/EnterpEffAndPrjMgmt/TaskList_v01.00/types'>
                                    <technicalContext xmlns='http://schemas.telekom.net/csdg_v01.01'>
                                      <from>tmcz.telekom.it.architecture.MT:MT</from>
                                      <routingInfo>tmcz.telekom.it.architecture.MT:MT:PSLIntTaskList</routingInfo>
                                      <messageId>***messageId***</messageId>
                                      <currentSenderTimestampUTC>***TimeStamp***</currentSenderTimestampUTC>
                                      <expiryOffsetInMillis>60000</expiryOffsetInMillis>
                                    </technicalContext>
                                    <businessContext xmlns='http://schemas.telekom.net/csdg_v01.01'>
                                      <processId>1</processId>
                                      <processTypeId>Masterdata Tasklists synchronization</processTypeId>
                                    </businessContext>
                                  </context>
                                  <data xmlns='http://services.tint.telekom.net/EnterpSupp/EnterpEffAndPrjMgmt/TaskList_v01.00/types'>
                                    <Filter>
                                      <Criterions xmlns=''>
                                        <ClassificationFiltersJoin>true</ClassificationFiltersJoin>
                                        <Classification>
                                          <Selector>***Selector***</Selector>
                                          <Value>***CostCenter***</Value>
                                        </Classification>
                                        <Classification>
                                          <Selector>018/PSL_1000000/PSL_1200000</Selector>
                                          <Value>***Constant***</Value>
                                        </Classification>
                                      </Criterions>
                                      <ReturnData xmlns=''>
                                        <Header>false</Header>
                                        <Operations>true</Operations>
                                        <Components>false</Components>
                                        <Services>false</Services>
                                        <Classifications>true</Classifications>
                                      </ReturnData>
                                    </Filter>
                                  </data>
                                </q1:getByFilter>
                              </soap:Body>
                            </soap:Envelope>",
                lastUpdate = DateTime.Parse("2021-05-17")
            };
            return definition;
        }

        private static SapRequests CSOrderGetByFilter() //1504
        {
            SapRequests definition = new SapRequests()
            {
                callType = "csOrderGetByFilter",
                qpf = "https://10.254.118.59:8643/csdg/rpc/OneERP2-Q/tmcz.telekom.it.architecture.MT/MT/PSLIntCSOrder/up",
                ppf = "https://hksbpcalp.cz.tmo:8443/csdg/rpc/ProductionInfrastructure/tmcz.telekom.it.architecture.MT/MT/PSLIntCSOrder/up",
                apf = "https://10.254.118.59:8443/csdg/rpc/OneERP1-A/tmcz.telekom.it.architecture.MT/MT/PSLIntCSOrder/up",
                separator = "data",
                itemSplitter = "CSOrders",
                xmlMessage = @" <soap:Envelope xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns:soap='http://schemas.xmlsoap.org/soap/envelope/'>
                              <soap:Body>
                                <getByFilter xmlns='http://services.tint.telekom.net/EnterpSupp/EnterpEffAndPrjMgmt/CSOrder_v01.00'>
                                  <context xmlns='http://services.tint.telekom.net/EnterpSupp/EnterpEffAndPrjMgmt/CSOrder_v01.00/types'>
                                    <technicalContext xmlns='http://schemas.telekom.net/csdg_v01.01'>
                                      <from>tmcz.telekom.it.architecture.MT:MT</from>
                                      <routingInfo>tmcz.telekom.it.architecture.MT:MT:PSLIntCSOrder</routingInfo>
                                      <messageId>***messageId***</messageId>
                                      <currentSenderTimestampUTC>***TimeStamp***</currentSenderTimestampUTC>
                                      <expiryOffsetInMillis>60000</expiryOffsetInMillis>
                                    </technicalContext>
                                    <businessContext xmlns='http://schemas.telekom.net/csdg_v01.01'>
                                      <processId>1</processId>
                                      <processTypeId>Online MT - Check CSO on site</processTypeId>
                                    </businessContext>
                                  </context>
                                  <data xmlns='http://services.tint.telekom.net/EnterpSupp/EnterpEffAndPrjMgmt/CSOrder_v01.00/types'>
                                    <Filter>
                                      <Criterions xmlns=''>
                                        ***Ids***
                                        ***WbsElementIds***
                                        ***FunctionalLocationIds***
                                      </Criterions>
                                      <ReturnData xmlns=''>
                                        <Header>***Header***</Header>
                                        <Partners>false</Partners>
                                        <Components>true</Components>
                                        <Operations>true</Operations>
                                        <MaterialDocumentIds>false</MaterialDocumentIds>
                                        <Services>false</Services>
                                        <FinancialOverview>false</FinancialOverview>
                                      </ReturnData>
                                    </Filter>
                                  </data>
                                </getByFilter>
                              </soap:Body>
                            </soap:Envelope>",
                lastUpdate = DateTime.Parse("2021-05-17")
            };
            return definition;
        }

        private static SapRequests CSOrderCreateOrChange() //1504 insert OK
        {
            SapRequests definition = new SapRequests()
            {
                callType = "csOrderCreateOrChange",
                qpf = "https://10.254.118.59:8643/csdg/rpc/OneERP2-Q/tmcz.telekom.it.architecture.MT/MT/PSLIntCSOrder/up",
                ppf = "https://hksbpcalp.cz.tmo:8443/csdg/rpc/ProductionInfrastructure/tmcz.telekom.it.architecture.MT/MT/PSLIntCSOrder/up",
                apf = "https://10.254.118.59:8443/csdg/rpc/OneERP1-A/tmcz.telekom.it.architecture.MT/MT/PSLIntCSOrder/up",
                separator = "data",
                xmlMessage = @"<soap:Envelope xmlns:soap='http://schemas.xmlsoap.org/soap/envelope/' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'>
                              <soap:Body>
                                <createOrChange xmlns='http://services.tint.telekom.net/EnterpSupp/EnterpEffAndPrjMgmt/CSOrder_v01.00'>
                                  <context xmlns='http://services.tint.telekom.net/EnterpSupp/EnterpEffAndPrjMgmt/CSOrder_v01.00/types'>
                                    <technicalContext xmlns='http://schemas.telekom.net/csdg_v01.01'>
                                      <from>tmcz.telekom.it.architecture.MT:MT</from>
                                      <routingInfo>tmcz.telekom.it.architecture.MT:MT:PSLIntCSOrder</routingInfo>
                                      <messageId>***messageId***</messageId>
                                      <currentSenderTimestampUTC>***TimeStamp***</currentSenderTimestampUTC>
                                      <expiryOffsetInMillis>60000</expiryOffsetInMillis>
                                      <property name='Caller.User'>***userName***</property>
                                    </technicalContext>
                                    <businessContext xmlns='http://schemas.telekom.net/csdg_v01.01'>
                                      <processId>1</processId>
                                      <processTypeId>CT3_CSO_new_B2C</processTypeId>
                                    </businessContext>
                                  </context>
                                  <data xmlns='http://services.tint.telekom.net/EnterpSupp/EnterpEffAndPrjMgmt/CSOrder_v01.00/types'>
                                    <CSOrderChanges>
                                      <Action xmlns=''>***type***</Action>
                                      <Id xmlns=''>***Id***</Id>
                                      <Header xmlns=''>
                                        <OrderTypeId>ZI10</OrderTypeId>
                                        ***MainWorkCenter***
                                        <MaintenanceActivityTypeId>700</MaintenanceActivityTypeId>
                                        ***FunctionalLocationId***
                                        ***WbsElementId***
                                        <StartAt>
                                          <HasValue>true</HasValue>
                                          <Value>***TimeStamp***</Value>
                                        </StartAt>
                                        <FinishAt>
                                          <HasValue>true</HasValue>
                                          <Value>***TimeStamp30***</Value>
                                        </FinishAt>
                                        ***TroubleId***
                                        <MaintenancePlanningPlantId>CZ02</MaintenancePlanningPlantId>
                                        <ExternalId>***Id***</ExternalId>
                                      </Header>
                                      ***partnerType***
                                      <Operations xmlns=''>
                                        <Action>***type***</Action>
                                        <TaskListReference>
                                          <TypeId>A</TypeId>
                                          <Group>CZOMMNT</Group>
                                          <GroupCounter>13</GroupCounter>
                                          <Number>0010</Number>
                                        </TaskListReference>
                                        <ControlKey>ZS00</ControlKey>
                                        <IsRelevantForReservationOrPR>2</IsRelevantForReservationOrPR>
                                        <Components>
                                          <Action>***type***</Action>
                                          <Number>***ComponentNumber***</Number>
                                          <MaterialId>***MaterialId***</MaterialId>
                                          <RequirementQuantity>***Quantity***</RequirementQuantity>
                                          <RequirementUnitId>***RequirementUnitId***</RequirementUnitId>
                                          <RequirementDate>***TimeStamp***</RequirementDate>
                                          <StorageLocation>***StorageLocation***</StorageLocation>
                                          <PlantId>CZ02</PlantId>
                                          <CategoryId>***CategoryId***</CategoryId>
                                        </Components>
                                      </Operations>
                                    </CSOrderChanges>
                                  </data>
                                </createOrChange>
                              </soap:Body>
                            </soap:Envelope>",
                lastUpdate = DateTime.Parse("2021-05-17")
            };
            return definition;
        }


        private static SapRequests FunctionalLocationGetByFilter() //1507
        {
            SapRequests definition = new SapRequests()
            {
                callType = "functionalLocationGetByFilter",
                qpf = "https://10.254.118.59:8643/csdg/rpc/OneERP2-Q/tmcz.telekom.it.architecture.MT/MT/PSLIntFunctionalLocation/up",
                ppf = "https://hksbpcalp.cz.tmo:8443/csdg/rpc/ProductionInfrastructure/tmcz.telekom.it.architecture.MT/MT/PSLIntFunctionalLocation/up",
                apf = "https://10.254.118.59:8443/csdg/rpc/OneERP1-A/tmcz.telekom.it.architecture.MT/MT/PSLIntFunctionalLocation/up",
                separator = "data",
                xmlMessage = @" <soap:Envelope xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns:soap='http://schemas.xmlsoap.org/soap/envelope/'>
                              <soap:Body>
                                <getByFilter xmlns='http://services.tint.telekom.net/EnterpSupp/EnterpEffAndPrjMgmt/FunctionalLocation_v01.00'>
                                  <context xmlns='http://services.tint.telekom.net/EnterpSupp/EnterpEffAndPrjMgmt/FunctionalLocation_v01.00/types'>
                                    <technicalContext xmlns='http://schemas.telekom.net/csdg_v01.01'>
                                      <from>tmcz.telekom.it.architecture.MT:MT</from>
                                      <routingInfo>tmcz.telekom.it.architecture.MT:MT:PSLIntFunctionalLocation</routingInfo>
                                      <messageId>***messageId***</messageId>
                                      <currentSenderTimestampUTC>***TimeStamp***</currentSenderTimestampUTC>
                                      <expiryOffsetInMillis>60000</expiryOffsetInMillis>
                                    </technicalContext>
                                    <businessContext xmlns='http://schemas.telekom.net/csdg_v01.01'>
                                      <processId>1</processId>
                                      <processTypeId>FunctionalLocation getByFilter</processTypeId>
                                    </businessContext>
                                  </context>
                                  <data xmlns='http://services.tint.telekom.net/EnterpSupp/EnterpEffAndPrjMgmt/FunctionalLocation_v01.00/types'>
                                    <Filter>
                                      <Criterions xmlns=''>
                                        <IdBlocks>
                                          <Index>3</Index>
                                          <Value>***SiteNumber***</Value>
                                        </IdBlocks>
                                      </Criterions>
                                      <ReturnData xmlns=''>
                                        <Header>true</Header>
                                        <Classifications>false</Classifications>
                                      </ReturnData>
                                    </Filter>
                                  </data>
                                </getByFilter>
                              </soap:Body>
                            </soap:Envelope>",
                lastUpdate = DateTime.Parse("2021-05-17")
            };
            return definition;
        }

        private static SapRequests MaterialDocumentCreateOrChange() //1506
        {
            SapRequests definition = new SapRequests()
            {
                callType = "materialDocumentCreateOrChange",
                qpf = "https://10.254.118.59:8643/csdg/rpc/OneERP2-Q/tmcz.telekom.it.architecture.MT/MT/PSLIntMaterialDocument/up",
                ppf = "https://hksbpcalp.cz.tmo:8443/csdg/rpc/ProductionInfrastructure/tmcz.telekom.it.architecture.MT/MT/PSLIntMaterialDocument/up",
                apf = "https://10.254.118.59:8443/csdg/rpc/OneERP1-A/tmcz.telekom.it.architecture.MT/MT/PSLIntMaterialDocument/up",
                separator = "data",
                xmlMessage = @"<soap:Envelope xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns:soap='http://schemas.xmlsoap.org/soap/envelope/'>
                              <soap:Body>
                                <createOrChange xmlns='http://services.tint.telekom.net/EnterpSupp/EnterpEffAndPrjMgmt/MaterialDocument_v01.00'>
                                  <context xmlns='http://services.tint.telekom.net/EnterpSupp/EnterpEffAndPrjMgmt/MaterialDocument_v01.00/types'>
                                    <technicalContext xmlns='http://schemas.telekom.net/csdg_v01.01'>
                                      <from>tmcz.telekom.it.architecture.MT:MT</from>
                                      <routingInfo>tmcz.telekom.it.architecture.MT:MT:PSLIntMaterialDocument</routingInfo>
                                      <messageId>***messageId***</messageId>
                                      <currentSenderTimestampUTC>***TimeStamp***</currentSenderTimestampUTC>
                                      <expiryOffsetInMillis>60000</expiryOffsetInMillis>
                                      <property name='Caller.User'>***userName***</property>
                                    </technicalContext>
                                    <businessContext xmlns='http://schemas.telekom.net/csdg_v01.01'>
                                      <processId>1</processId>
                                      <processTypeId>CT10 Execute GR for PO </processTypeId>
                                    </businessContext>
                                  </context>
                                  <data xmlns='http://services.tint.telekom.net/EnterpSupp/EnterpEffAndPrjMgmt/MaterialDocument_v01.00/types'>
                                    <MaterialDocumentChanges>
                                      <Id xmlns=''>***messageId***</Id>
                                      <MovementCode xmlns=''>***MovementCode***</MovementCode>
                                      <Header xmlns=''>
                                        <PostingDate>***date***</PostingDate>
                                        <DocumentDate>***date***</DocumentDate>
                                        <Description>Auto-generate</Description>
                                      </Header>
                                      <Items xmlns=''>
                                        <Quantity>***Quantity***</Quantity>
                                        <UnitId>***UnitId***</UnitId>
                                        ***MovementIndicator***
                                        ***MovementTypeId***
                                        ***PlantId***
                                        <StorageLocationCode>***StorageLocationCode***</StorageLocationCode>
                                        <MaterialId>***MaterialId***</MaterialId>
                                        <BatchNumber>***BatchNumber***</BatchNumber>
                                        ***ReceivingIssuingPlant***
                                        ***ReceivingIssuingStorageLocationCode***
                                        ***ReceivingIssuingBatchNumber***
                                        <SerialNumbers>
                                          ***UniqueItemId***
                                        </SerialNumbers>
                                        <ReservationItemId>
                                          ***HeaderId***
                                          ***ItemNumber***
                                        </ReservationItemId>
                                      </Items>
                                    </MaterialDocumentChanges>
                                  </data>
                                </createOrChange>
                              </soap:Body>
                            </soap:Envelope>",
                lastUpdate = DateTime.Parse("2021-05-17")
            };
            return definition;
        }

        private static SapRequests MaterialCheckAvailability() //1505
        {

            SapRequests definition = new SapRequests()
            {
                callType = "materialCheckAvailability",
                qpf = "https://10.254.118.59:8643/csdg/rpc/OneERP2-Q/tmcz.telekom.it.architecture.MT/MT/PSLIntMaterial/up",
                ppf = "https://hksbpcalp.cz.tmo:8443/csdg/rpc/ProductionInfrastructure/tmcz.telekom.it.architecture.MT/MT/PSLIntMaterial/up",
                apf = "https://10.254.118.59:8443/csdg/rpc/OneERP1-A/tmcz.telekom.it.architecture.MT/MT/PSLIntMaterial/up",
                separator = "data",
                xmlMessage = @"<soapenv:Envelope xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/'>
                          <soapenv:Body>
                            <checkAvailability xmlns='http://services.tint.telekom.net/EnterpSupp/EnterpEffAndPrjMgmt/Material_v01.00'>
                              <context xmlns='http://services.tint.telekom.net/EnterpSupp/EnterpEffAndPrjMgmt/Material_v01.00/types'>
                                <technicalContext xmlns='http://schemas.telekom.net/csdg_v01.01'>
                                  <from>tmcz.telekom.it.architecture.MT:MT</from>
                                  <routingInfo>tmcz.telekom.it.architecture.MT:MT:PSLIntMaterial</routingInfo>
                                  <messageId>***messageId***</messageId>
                                  <currentSenderTimestampUTC>***TimeStamp***</currentSenderTimestampUTC>
                                  <expiryOffsetInMillis>60000</expiryOffsetInMillis>
                                </technicalContext>
                                <businessContext xmlns='http://schemas.telekom.net/csdg_v01.01'>
                                  <processId>1</processId>
                                  <processTypeId>CT9 checkAvailability</processTypeId>
                                </businessContext>
                              </context>
                           <data xmlns='http://services.tint.telekom.net/EnterpSupp/EnterpEffAndPrjMgmt/Material_v01.00/types'>
                            <Filters>
                              <MaterialId>***MaterialId***</MaterialId>
                              <CheckRuleId>03</CheckRuleId>
                              <AtpSegmentTypeIds>50</AtpSegmentTypeIds>
                              <MrpAreaId>***MrpAreaId***</MrpAreaId>
                            </Filters>
                          </data>
                        </checkAvailability>
                      </soapenv:Body>
                    </soapenv:Envelope>",
                lastUpdate = DateTime.Parse("2021-05-17")
            };
            return definition;
        }

        private static SapRequests ServiceNotificationCreateOrChange() //1508
        {

            SapRequests definition = new SapRequests()
            {
                callType = "serviceNotificationCreateOrChange",
                qpf = "https://10.254.118.59:8643/csdg/rpc/OneERP2-Q/tmcz.telekom.it.architecture.MT/MT/PSLIntServiceNotification/up",
                ppf = "https://hksbpcalp.cz.tmo:8443/csdg/rpc/ProductionInfrastructure/tmcz.telekom.it.architecture.MT/MT/PSLIntServiceNotification/up",
                apf = "https://10.254.118.59:8443/csdg/rpc/OneERP1-A/tmcz.telekom.it.architecture.MT/MT/PSLIntServiceNotification/up",
                separator = "data",
                xmlMessage = @"<soap:Envelope xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns:soap='http://schemas.xmlsoap.org/soap/envelope/'>
                              <soap:Body>
                                <createOrChange xmlns='http://services.tint.telekom.net/EnterpSupp/EnterpEffAndPrjMgmt/ServiceNotification_v01.00'>
                                  <context xmlns='http://services.tint.telekom.net/EnterpSupp/EnterpEffAndPrjMgmt/ServiceNotification_v01.00/types'>
                                    <technicalContext xmlns='http://schemas.telekom.net/csdg_v01.01'>
                                      <from>tmcz.telekom.it.architecture.MT:MT</from>
                                      <routingInfo>tmcz.telekom.it.architecture.MT:MT:PSLIntServiceNotification</routingInfo>
                                      <messageId>***messageId***</messageId>
                                      <currentSenderTimestampUTC>***TimeStamp***</currentSenderTimestampUTC>
                                      <expiryOffsetInMillis>60000</expiryOffsetInMillis>
                                      <property name='Caller.User'>***userName***</property>
                                    </technicalContext>
                                    <businessContext xmlns='http://schemas.telekom.net/csdg_v01.01'>
                                      <processId>1</processId>
                                      <processTypeId>CT3-CSO-release</processTypeId>
                                    </businessContext>
                                  </context>
                                  <data xmlns='http://services.tint.telekom.net/EnterpSupp/EnterpEffAndPrjMgmt/ServiceNotification_v01.00/types'>
                                    <ServiceNotificationChanges>
                                      <Action xmlns=''>I</Action>
                                      <Id xmlns=''>***messageId***</Id>
                                      <Header xmlns=''>
                                        <TypeId>***TypeId***</TypeId>
                                        <OrderId>***OrderId***</OrderId>
                                      </Header>
                                    </ServiceNotificationChanges>
                                  </data>
                                </createOrChange>
                              </soap:Body>
                            </soap:Envelope>",
                lastUpdate = DateTime.Parse("2021-05-17")
            };
            return definition;
        }

        public class CsOrderCreateOrChangeRequest {

            public string OrderId { get; set; }
            public string FunctionalLocationId { get; set; }
            public string CategoryId { get; set; }
            public string TicketId { get; set; }
            public string Quantity { get; set; }
            public string ComponentNumber { get; set; }
            public string MaterialId { get; set; }
            public string RequirementUnitId { get; set; }
            public bool Insert { get; set; }
        }

        public class FunctionalLocationsResponse
        {
            public string Id { get; set; }
            public string MaterialId { get; set; }
            public string UIID { get; set; }
            public string SN { get; set; }
            public string MaterialName { get; set; }
            public string FunctionalLocationsId { get; set; }

        }

        public class MaterialResponse
        {
            public string MaterialId { get; set; }
            public string Mnformated { get; set; }
            public string Mnoriginal { get; set; }
            public string UnitId { get; set; }
            public string MaterialName { get; set; }
            public string Pn { get; set; }
            public string SerialNumberProfileId { get; set; }
            public string AssetClassId { get; set; }
            public string ValuationCategoryId { get; set; }
            public bool asEquipInfo { get; set; }
        }

        public class CSOrderResponse
        {
            public string Id { get; set; }
            public string OperationRoutingNumberInOrder { get; set; }
            public string GeneralCounterForOrder { get; set; }
            public string TypeId { get; set; }
            public string Group { get; set; }
            public string GroupCounter { get; set; }
            public string Number { get; set; }
            public string MaterialId { get; set; }
        }

        public class TaskListResponse
        {
            public string TypeId { get; set; }
            public string Group { get; set; }
            public string GroupCounter { get; set; }
            public string Number { get; set; }
        }

        public class checkAvaiabilityResponse
        {
            public string Area { get; set; }
            public string ItemName { get; set; }
            public string Pn { get; set; }
            public string Mn { get; set; }
            public string WarehouseNumber { get; set; }
            public string WarehouseName { get; set; }
            public string Plant { get; set; }
            public string QuantityFree { get; set; }
            public string QuantityBlocked { get; set; }
            public string QuantityInTransfer { get; set; }
            public string UnitId { get; set; }
        }

        /// SAP prepare functions
        public class SapUIID
        {
            public string inputText { get; set; }
            public string UiidRequest { get; set; }
            public string codeType { get; set; }
            public string mnInserted { get; set; }
            public string mnFormated { get; set; }
            public string pnInserted { get; set; }
            public string snInserted { get; set; }
            public string EquipmentNumber { get; set; }
            public string FunctionalLocationId { get; set; }
            public string Status { get; set; }
            public string Text { get; set; }
            public string UnitId { get; set; }
            public string MovementCode { get; set; }
            public string MovementIndicator { get; set; }
            public string MovementTypeId { get; set; }
            public string PlantId { get; set; }
            public string BatchNumber { get; set; }
            public string ReceivingIssuingPlant { get; set; }
            public string ReceivingIssuingStorageLocationCode { get; set; }
            public string ReceivingIssuingBatchNumber { get; set; }
            public string HeaderId { get; set; }
            public string ItemNumber { get; set; }
            public string MrpAreaId { get; set; }
            public string SiteNumber { get; set; }

            public string sourceLocation { get; set; }
            public string targetLocation { get; set; }
            public bool preparedFor1506 { get; set; }

            public string AssetClassId { get; set; }
            public bool SapMaterialRequestSuccess { get; set; }

            public string ValuationCategoryId { get; set; }
            public string SerialNumberProfileId { get; set; }
            public string Quantity { get; set; }

            public string AssetClass { get; set; }
            public bool SapEquipmentRequestSuccess { get; set; }
            public string SapError { get; set; }

            public bool SapFunctionalLocationsSuccess { get; set; }
            public bool SapTaskListRequestSuccess { get; set; }
            public bool SapCsOrderRequest24Success { get; set; }

            public string newOrderId { get; set; }
            public bool SapCsOrderCreateOrChangeRequestSuccess { get; set; }
            public bool SapServiceNotificationRequestRequestSuccess { get; set; }
            public bool SapCsOrderRequest07Success { get; set; }
        }

        public static SapUIID GenerateUIID(SapUIID GenerateUIIDSource)
        {
            SapUIID GenerateUIID = SystemFunctions.Clone(ref GenerateUIIDSource);
            
            bool definitionFouded = false;
            if (!String.IsNullOrWhiteSpace(GenerateUIID.inputText))
            {
                GenerateUIID.UiidRequest = null;
                UIIDDefinitions defaultUIIDFormat = App.UIIDDefinitions.Find(item => GenerateUIID.inputText.StartsWith(item.detectCode) || item.defaultRequest);

                if (defaultUIIDFormat.defaultRequest)
                { //generate uiid by definitions
                    if (String.IsNullOrWhiteSpace(GenerateUIID.mnInserted) && String.IsNullOrWhiteSpace(GenerateUIID.pnInserted))
                    {
                        App.SupplierCodesDefinitions.ForEach(delegate (SupplierCodesDefinitions item)
                        {
                            if (!definitionFouded)
                            {
                                try
                                {

                                    // mn exist
                                    if (item.codeType == GenerateUIID.codeType && (item.codePrefix == null || (item.codePrefix != null && GenerateUIID.inputText.StartsWith(item.codePrefix != null ? item.codePrefix : ""))) && item.mnExist && item.segmentLength == GenerateUIID.inputText.Split((int.TryParse(item.separator, out _) ? ((char)int.Parse(item.separator)).ToString().ToCharArray() : item.separator.ToCharArray())).Length)
                                    {
                                        string[] requestParts = GenerateUIID.inputText.Split((int.TryParse(item.separator, out _) ? ((char)int.Parse(item.separator)).ToString().ToCharArray() : item.separator.ToCharArray()));
                                        GenerateUIID.mnInserted = requestParts[(int)item.mnSegment].Remove(0, item.mnPrefixSeparator.Length).Remove(requestParts[(int)item.mnSegment].Length - item.mnPrefixSeparator.Length - item.mnSuffixSeparator.Length, item.mnSuffixSeparator.Length).TrimStart(item.mnRemoveCharacterOnStart ? item.mnRemoveCharacter[0] : new char());
                                        GenerateUIID.mnFormated = mnFormatted(requestParts[(int)item.mnSegment].Remove(0, item.mnPrefixSeparator.Length).Remove(requestParts[(int)item.mnSegment].Length - item.mnPrefixSeparator.Length - item.mnSuffixSeparator.Length, item.mnSuffixSeparator.Length).TrimStart(item.mnRemoveCharacterOnStart ? item.mnRemoveCharacter[0] : new char()));
                                    }

                                    //sn exist
                                    if (GenerateUIID.SapMaterialRequestSuccess && item.codeType == GenerateUIID.codeType && (item.codePrefix == null || (item.codePrefix != null && GenerateUIID.inputText.StartsWith(item.codePrefix != null ? item.codePrefix : ""))) && item.snExist && item.segmentLength == GenerateUIID.inputText.Split((int.TryParse(item.separator, out _) ? ((char)int.Parse(item.separator)).ToString().ToCharArray() : item.separator.ToCharArray())).Length)
                                    {
                                        string[] requestParts = GenerateUIID.inputText.Split((int.TryParse(item.separator, out _) ? ((char)int.Parse(item.separator)).ToString().ToCharArray() : item.separator.ToCharArray()));
                                        GenerateUIID.snInserted = requestParts[(int)item.snSegment].Remove(0, item.snPrefixSeparator.Length).Remove(requestParts[(int)item.snSegment].Length - item.snPrefixSeparator.Length - item.snSuffixSeparator.Length, item.snSuffixSeparator.Length).TrimStart(item.snRemoveCharacterOnStart ? item.snRemoveCharacter[0] : new char());
                                    }

                                    //pn exist
                                    if (item.codeType == GenerateUIID.codeType && (item.codePrefix == null || (item.codePrefix != null && GenerateUIID.inputText.StartsWith(item.codePrefix != null ? item.codePrefix : ""))) && item.pnExist && item.segmentLength == GenerateUIID.inputText.Split((int.TryParse(item.separator, out _) ? ((char)int.Parse(item.separator)).ToString().ToCharArray() : item.separator.ToCharArray())).Length)
                                    {
                                        string[] requestParts = GenerateUIID.inputText.Split((int.TryParse(item.separator, out _) ? ((char)int.Parse(item.separator)).ToString().ToCharArray() : item.separator.ToCharArray()));
                                        GenerateUIID.pnInserted = requestParts[(int)item.pnSegment].Remove(0, item.pnPrefixSeparator.Length).Remove(requestParts[(int)item.pnSegment].Length - item.pnPrefixSeparator.Length - item.pnSuffixSeparator.Length, item.pnSuffixSeparator.Length).TrimStart(item.pnRemoveCharacterOnStart ? item.pnRemoveCharacter[0] : new char());
                                    }


                                    if (item.codeType == GenerateUIID.codeType && (item.codePrefix == null || (item.codePrefix != null && GenerateUIID.inputText.StartsWith(item.codePrefix != null ? item.codePrefix : ""))) && item.mnExist && item.snExist && item.segmentLength == GenerateUIID.inputText.Split((int.TryParse(item.separator, out _) ? ((char)int.Parse(item.separator)).ToString().ToCharArray() : item.separator.ToCharArray())).Length)
                                    {
                                        string[] requestParts = GenerateUIID.inputText.Split((int.TryParse(item.separator, out _) ? ((char)int.Parse(item.separator)).ToString().ToCharArray() : item.separator.ToCharArray()));
                                        GenerateUIID.pnInserted = "";
                                        GenerateUIID.mnInserted = requestParts[(int)item.mnSegment].Remove(0, item.mnPrefixSeparator.Length).Remove(requestParts[(int)item.mnSegment].Length - item.mnPrefixSeparator.Length - item.mnSuffixSeparator.Length, item.mnSuffixSeparator.Length).TrimStart(item.mnRemoveCharacterOnStart ? item.mnRemoveCharacter[0] : new char());
                                        GenerateUIID.mnFormated = mnFormatted(requestParts[(int)item.mnSegment].Remove(0, item.mnPrefixSeparator.Length).Remove(requestParts[(int)item.mnSegment].Length - item.mnPrefixSeparator.Length - item.mnSuffixSeparator.Length, item.mnSuffixSeparator.Length).TrimStart(item.mnRemoveCharacterOnStart ? item.mnRemoveCharacter[0] : new char()));
                                        GenerateUIID.snInserted = requestParts[(int)item.snSegment].Remove(0, item.snPrefixSeparator.Length).Remove(requestParts[(int)item.snSegment].Length - item.snPrefixSeparator.Length - item.snSuffixSeparator.Length, item.snSuffixSeparator.Length).TrimStart(item.snRemoveCharacterOnStart ? item.snRemoveCharacter[0] : new char());

                                        GenerateUIID.UiidRequest = defaultUIIDFormat.prefix
                                            + defaultUIIDFormat.snPrefix
                                            + ((defaultUIIDFormat.snLength == null)
                                                    ? requestParts[(int)item.snSegment].Remove(0, item.snPrefixSeparator.Length).Remove(requestParts[(int)item.snSegment].Length - item.snPrefixSeparator.Length - item.snSuffixSeparator.Length, item.snSuffixSeparator.Length).TrimStart(item.snRemoveCharacterOnStart ? item.snRemoveCharacter[0] : new char())
                                                    : requestParts[(int)item.snSegment].Remove(0, item.snPrefixSeparator.Length).Remove(requestParts[(int)item.snSegment].Length - item.snPrefixSeparator.Length - item.snSuffixSeparator.Length, item.snSuffixSeparator.Length).TrimStart(item.snRemoveCharacterOnStart ? item.snRemoveCharacter[0] : new char()).PadLeft((int)defaultUIIDFormat.snLength, defaultUIIDFormat.snfillCharacter[0]))
                                                + defaultUIIDFormat.mnPrefix
                                                + ((defaultUIIDFormat.mnLength == null)
                                                    ? requestParts[(int)item.mnSegment].Remove(0, item.mnPrefixSeparator.Length).Remove(requestParts[(int)item.mnSegment].Length - item.mnPrefixSeparator.Length - item.mnSuffixSeparator.Length, item.mnSuffixSeparator.Length).TrimStart(item.mnRemoveCharacterOnStart ? item.mnRemoveCharacter[0] : new char())
                                                    : requestParts[(int)item.mnSegment].Remove(0, item.mnPrefixSeparator.Length).Remove(requestParts[(int)item.mnSegment].Length - item.mnPrefixSeparator.Length - item.mnSuffixSeparator.Length, item.mnSuffixSeparator.Length).TrimStart(item.mnRemoveCharacterOnStart ? item.mnRemoveCharacter[0] : new char()).PadLeft((int)defaultUIIDFormat.mnLength, defaultUIIDFormat.mnfillCharacter[0]))
                                                ;
                                        definitionFouded = true;
                                    }
                                }
                                catch (Exception) { }
                            }
                        });

                        if (!definitionFouded && string.IsNullOrWhiteSpace(GenerateUIID.mnFormated) && string.IsNullOrWhiteSpace(GenerateUIID.pnInserted))// kod nebyl detekovan prirazuji PN nebo MN
                        {
                            if (MathFunctions.IsNumeric(GenerateUIID.inputText)) { GenerateUIID.mnFormated = mnFormatted(GenerateUIID.inputText); GenerateUIID.mnInserted = GenerateUIID.inputText; } else { GenerateUIID.pnInserted = GenerateUIID.inputText; }
                        }

                    }
                    else if (!String.IsNullOrWhiteSpace(GenerateUIID.mnInserted)) // UIID from MN/SN
                    {
                        if (String.IsNullOrWhiteSpace(GenerateUIID.snInserted))
                        {
                            GenerateUIID.snInserted = ((defaultUIIDFormat.snLength == null)
                                                 ? GenerateUIID.inputText
                                                 : GenerateUIID.inputText.PadLeft((int)defaultUIIDFormat.snLength, defaultUIIDFormat.snfillCharacter[0]));
                        }

                        GenerateUIID.UiidRequest = defaultUIIDFormat.prefix
                                     + defaultUIIDFormat.snPrefix
                                     + ((defaultUIIDFormat.snLength == null)
                                             ? GenerateUIID.snInserted
                                             : GenerateUIID.snInserted.PadLeft((int)defaultUIIDFormat.snLength, defaultUIIDFormat.snfillCharacter[0]))
                                         + defaultUIIDFormat.mnPrefix
                                         + ((defaultUIIDFormat.mnLength == null)
                                             ? GenerateUIID.mnFormated
                                             : GenerateUIID.mnFormated.PadLeft((int)defaultUIIDFormat.mnLength, defaultUIIDFormat.mnfillCharacter[0]))
                                         ;
                        definitionFouded = true;
                    }
                    else if (!String.IsNullOrWhiteSpace(GenerateUIID.pnInserted))
                    { // SEPARATE PN ONLY OR PN/SN
                      //TODO SEPARATING BY Definitions

                        if (String.IsNullOrWhiteSpace(GenerateUIID.snInserted)) GenerateUIID.snInserted = GenerateUIID.inputText;

                    }
                }
                else
                {   //CZTM request
                    GenerateUIID.UiidRequest = GenerateUIID.inputText;
                    definitionFouded = true;
                }
            }
            return GenerateUIID;
        }

        public static string mnFormatted(string mnNumber)
        {
            if (!String.IsNullOrWhiteSpace(mnNumber))
            {
                SapFormats mnFormat = App.SapFormats.Find(item => item.fieldName == "mn");
                return mnNumber.TrimStart(mnFormat.removeCharacterOnStart ? mnFormat.removeCharacter[0] : new char()).PadLeft((int)mnFormat.length, mnFormat.fillCharacter[0]);
            }
            else { return mnNumber; }
        }

    }
}
