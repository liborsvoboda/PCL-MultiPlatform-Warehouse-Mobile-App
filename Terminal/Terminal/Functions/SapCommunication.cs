using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Xml;
using Terminal.DbModels;
using static Terminal.Database.SapDefinitions;

namespace Terminal
{
    public class SapCommunication
    {

        public static XmlDocument SendSapRequests(SapRequests definition)
        {
            try
            {
                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (httpSender, cert, chain, sslPolicyErrors) => { return true; };
                HttpClient client = new HttpClient(clientHandler);

                StringContent data = new StringContent(definition.xmlMessage, Encoding.UTF8, "application/xml");
                var response = client.PostAsync((string)definition.GetType().GetProperty(App.SapConnections.type).GetValue(definition, null), data).GetAwaiter().GetResult();

                return new XmlDocument { InnerXml = response.Content.ReadAsStringAsync().GetAwaiter().GetResult() };
            }
            catch (Exception) { return new XmlDocument(); }
        }


        public static List<List<SapResponses>> DeserializeInnerSoapObject(string soapResponse, SapRequests definition)
        {
            List<List<SapResponses>> finalResponse = new List<List<SapResponses>>();
            try
            {
                bool responseResult;

                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(soapResponse.Replace("n0:", "").Replace("n1:", ""));
                XmlNode responseXml = xmlDocument.GetElementsByTagName(definition.separator)[0];
                XmlNode responseNodes;

                if (responseXml != null)
                {
                    if (responseXml.InnerText.Length != 0)
                    {
                        int allcycle = 1;
                        if (!string.IsNullOrWhiteSpace(definition.itemSplitter)) allcycle = responseXml.SelectNodes(definition.itemSplitter).Count;

                        for (int cycle = 0; cycle < allcycle; cycle++)
                        {
                            responseResult = true;
                            if (!string.IsNullOrWhiteSpace(definition.itemSplitter)) responseNodes = responseXml.SelectNodes(definition.itemSplitter)[cycle];
                            else { responseNodes = responseXml; }

                            List<SapResponses> response = new List<SapResponses>();

                            App.SapResponses.FindAll(item => item.callType == definition.callType).ForEach(responseItem =>
                            {
                                SapResponses item = new SapResponses() { name = responseItem.name, callType = responseItem.callType, prefixForCheck = responseItem.prefixForCheck, valueForCheck = responseItem.valueForCheck, valueType = responseItem.valueType, required = responseItem.required, sum = responseItem.sum };

                                if (responseItem.required && responseItem.level != null && responseItem.valueType != (int)ValueTypes.List)
                                {
                                    if (!string.IsNullOrWhiteSpace(responseItem.prefixForCheck))
                                    {
                                        item.status = (responseNodes.SelectNodes(responseItem.route)[(int)responseItem.level].InnerText.StartsWith(responseItem.prefixForCheck)) ? true : false;
                                        item.realValue = (responseItem.removePrefix) ? responseNodes.SelectNodes(responseItem.route)[(int)responseItem.level].InnerText.TrimStart(responseItem.prefixForCheck.ToCharArray()) : responseNodes.SelectNodes(responseItem.route)[(int)responseItem.level].InnerText;
                                    }
                                    else { item.realValue = responseNodes.SelectNodes(responseItem.route)[(int)responseItem.level].InnerText; item.status = true; }

                                    if (!string.IsNullOrWhiteSpace(responseItem.valueForCheck)) { if (item.realValue == responseItem.valueForCheck) { item.status = true; } } else { item.status = true; }
                                    response.Add(item);
                                }

                                else if (responseItem.required && responseItem.level == null && responseItem.valueType != (int)ValueTypes.List)
                                {
                                    if (!string.IsNullOrWhiteSpace(responseItem.prefixForCheck))
                                    {
                                        item.status = (responseNodes.SelectSingleNode(responseItem.route).InnerText.StartsWith(responseItem.prefixForCheck)) ? true : false;
                                        item.realValue = (responseItem.removePrefix) ? responseNodes.SelectSingleNode(responseItem.route).InnerText.TrimStart(responseItem.prefixForCheck.ToCharArray()) : responseNodes.SelectSingleNode(responseItem.route).InnerText;
                                    }
                                    else {
                                        item.realValue = responseNodes.SelectSingleNode(responseItem.route).InnerText; item.status = true; }

                                    if (!string.IsNullOrWhiteSpace(responseItem.valueForCheck)) { if (item.realValue == responseItem.valueForCheck) { item.status = true; } } else { item.status = true; }
                                    response.Add(item);
                                }
                                else if (responseItem.required && responseItem.valueType == (int)ValueTypes.List)
                                {
                                    if (!string.IsNullOrWhiteSpace(responseItem.prefixForCheck) && string.IsNullOrWhiteSpace(responseItem.valueForCheck))
                                    {
                                        if (responseItem.removePrefix)
                                        {
                                            if (responseItem.sum)
                                            {
                                                item.realValue = responseNodes.SelectNodes(responseItem.route).Cast<XmlNode>().Where(node => node.InnerText.StartsWith(responseItem.prefixForCheck)).Select(node => node.InnerText.TrimStart(responseItem.prefixForCheck.ToCharArray())).Sum(node => float.Parse(node, CultureInfo.InvariantCulture.NumberFormat)).ToString();
                                            }
                                            else
                                            {
                                                item.realValue = string.Join(",", responseNodes.SelectNodes(responseItem.route).Cast<XmlNode>().Where(node => node.InnerText.StartsWith(responseItem.prefixForCheck)).Select(node => node.InnerText.TrimStart(responseItem.prefixForCheck.ToCharArray())).ToList());
                                            }
                                        }
                                        else {
                                            if (responseItem.sum) {
                                                item.realValue = responseNodes.SelectNodes(responseItem.route).Cast<XmlNode>().Where(node => node.InnerText.StartsWith(responseItem.prefixForCheck)).Select(node => node.InnerText).Sum(node => float.Parse(node, CultureInfo.InvariantCulture.NumberFormat)).ToString();
                                            }
                                            else
                                            {
                                                item.realValue = string.Join(",", responseNodes.SelectNodes(responseItem.route).Cast<XmlNode>().Where(node => node.InnerText.StartsWith(responseItem.prefixForCheck)).Select(node => node.InnerText).ToList());
                                            }
                                        }

                                    }
                                    else if (!responseItem.cannotExist && !string.IsNullOrWhiteSpace(responseItem.valueForCheck))
                                    {
                                        item.realValue = responseNodes.SelectNodes(responseItem.route).Cast<XmlNode>().Select(node => node.InnerText).ToList().Contains(responseItem.valueForCheck) ? responseItem.valueForCheck : null;
                                    }
                                    else
                                    {
                                        if (responseItem.sum)
                                        {
                                            item.realValue = responseNodes.SelectNodes(responseItem.route).Cast<XmlNode>().Select(node => node.InnerText).Sum(node => float.Parse(node, CultureInfo.InvariantCulture.NumberFormat)).ToString();
                                        }
                                        else
                                        {
                                            item.realValue = string.Join(",", responseNodes.SelectNodes(responseItem.route).Cast<XmlNode>().Select(node => node.InnerText).ToList());
                                        }
                                    }
                                    if (responseItem.reverseCount) item.realValue = item.realValue.Replace("-", "");
                                    if (!string.IsNullOrWhiteSpace(item.realValue)) { item.status = true; }

                                    if (item.realValue != "NotExistOk") response.Add(item);
                                }
                                else if (!responseItem.required && responseItem.level != null && responseItem.cannotExist && responseItem.valueType != (int)ValueTypes.List)
                                {

                                    item.status = responseNodes.SelectNodes(responseItem.route).Count > 0 ? responseNodes.SelectNodes(responseItem.route)[(int)responseItem.level] == null : true;

                                }
                                else if (!responseItem.required && responseItem.level == null && responseItem.cannotExist && responseItem.valueType != (int)ValueTypes.List)
                                {
                                    item.status = responseNodes.SelectSingleNode(responseItem.route) == null;
                                }
                                else if (!responseItem.required && responseItem.level != null && !responseItem.cannotExist && responseItem.valueType != (int)ValueTypes.List)
                                {
                                    try
                                    {
                                        item.realValue = responseNodes.SelectNodes(responseItem.route).Count > 0 ? responseNodes.SelectNodes(responseItem.route)[(int)responseItem.level].InnerText : null;
                                        item.status = true;
                                        response.Add(item);
                                    }
                                    catch (Exception) { }
                                }
                                else if (!responseItem.required && responseItem.level == null && !responseItem.cannotExist && responseItem.valueType != (int)ValueTypes.List)
                                {
                                    try
                                    {
                                        if (responseNodes.SelectSingleNode(responseItem.route) != null)
                                        {
                                            item.realValue = responseNodes.SelectSingleNode(responseItem.route).InnerText; item.status = true;
                                            response.Add(item);
                                        }
                                    }
                                    catch (Exception) { item.status = true; }
                                }
                                else if (!responseItem.required && responseItem.valueType == (int)ValueTypes.List)
                                {
                                    if (!string.IsNullOrWhiteSpace(responseItem.prefixForCheck))
                                    {
                                        if (responseItem.removePrefix)
                                        {
                                            if (responseItem.sum)
                                            {
                                                item.realValue = responseNodes.SelectNodes(responseItem.route).Cast<XmlNode>().Where(node => node.InnerText.StartsWith(responseItem.prefixForCheck)).Select(node => node.InnerText.TrimStart(responseItem.prefixForCheck.ToCharArray())).Sum(node => float.Parse(node, CultureInfo.InvariantCulture.NumberFormat)).ToString();
                                            }
                                            else
                                            {
                                                item.realValue = string.Join(",", responseNodes.SelectNodes(responseItem.route).Cast<XmlNode>().Where(node => node.InnerText.StartsWith(responseItem.prefixForCheck)).Select(node => node.InnerText.TrimStart(responseItem.prefixForCheck.ToCharArray())).ToList());
                                            }
                                        }
                                        else
                                        {
                                            if (responseItem.sum)
                                            {
                                                item.realValue = responseNodes.SelectNodes(responseItem.route).Cast<XmlNode>().Where(node => node.InnerText.StartsWith(responseItem.prefixForCheck)).Select(node => node.InnerText).Sum(node => float.Parse(node, CultureInfo.InvariantCulture.NumberFormat)).ToString();
                                            }
                                            else
                                            {
                                                item.realValue = string.Join(",", responseNodes.SelectNodes(responseItem.route).Cast<XmlNode>().Where(node => node.InnerText.StartsWith(responseItem.prefixForCheck)).Select(node => node.InnerText).ToList());
                                            }
                                        }
                                    }
                                    else if (responseItem.cannotExist && !string.IsNullOrWhiteSpace(responseItem.valueForCheck))
                                    {
                                        item.realValue = responseNodes.SelectNodes(responseItem.route).Cast<XmlNode>().Select(node => node.InnerText).ToList().Contains(responseItem.valueForCheck) ? null : "NotExistOk";
                                    }
                                    else
                                    {
                                        if (responseItem.sum)
                                        {
                                            item.realValue = responseNodes.SelectNodes(responseItem.route).Cast<XmlNode>().Select(node => node.InnerText).Sum(node => float.Parse(node, CultureInfo.InvariantCulture.NumberFormat)).ToString();
                                        } else
                                        {
                                            item.realValue = string.Join(",", responseNodes.SelectNodes(responseItem.route).Cast<XmlNode>().Select(node => node.InnerText).ToList());
                                        }
                                    }

                                    if (responseItem.reverseCount) item.realValue = item.realValue.Replace("-", "");

                                    if (responseItem.cannotExist && item.realValue == null) item.status = false;
                                    else item.status = true;

                                    if (item.realValue != "NotExistOk") response.Add(item);
                                }

                                if (!item.status) responseResult = false;
                            });
                            if (responseResult) finalResponse.Add(response);
                        }
                    }
                }
                return finalResponse;
            }
            catch (Exception e) { return finalResponse; }
        }


        public static List<List<SapResponses>> materialRequest(SapUIID SapUIIDGenerated,bool descriptionCall)
        {
            SapRequests definition = DatabaseCommunication.GetSapDefinition("materialGetByFilter");
            definition.xmlMessage = definition.xmlMessage
                .Replace("***TimeStamp***", DateTime.Now.ToUniversalTime().AddHours(TimeZoneInfo.Local.GetUtcOffset(DateTime.Now).Hours).ToString("yyyy-MM-ddTHH:mm:ss.fffZ"))
                .Replace("***messageId***", new DateTimeOffset(DateTime.Now.ToUniversalTime().AddHours(TimeZoneInfo.Local.GetUtcOffset(DateTime.Now).Hours)).ToUnixTimeSeconds().ToString())
                .Replace("***Ids***", (!string.IsNullOrWhiteSpace(SapUIIDGenerated.mnFormated)) ? "<Ids>" + SapUIIDGenerated.mnFormated + "</Ids>" : "")
                .Replace("***ManufacturerPartNumbers***", (string.IsNullOrWhiteSpace(SapUIIDGenerated.mnFormated) && !string.IsNullOrWhiteSpace(SapUIIDGenerated.pnInserted) && !descriptionCall) ? "<ManufacturerPartNumbers>" + SapUIIDGenerated.pnInserted + "</ManufacturerPartNumbers>" : "")
                .Replace("***MaterialDescriptions***", (descriptionCall && string.IsNullOrWhiteSpace(SapUIIDGenerated.mnFormated)) ? "<MaterialDescriptions>*" + SapUIIDGenerated.pnInserted + "*</MaterialDescriptions>" : "");

            return  DeserializeInnerSoapObject(SendSapRequests(definition).InnerXml, definition);
        }


        public static List<List<SapResponses>> equipmentSiteRequest(FunctionalLocationsResponse siterequest)
        {
            SapRequests definition; List<List<SapResponses>> responseData = new List<List<SapResponses>>();

                definition = DatabaseCommunication.GetSapDefinition("equipmentGetByFilter");
                definition.xmlMessage = definition.xmlMessage
                    .Replace("***TimeStamp***", DateTime.Now.ToUniversalTime().AddHours(TimeZoneInfo.Local.GetUtcOffset(DateTime.Now).Hours).ToString("yyyy-MM-ddTHH:mm:ss.fffZ"))
                    .Replace("***messageId***", new DateTimeOffset(DateTime.Now.ToUniversalTime().AddHours(TimeZoneInfo.Local.GetUtcOffset(DateTime.Now).Hours)).ToUnixTimeSeconds().ToString())
                    .Replace("***CategoryIds***", "")
                    .Replace("***FunctionalLocationIds***", "<FunctionalLocationIds>" + siterequest.FunctionalLocationsId + "</FunctionalLocationIds>")
                    .Replace("***UniqueItemIds***", "")
                    .Replace("***ManufacturerSerialNumbers***", "")
                    .Replace("***ConstructionMaterialIds***", "<ConstructionMaterialIds>" + siterequest.MaterialId+ "</ConstructionMaterialIds>");

                responseData = DeserializeInnerSoapObject(SendSapRequests(definition).InnerXml, definition);

            return responseData;
        }

        public static List<List<SapResponses>> equipmentRequest(SapUIID SapUIIDGenerated)
        {
            string originalSn = SapUIIDGenerated.snInserted;
            string extendedSn = "S" + SapUIIDGenerated.snInserted;
            SapRequests definition; List<List<SapResponses>> responseData = new List<List<SapResponses>>();

            int requestLimit = (SapUIIDGenerated.UiidRequest == null) ? 2 : 4;
            for (int i = 0; i < requestLimit; i++)
            {
                definition = DatabaseCommunication.GetSapDefinition("equipmentGetByFilter");
                definition.xmlMessage = definition.xmlMessage
                    .Replace("***TimeStamp***", DateTime.Now.ToUniversalTime().AddHours(TimeZoneInfo.Local.GetUtcOffset(DateTime.Now).Hours).ToString("yyyy-MM-ddTHH:mm:ss.fffZ"))
                    .Replace("***messageId***", new DateTimeOffset(DateTime.Now.ToUniversalTime().AddHours(TimeZoneInfo.Local.GetUtcOffset(DateTime.Now).Hours)).ToUnixTimeSeconds().ToString())
                    .Replace("***CategoryIds***", "")
                    .Replace("***FunctionalLocationIds***", "")
                    ;
                

                if (i == 1 || i == 3) { SapUIIDGenerated.snInserted = extendedSn; SapUIIDGenerated = GenerateUIID(SapUIIDGenerated); SapUIIDGenerated.snInserted = extendedSn; }
                if (i > 1) SapUIIDGenerated.UiidRequest = null;

                if (!string.IsNullOrWhiteSpace(SapUIIDGenerated.UiidRequest)) { // test uuid 
                    definition.xmlMessage = definition.xmlMessage
                        .Replace("***UniqueItemIds***", "<UniqueItemIds>" + SapUIIDGenerated.UiidRequest + "</UniqueItemIds>")
                        .Replace("***ManufacturerSerialNumbers***", "")
                        .Replace("***ConstructionMaterialIds***", "");
                } else {
                    definition.xmlMessage = definition.xmlMessage
                          .Replace("***UniqueItemIds***", "")
                          .Replace("***ManufacturerSerialNumbers***", "<ManufacturerSerialNumbers>" + SapUIIDGenerated.snInserted + "</ManufacturerSerialNumbers>")
                          .Replace("***ConstructionMaterialIds***", "<ConstructionMaterialIds>" + SapUIIDGenerated.mnFormated + "</ConstructionMaterialIds>");
                    
                }

                responseData = DeserializeInnerSoapObject(SendSapRequests(definition).InnerXml, definition);
                if (responseData.Count > 0)
                {
                    break;
                }
            }

            return responseData;
        }

        public static List<List<SapResponses>> materialDocumentRequest(SapUIID SapUIIDGenerated)
        {
            SapRequests definition = DatabaseCommunication.GetSapDefinition("materialDocumentCreateOrChange");
            definition.xmlMessage = definition.xmlMessage
                  .Replace("***TimeStamp***", DateTime.Now.ToUniversalTime().AddHours(TimeZoneInfo.Local.GetUtcOffset(DateTime.Now).Hours).ToString("yyyy-MM-ddTHH:mm:ss.fffZ"))
                  .Replace("***messageId***", new DateTimeOffset(DateTime.Now.ToUniversalTime().AddHours(TimeZoneInfo.Local.GetUtcOffset(DateTime.Now).Hours)).ToUnixTimeSeconds().ToString())
                  .Replace("***MovementCode***", SapUIIDGenerated.MovementCode)
                  .Replace("***userName***", App.actualUser.ldapSurname+" "+ App.actualUser.ldapName + " " + App.actualUser.mail)
                  .Replace("***date***", DateTime.Now.ToUniversalTime().AddHours(TimeZoneInfo.Local.GetUtcOffset(DateTime.Now).Hours).ToString("yyyy-MM-dd"))
                  .Replace("***Quantity***", string.IsNullOrWhiteSpace(SapUIIDGenerated.Quantity) ? "1" : SapUIIDGenerated.Quantity)
                  .Replace("***UnitId***", SapUIIDGenerated.UnitId)
                  .Replace("***MovementIndicator***", string.IsNullOrWhiteSpace(SapUIIDGenerated.MovementIndicator)? "": "<MovementIndicator>" + SapUIIDGenerated.MovementIndicator+ "</MovementIndicator>")
                  .Replace("***MovementTypeId***", string.IsNullOrWhiteSpace(SapUIIDGenerated.MovementTypeId) ? "" : "<MovementTypeId>" + SapUIIDGenerated.MovementTypeId + "</MovementTypeId>")
                  .Replace("***PlantId***", string.IsNullOrWhiteSpace(SapUIIDGenerated.PlantId) ? "" : "<PlantId>" + SapUIIDGenerated.PlantId + "</PlantId>")
                  .Replace("***StorageLocationCode***", SapUIIDGenerated.sourceLocation)
                  .Replace("***MaterialId***", SapUIIDGenerated.mnFormated)
                  .Replace("***BatchNumber***", SapUIIDGenerated.BatchNumber)
                  .Replace("***ReceivingIssuingPlant***", string.IsNullOrWhiteSpace(SapUIIDGenerated.ReceivingIssuingPlant) ? "" : "<ReceivingIssuingPlant>"+ SapUIIDGenerated.ReceivingIssuingPlant + "</ReceivingIssuingPlant>")
                  .Replace("***ReceivingIssuingStorageLocationCode***", string.IsNullOrWhiteSpace(SapUIIDGenerated.ReceivingIssuingStorageLocationCode) ? "" : "<ReceivingIssuingStorageLocationCode>" + SapUIIDGenerated.ReceivingIssuingStorageLocationCode + "</ReceivingIssuingStorageLocationCode>")
                  .Replace("***ReceivingIssuingBatchNumber***", string.IsNullOrWhiteSpace(SapUIIDGenerated.ReceivingIssuingBatchNumber) ? "" : "<ReceivingIssuingBatchNumber>" + SapUIIDGenerated.ReceivingIssuingBatchNumber + "</ReceivingIssuingBatchNumber>")
                  .Replace("***UniqueItemId***", string.IsNullOrWhiteSpace(SapUIIDGenerated.UiidRequest) ? "" : "<UniqueItemId>" + SapUIIDGenerated.UiidRequest + "</UniqueItemId>")
                  .Replace("***HeaderId***", string.IsNullOrWhiteSpace(SapUIIDGenerated.HeaderId) ? "" : "<HeaderId>" + SapUIIDGenerated.HeaderId + "</HeaderId>")
                  .Replace("***ItemNumber***", string.IsNullOrWhiteSpace(SapUIIDGenerated.ItemNumber) ? "" : "<ItemNumber>" + SapUIIDGenerated.ItemNumber + "</ItemNumber>")
                  ;

            return DeserializeInnerSoapObject(SendSapRequests(definition).InnerXml, definition);
        }

        public static List<List<SapResponses>> materialCheckAvailabilityRequest(SapUIID SapUIIDGenerated)
        {
            SapRequests definition = DatabaseCommunication.GetSapDefinition("materialCheckAvailability");
            definition.xmlMessage = definition.xmlMessage
                  .Replace("***TimeStamp***", DateTime.Now.ToUniversalTime().AddHours(TimeZoneInfo.Local.GetUtcOffset(DateTime.Now).Hours).ToString("yyyy-MM-ddTHH:mm:ss.fffZ"))
                  .Replace("***messageId***", new DateTimeOffset(DateTime.Now.ToUniversalTime().AddHours(TimeZoneInfo.Local.GetUtcOffset(DateTime.Now).Hours)).ToUnixTimeSeconds().ToString())
                  .Replace("***MaterialId***", SapUIIDGenerated.mnFormated)
                  .Replace("***MrpAreaId***", SapUIIDGenerated.MrpAreaId)
                  ;

            return DeserializeInnerSoapObject(SendSapRequests(definition).InnerXml, definition);
        }

        public static List<List<SapResponses>> functionalLocationRequest(SapUIID SapUIIDGenerated)
        {
            SapRequests definition = DatabaseCommunication.GetSapDefinition("functionalLocationGetByFilter");
            definition.xmlMessage = definition.xmlMessage
                  .Replace("***TimeStamp***", DateTime.Now.ToUniversalTime().AddHours(TimeZoneInfo.Local.GetUtcOffset(DateTime.Now).Hours).ToString("yyyy-MM-ddTHH:mm:ss.fffZ"))
                  .Replace("***messageId***", new DateTimeOffset(DateTime.Now.ToUniversalTime().AddHours(TimeZoneInfo.Local.GetUtcOffset(DateTime.Now).Hours)).ToUnixTimeSeconds().ToString())
                  .Replace("***SiteNumber***", SapUIIDGenerated.SiteNumber)
                  ;

            return DeserializeInnerSoapObject(SendSapRequests(definition).InnerXml, definition);
        }

        public static List<List<SapResponses>> taskListRequest(SapUIID SapUIIDGenerated)
        {
            SapRequests definition = DatabaseCommunication.GetSapDefinition("taskListGetByFilter");
            definition.xmlMessage = definition.xmlMessage
                  .Replace("***TimeStamp***", DateTime.Now.ToUniversalTime().AddHours(TimeZoneInfo.Local.GetUtcOffset(DateTime.Now).Hours).ToString("yyyy-MM-ddTHH:mm:ss.fffZ"))
                  .Replace("***messageId***", new DateTimeOffset(DateTime.Now.ToUniversalTime().AddHours(TimeZoneInfo.Local.GetUtcOffset(DateTime.Now).Hours)).ToUnixTimeSeconds().ToString())
                  .Replace("***Selector***", !string.IsNullOrWhiteSpace(SapUIIDGenerated.AssetClass) ? "018/PSL_1000000/PSL_1000001" : !string.IsNullOrWhiteSpace(SapUIIDGenerated.AssetClassId) ? "018/PSL_1000000/PSL_1000001" : "018/PSL_1000000/PSL_1100001")
                  .Replace("***CostCenter***", !string.IsNullOrWhiteSpace(SapUIIDGenerated.AssetClass) ? SapUIIDGenerated.AssetClass : !string.IsNullOrWhiteSpace(SapUIIDGenerated.AssetClassId) ? SapUIIDGenerated.AssetClassId.Split(',')[0] : App.actualUser.costCenter)
                  .Replace("***Constant***", !string.IsNullOrWhiteSpace(SapUIIDGenerated.AssetClass) ? "ERW" : !string.IsNullOrWhiteSpace(SapUIIDGenerated.AssetClassId) ? "ERW" : "AUF")
                  ;

            return DeserializeInnerSoapObject(SendSapRequests(definition).InnerXml, definition);
        }

        public static List<List<SapResponses>> csOrderRequest(string data, bool ids, bool header = true)
        {
            SapRequests definition = DatabaseCommunication.GetSapDefinition("csOrderGetByFilter");
            definition.xmlMessage = definition.xmlMessage
                  .Replace("***TimeStamp***", DateTime.Now.ToUniversalTime().AddHours(TimeZoneInfo.Local.GetUtcOffset(DateTime.Now).Hours).ToString("yyyy-MM-ddTHH:mm:ss.fffZ"))
                  .Replace("***messageId***", new DateTimeOffset(DateTime.Now.ToUniversalTime().AddHours(TimeZoneInfo.Local.GetUtcOffset(DateTime.Now).Hours)).ToUnixTimeSeconds().ToString())
                  .Replace("***Ids***", ids ? "<Ids>" + data + "</Ids>" : "")
                  .Replace("***WbsElementIds***", !ids ? "<WbsElementIds>"+App.actualUser.wbs+"</WbsElementIds>" : "")
                  .Replace("***FunctionalLocationIds***", !ids ? "<FunctionalLocationIds>" + data + "</FunctionalLocationIds>" : "")
                  .Replace("***Header***", header ? "true" : "false")
                  ;

            return DeserializeInnerSoapObject(SendSapRequests(definition).InnerXml, definition);
        }

        public static List<List<SapResponses>> csOrderCreateOrChangeRequest(CsOrderCreateOrChangeRequest data)
        {
            SapRequests definition = DatabaseCommunication.GetSapDefinition("csOrderCreateOrChange");
            definition.xmlMessage = definition.xmlMessage
                  .Replace("***TimeStamp***", DateTime.Now.ToUniversalTime().AddHours(TimeZoneInfo.Local.GetUtcOffset(DateTime.Now).Hours).ToString("yyyy-MM-ddTHH:mm:ss.fffZ"))
                  .Replace("***TimeStamp30***", DateTime.Now.ToUniversalTime().AddDays(30).AddHours(TimeZoneInfo.Local.GetUtcOffset(DateTime.Now).Hours).ToString("yyyy-MM-ddTHH:mm:ss.fffZ"))
                  .Replace("***messageId***", new DateTimeOffset(DateTime.Now.ToUniversalTime().AddHours(TimeZoneInfo.Local.GetUtcOffset(DateTime.Now).Hours)).ToUnixTimeSeconds().ToString())
                  .Replace("***userName***", App.actualUser.ldapSurname + " " + App.actualUser.ldapName + " " + App.actualUser.mail)
                  .Replace("***type***", data.Insert ? "I" : "U")
                  .Replace("***Id***", data.OrderId)
                  .Replace("***MainWorkCenter***", data.Insert ? "<MainWorkCenter>" + App.actualUser.WorkCenter + "</MainWorkCenter>" : "")
                  .Replace("***FunctionalLocationId***", data.Insert ? "<FunctionalLocationId>" + data.FunctionalLocationId + "</FunctionalLocationId>" : "")
                  .Replace("***WbsElementId***", data.Insert ? "<WbsElementId>" + App.actualUser.wbs + "</WbsElementId>" : "")
                  .Replace("***TroubleId***", data.Insert ? "<Description>MT online " + data.TicketId + "</Description>" : "")
                  .Replace("***partnerType***", data.Insert ? "<Partners xmlns=''><Action>I</Action><RoleId>ZQ</RoleId><PartnerNumber>"+App.actualUser.cid+ "</PartnerNumber></Partners>" : "")

                  
                  .Replace("***ComponentNumber***", data.ComponentNumber)
                  .Replace("***MaterialId***", data.MaterialId)
                  .Replace("***Quantity***", data.Quantity)
                  .Replace("***RequirementUnitId***", data.RequirementUnitId)
                  .Replace("***StorageLocation***", App.actualUser.location)
                  .Replace("***CategoryId***", data.CategoryId)
                  ;

            return DeserializeInnerSoapObject(SendSapRequests(definition).InnerXml, definition);
        }

        public static List<List<SapResponses>> serviceNotificationCreateOrChangeRequest(string typeId, string orderId)
        {
            SapRequests definition = DatabaseCommunication.GetSapDefinition("serviceNotificationCreateOrChange");
            definition.xmlMessage = definition.xmlMessage
                  .Replace("***userName***", App.actualUser.ldapSurname + " " + App.actualUser.ldapName + " " + App.actualUser.mail)
                  .Replace("***TimeStamp***", DateTime.Now.ToUniversalTime().AddHours(TimeZoneInfo.Local.GetUtcOffset(DateTime.Now).Hours).ToString("yyyy-MM-ddTHH:mm:ss.fffZ"))
                  .Replace("***messageId***", new DateTimeOffset(DateTime.Now.ToUniversalTime().AddHours(TimeZoneInfo.Local.GetUtcOffset(DateTime.Now).Hours)).ToUnixTimeSeconds().ToString())
                  .Replace("***TypeId***", typeId)
                  .Replace("***OrderId***", orderId)
                  ;

            return DeserializeInnerSoapObject(SendSapRequests(definition).InnerXml, definition);
        }
        
    }
}
