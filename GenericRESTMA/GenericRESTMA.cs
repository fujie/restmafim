#define TRACE
using Microsoft.MetadirectoryServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;

namespace FimSync_Ezma
{
    //
    // To do
    // - Cache Access Token
    // - Password Management : partially implemented but not tested with PCNS yet
    // - Delta Import
    // - Additional Attribute Types Support for Multi Value Attributes
    // - More Error Handling
    // - More Validation for FIM ConfigParameters (emailAddress format, exsistence of pluginLibrary etc)

    class ConstDefinition
    {
        public const string CFG_PLUGIN_FILENAME = "Plugin File Name";
        public const string CFG_JWT_SUB = "Subject(sub)";
        public const string CFG_JWT_ISS = "Issuer(iss)";
        public const string CFG_JWT_KEY_FILENAME = "Private Key File";
        public const string CFG_JWT_KEY_PASSWORD = "Private Key Password";
        public const string CFG_PROXY_SERVER = "Server Address";
        public const string CFG_PROXY_USERNAME = "Username";
        public const string CFG_PROXY_PASSWORD = "Password";
        public const int    CFG_IMPORT_MAX_PAGE_SIZE = 100;
        public const int    CFG_IMPORT_DEFAULT_PAGE_SIZE = 100;
        public const int    CFG_EXPORT_MAX_PAGE_SIZE = 100;
        public const int    CFG_EXPORT_DEFAULT_PAGE_SIZE = 100;
        public const string MSG0000_START_CAPABILITIES = "Starting Capabilities";
        public const string MSG0100_START_GETSCHEMA = "Starting GetSchema";
        public const string MSG0150_VERBOSE_GETSCHEMA = "GetSchema : ";
        public const string MSG0199_ERROR_GETSCHEMA = "Error in GetSchema : ";
        public const string MSG0200_START_GETCONFIGPARAMETERS = "Starting GetConfigParameters";
        public const string MSG0299_ERROR_GETCONFIGPARAMETERS = " Error in GetConfigParameters : ";
        public const string MSG0300_START_VALIDATECONFIGPARAMETERS = "Starting ValidateConfigParameters";
        public const string MSG0399_ERROR_VALIDATECONFIGPARAMETERS = "Error in ValidateConfigParameters : ";
        public const string MSG0400_START_OPENIMPORTCONNECTION = "Starting OpenImportConnection";
        public const string MSG0450_VERBOSE_OPENIMPORTCONNECTION = "OpenImportConnection : ";
        public const string MSG0499_ERROR_OPENIMPORTCONNECTION = "Error in OpenImportConnection : ";
        public const string MSG0500_START_GETIMPORTENTRIES = "Starting GetImportEntries";
        public const string MSG0550_VERBOSE_GETIMPORTENTRIES = "GetImportEntries : ";
        public const string MSG0599_ERROR_GETIMPORTENTRIES = "Error in GetImportEntries : ";
        public const string MSG0600_START_CLOSEIMPORTCONNECTION = "Starting CloseImportConnection";
        public const string MSG0650_VERBOSE_CLOSEIMPORTCONNECTION = "CloseImportConnection : ";
        public const string MSG0700_START_OPENEXPORTCONNECTION = "Starting OpenExportConnection";
        public const string MSG0750_VERBOSE_OPENEXPORTCONNECTION = "OpenExportConnection : ";
        public const string MSG0799_ERROR_OPENEXPORTCONNECTION = "Error in OpenExportConnection : ";
        public const string MSG0800_START_PUTEXPORTENTRIES = "Starting PutExportEntries";
        public const string MSG0850_VERBOSE_PUTEXPORTENTRIES = "PutExportEntries : ";
        public const string MSG0899_ERROR_PUTEXPORTENTRIES = "Error in PutExportEntries : ";
        public const string MSG0900_START_CLOSEEXPORTCONNECTION = "Starting CloseExportConnection";
        public const string MSG1000_START_OPENPASSWORDCONNECTION = "Starting OpenPasswordConnection";
        public const string MSG1050_VERBOSE_OPENPASSWORDCONNECTION = "OpenPasswordConnection : ";
        public const string MSG1099_ERROR_OPENPASSWORDCONNECTION = "Error in OpenPasswordConnection : ";
        public const string MSG1100_START_CLOSEPASSWORDCONNECTION = "Starting ClosePasswordConnection";
        public const string MSG1200_START_SETPASSWORD = "Starting SetPassword";
        public const string MSG1299_ERROR_SETPASSWORD = "Error in SetPassword : ";
        public const string MSG1300_START_CHANGEPASSWORD = "Starting ChangePassword";
        public const string MSG1399_ERROR_CHANGEPASSWORD = "Error in ChangePassword : ";
        public const string MSG1400_START_REQUIRECHANGEPASSWORDONNEXTLOGIN = "Starting RequireChangePasswordOnNextLogin";
        public const string MSG1499_ERROR_REQUIRECHANGEPASSWORDONNEXTLOGIN = "Error in RequireChangePasswordOnNextLogin : ";
        public const int    ID0000_START_CAPABILITIES = 0;
        public const int    ID0100_START_GETSCHEMA = 100;
        public const int    ID0150_VERBOSE_GETSCHEMA = 150;
        public const int    ID0199_ERROR_GETSCHEMA = 199;
        public const int    ID0200_START_GETCONFIGPARAMETERS = 200;
        public const int    ID0299_ERROR_GETCONFIGPARAMETERS = 299;
        public const int    ID0300_START_VALIDATECONFIGPARAMETERS = 300;
        public const int    ID0399_ERROR_VALIDATECONFIGPARAMETERS = 399;
        public const int    ID0400_START_OPENIMPORTCONNECTION = 400;
        public const int    ID0450_VERBOSE_OPENIMPORTCONNECTION = 450;
        public const int    ID0499_ERROR_OPENIMPORTCONNECTION = 499;
        public const int    ID0500_START_GETIMPORTENTRIES = 500;
        public const int    ID0550_VERBOSE_GETIMPORTENTRIES = 550;
        public const int    ID0599_ERROR_GETIMPORTENTRIES = 599;
        public const int    ID0600_START_CLOSEIMPORTCONNECTION = 600;
        public const int    ID0650_VERBOSE_CLOSEIMPORTCONNECTION = 650;
        public const int    ID0699_ERROR_CLOSEIMPORTCONNECTION = 699;
        public const int    ID0700_START_OPENEXPORTCONNECTION = 700;
        public const int    ID0750_VERBOSE_OPENEXPORTCONNECTION = 750;
        public const int    ID0799_ERROR_OPENEXPORTCONNECTION = 799;
        public const int    ID0800_START_PUTEXPORTENTRIES = 800;
        public const int    ID0850_VERBOSE_PUTEXPORTENTRIES = 850;
        public const int    ID0899_ERROR_PUTEXPORTENTRIES = 899;
        public const int    ID0900_START_CLOSEEXPORTCONNECTION = 900;
        public const int    ID1000_START_OPENPASSWORDCONNECTION = 1000;
        public const int    ID1050_VERBOSE_OPENPASSWORDCONNECTION = 1050;
        public const int    ID1099_ERROR_OPENPASSWORDCONNECTION = 1099;
        public const int    ID1100_START_CLOSEPASSWORDCONNECTION = 1100;
        public const int    ID1200_START_SETPASSWORD = 1200;
        public const int    ID1299_ERROR_SETPASSWORD = 1299;
        public const int    ID1300_START_CHANGEPASSWORD = 1300;
        public const int    ID1399_ERROR_CHANGEPASSWORD = 1399;
        public const int    ID1400_START_REQUIRECHANGEPASSWORDONNEXTLOGIN = 1400;
        public const int    ID1499_ERROR_REQUIRECHANGEPASSWORDONNEXTLOGIN = 1499;
    }

    public class EzmaExtension :
    IMAExtensible2CallExport,
    IMAExtensible2CallImport,
    IMAExtensible2GetSchema,
    IMAExtensible2GetCapabilities,
    IMAExtensible2GetParameters,
    IMAExtensible2Password
    {
        //
        // global variables
        //
        List<string> targetObjectTypes = null;
        OperationType operationType;
        string accessToken = null;
        Dictionary<string, KeyedCollection<string, SchemaAttribute>> _fimSchemas;
        string domain = null;
        string pagingToken = null;
        int importPagesize = ConstDefinition.CFG_IMPORT_DEFAULT_PAGE_SIZE;
        // Plugin
        dynamic pluginInstance;
        // Proxy Server Type
        Utils.ProxyServerType proxyServerType;
        WebProxy webProxy = null;
        // utility
        Utils utils;

        public EzmaExtension()
        {
            // Create Utility Instance
            utils = new Utils();
        }
        public MACapabilities Capabilities
        {
            get
            {
                utils.Logger(TraceEventType.Information,
                             ConstDefinition.ID0000_START_CAPABILITIES,
                             ConstDefinition.MSG0000_START_CAPABILITIES);
                return new MACapabilities
                {
                    ObjectRename = false,
                    ObjectConfirmation = MAObjectConfirmation.Normal,
                    DeleteAddAsReplace = true,
                    DeltaImport = false,
                    DistinguishedNameStyle = MADistinguishedNameStyle.None,
                    ExportType = MAExportType.AttributeUpdate,
                    NoReferenceValuesInFirstExport = false,
                    FullExport = true,
                    Normalizations = MANormalizations.None
                };
            }
        }
        // Get Schema
        public Schema GetSchema(KeyedCollection<string, ConfigParameter> _configParameters)
        {
            utils.Logger(TraceEventType.Information,
                         ConstDefinition.ID0100_START_GETSCHEMA,
                         ConstDefinition.MSG0100_START_GETSCHEMA);
            try
            {
                var _schema = Schema.Create();
                pluginInstance = utils.GetPluginLibraryInstance(_configParameters);
                Dictionary<Tuple<string, string>, Tuple<Type, bool, bool, bool, bool>> _schemaCollection = pluginInstance.GetSchema();
                string _previousObjectClass = null;
                SchemaType _objectType = null;

                foreach (KeyValuePair<Tuple<string, string>, Tuple<Type, bool, bool, bool, bool>> _eachSchema in _schemaCollection.OrderBy(p => p.Key.Item1))
                {
                    // Object Class Definition
                    if (_previousObjectClass != _eachSchema.Key.Item1)
                    {
                        if (_previousObjectClass != null)
                        {
                            // Add ObjectType Definition in previous loop on Management Agent
                            utils.Logger(TraceEventType.Verbose,
                                         ConstDefinition.ID0150_VERBOSE_GETSCHEMA,
                                         ConstDefinition.MSG0150_VERBOSE_GETSCHEMA + "Define new object type on Management Agent : " + _objectType.Name);
                            _schema.Types.Add(_objectType);
                        }
                        // Create a new objectClass
                        utils.Logger(TraceEventType.Verbose,
                                     ConstDefinition.ID0150_VERBOSE_GETSCHEMA,
                                     ConstDefinition.MSG0150_VERBOSE_GETSCHEMA + "Create new object type : " + _eachSchema.Key.Item1);
                        _objectType = SchemaType.Create(_eachSchema.Key.Item1, false);
                        _previousObjectClass = _eachSchema.Key.Item1;
                    }
                    var _attribute = _eachSchema.Value;
                    // Attribute Type Definition
                    AttributeType _attributeType = AttributeType.Undefined;
                    switch (Type.GetTypeCode(_attribute.Item1))
                    {
                        case TypeCode.String:
                            _attributeType = AttributeType.String;
                            break;
                        case TypeCode.Boolean:
                            _attributeType = AttributeType.Boolean;
                            break;
                        case TypeCode.Byte:
                            _attributeType = AttributeType.Binary;
                            break;
                        case TypeCode.Int64:
                            _attributeType = AttributeType.Integer;
                            break;
                        default:
                            // 'AttributeType.Undefined' is not defined in ECMA2.0(ex.Microsoft.MetadirectoryServicesEx.dll ver. 4.1.3419.0)
                            // You must use ECMA 2.2 or later, or delete this line.
                            _attributeType = AttributeType.Undefined;
                            break;
                    }
                    // Allowed Attribute Operation
                    AttributeOperation _attributeOperation = AttributeOperation.ImportExport;
                    if (_attribute.Item2)
                    {
                        // can read
                        if (_attribute.Item3)
                        {
                            // can write
                            _attributeOperation = AttributeOperation.ImportExport;
                        }
                        else
                        {
                            // read only
                            _attributeOperation = AttributeOperation.ImportOnly;
                        }
                    }
                    else
                    {
                        // cannot read
                        if (_attribute.Item3)
                        {
                            // can write
                            _attributeOperation = AttributeOperation.ExportOnly;
                        }
                        else
                        {
                            // cannot write -> cannot do anything
                            utils.Logger(TraceEventType.Error,
                                         ConstDefinition.ID0199_ERROR_GETSCHEMA,
                                         ConstDefinition.MSG0199_ERROR_GETSCHEMA + "Invalid attribute operation");
                            throw new ExtensibleExtensionException(ConstDefinition.MSG0199_ERROR_GETSCHEMA + "Invalid attribute operation");
                        }
                    }
                    // Source Anchor Definition
                    if (_attribute.Item4)
                    {
                        _objectType.Attributes.Add(SchemaAttribute.CreateAnchorAttribute(_eachSchema.Key.Item2,
                                                                                        _attributeType,
                                                                                        _attributeOperation));
                    }
                    else
                    {
                        if (_attribute.Item5)
                        {
                            // multi value attribute
                            // not support boolean type for multiple value
                            if (_attributeType == AttributeType.Boolean)
                            {
                                utils.Logger(TraceEventType.Error,
                                             ConstDefinition.ID0199_ERROR_GETSCHEMA,
                                             ConstDefinition.MSG0199_ERROR_GETSCHEMA + "Defining multi valued attribute as bool is not supported");
                                throw new ExtensibleExtensionException(ConstDefinition.MSG0199_ERROR_GETSCHEMA + "Defining multi valued attribute as bool is not supported");
                            }
                            else
                            {
                                _objectType.Attributes.Add(SchemaAttribute.CreateMultiValuedAttribute(_eachSchema.Key.Item2,
                                                                                                      _attributeType,
                                                                                                      _attributeOperation));
                            }
                        }
                        else
                        {
                            // single vaule attribute
                            _objectType.Attributes.Add(SchemaAttribute.CreateSingleValuedAttribute(_eachSchema.Key.Item2,
                                                                                                  _attributeType,
                                                                                                  _attributeOperation));
                        }
                    }
                }
                // Add last ObjectType Definition to MA Schema
                _schema.Types.Add(_objectType);
                return _schema;
            }
            catch (Exception ex)
            {
                utils.Logger(TraceEventType.Error,
                             ConstDefinition.ID0199_ERROR_GETSCHEMA,
                             ConstDefinition.MSG0199_ERROR_GETSCHEMA + ex.Message);
                throw new ExtensibleExtensionException(ConstDefinition.MSG0199_ERROR_GETSCHEMA + ex.Message);
            }
        }
        // Get Parameters
        public IList<ConfigParameterDefinition> GetConfigParameters(KeyedCollection<string, ConfigParameter> _configParameters, ConfigParameterPage _page)
        {
            utils.Logger(TraceEventType.Information,
                         ConstDefinition.ID0200_START_GETCONFIGPARAMETERS,
                         ConstDefinition.MSG0200_START_GETCONFIGPARAMETERS);
            var _configParametersDefinitions = new List<ConfigParameterDefinition>();
            try
            {
                switch (_page)
                {
                    case ConfigParameterPage.Capabilities:
                        break;
                    case ConfigParameterPage.Connectivity:
                        _configParametersDefinitions.Add(ConfigParameterDefinition.CreateLabelParameter("Plugin Library Configuration"));
                        _configParametersDefinitions.Add(ConfigParameterDefinition.CreateStringParameter(ConstDefinition.CFG_PLUGIN_FILENAME, ""));
                        _configParametersDefinitions.Add(ConfigParameterDefinition.CreateDividerParameter());
                        _configParametersDefinitions.Add(ConfigParameterDefinition.CreateLabelParameter("OAuth2.0 JWT Parameters Configuration"));
                        _configParametersDefinitions.Add(ConfigParameterDefinition.CreateStringParameter(ConstDefinition.CFG_JWT_SUB, ""));
                        _configParametersDefinitions.Add(ConfigParameterDefinition.CreateStringParameter(ConstDefinition.CFG_JWT_ISS, ""));
                        _configParametersDefinitions.Add(ConfigParameterDefinition.CreateStringParameter(ConstDefinition.CFG_JWT_KEY_FILENAME, ""));
                        _configParametersDefinitions.Add(ConfigParameterDefinition.CreateEncryptedStringParameter(ConstDefinition.CFG_JWT_KEY_PASSWORD, ""));
                        _configParametersDefinitions.Add(ConfigParameterDefinition.CreateDividerParameter());
                        _configParametersDefinitions.Add(ConfigParameterDefinition.CreateLabelParameter("Proxy Server Configuration"));
                        _configParametersDefinitions.Add(ConfigParameterDefinition.CreateStringParameter(ConstDefinition.CFG_PROXY_SERVER, ""));
                        _configParametersDefinitions.Add(ConfigParameterDefinition.CreateStringParameter(ConstDefinition.CFG_PROXY_USERNAME, ""));
                        _configParametersDefinitions.Add(ConfigParameterDefinition.CreateEncryptedStringParameter(ConstDefinition.CFG_PROXY_PASSWORD, ""));
                        break;
                    case ConfigParameterPage.Global:
                        break;
                    case ConfigParameterPage.Partition:
                        break;
                    case ConfigParameterPage.RunStep:
                        break;
                    case ConfigParameterPage.Schema:
                        break;
                }
            }
            catch (Exception ex)
            {
                utils.Logger(TraceEventType.Error,
                             ConstDefinition.ID0299_ERROR_GETCONFIGPARAMETERS,
                             ConstDefinition.MSG0299_ERROR_GETCONFIGPARAMETERS + ex.Message);
                throw new ExtensibleExtensionException(ConstDefinition.MSG0299_ERROR_GETCONFIGPARAMETERS + ex.Message);
            }
            return _configParametersDefinitions;
        }
        // Validate Parameters
        public ParameterValidationResult ValidateConfigParameters(KeyedCollection<string, ConfigParameter> _configParameters, ConfigParameterPage _page)
        {
            utils.Logger(TraceEventType.Information,
                         ConstDefinition.ID0300_START_VALIDATECONFIGPARAMETERS,
                         ConstDefinition.MSG0300_START_VALIDATECONFIGPARAMETERS);
            try
            {
                switch (_page)
                {
                    case ConfigParameterPage.Connectivity:
                        if (_configParameters[ConstDefinition.CFG_PLUGIN_FILENAME].Value.Length == 0)
                        {
                            return new ParameterValidationResult(
                                ParameterValidationResultCode.Failure,
                                ConstDefinition.CFG_PLUGIN_FILENAME + " must not be null",
                                ConstDefinition.CFG_PLUGIN_FILENAME);
                        }
                        if (_configParameters[ConstDefinition.CFG_JWT_SUB].Value.Length == 0)
                        {
                            return new ParameterValidationResult(
                                ParameterValidationResultCode.Failure,
                                ConstDefinition.CFG_JWT_SUB + " must not be null",
                                ConstDefinition.CFG_JWT_SUB);
                        }
                        if (_configParameters[ConstDefinition.CFG_JWT_ISS].Value.Length == 0)
                        {
                            return new ParameterValidationResult(
                                ParameterValidationResultCode.Failure,
                                ConstDefinition.CFG_JWT_ISS + " must not be null",
                                ConstDefinition.CFG_JWT_ISS);
                        }
                        if (_configParameters[ConstDefinition.CFG_JWT_KEY_FILENAME].Value.Length == 0)
                        {
                            return new ParameterValidationResult(
                                ParameterValidationResultCode.Failure,
                                ConstDefinition.CFG_JWT_KEY_FILENAME + " must not be null",
                                ConstDefinition.CFG_JWT_KEY_FILENAME);
                        }
                        if (utils.DecryptSecureString(_configParameters[ConstDefinition.CFG_JWT_KEY_PASSWORD].SecureValue).Length == 0)
                        {
                            return new ParameterValidationResult(
                                ParameterValidationResultCode.Failure,
                                ConstDefinition.CFG_JWT_KEY_PASSWORD + " must not be null",
                                ConstDefinition.CFG_JWT_KEY_PASSWORD);
                        }
                        // check proxy server settings
                        proxyServerType = utils.GetProxyUsageType(_configParameters);
                        switch (proxyServerType)
                        {
                            case Utils.ProxyServerType.NoProxy:
                                break;
                            case Utils.ProxyServerType.Proxy:
                                break;
                            case Utils.ProxyServerType.ProxyWithAuthN:
                                if (utils.DecryptSecureString(_configParameters[ConstDefinition.CFG_PROXY_PASSWORD].SecureValue).Length == 0)
                                {
                                    return new ParameterValidationResult(
                                        ParameterValidationResultCode.Failure,
                                        ConstDefinition.CFG_PROXY_PASSWORD + " must not be null",
                                        ConstDefinition.CFG_PROXY_PASSWORD);
                                }
                                break;
                        }
                        break;
                    case ConfigParameterPage.Global:
                        break;
                    case ConfigParameterPage.Partition:
                        break;
                    case ConfigParameterPage.RunStep:
                        break;
                    case ConfigParameterPage.Schema:
                        break;
                }
                return new ParameterValidationResult();
            }
            catch (Exception ex)
            {
                utils.Logger(TraceEventType.Error,
                             ConstDefinition.ID0399_ERROR_VALIDATECONFIGPARAMETERS,
                             ConstDefinition.MSG0300_START_VALIDATECONFIGPARAMETERS + ex.Message);
                throw new ExtensibleExtensionException(ConstDefinition.MSG0399_ERROR_VALIDATECONFIGPARAMETERS + ex.Message);
            }
        }
        //
        // Import
        //
        // OpenImportConnection
        public OpenImportConnectionResults OpenImportConnection(
                                       KeyedCollection<string, ConfigParameter> _configParameters,
                                       Schema _types,
                                       OpenImportConnectionRunStep _importRunStep)
        {
            utils.Logger(TraceEventType.Information,
                         ConstDefinition.ID0400_START_OPENIMPORTCONNECTION,
                         ConstDefinition.MSG0400_START_OPENIMPORTCONNECTION);
#if DEBUG
            utils.Logger(TraceEventType.Verbose, ConstDefinition.ID0450_VERBOSE_OPENIMPORTCONNECTION, _configParameters[ConstDefinition.CFG_JWT_SUB].Value.ToString());
            utils.Logger(TraceEventType.Verbose, ConstDefinition.ID0450_VERBOSE_OPENIMPORTCONNECTION, _configParameters[ConstDefinition.CFG_JWT_ISS].Value.ToString());
            utils.Logger(TraceEventType.Verbose, ConstDefinition.ID0450_VERBOSE_OPENIMPORTCONNECTION, _configParameters[ConstDefinition.CFG_JWT_KEY_FILENAME].Value.ToString());
            utils.Logger(TraceEventType.Verbose, ConstDefinition.ID0450_VERBOSE_OPENIMPORTCONNECTION, utils.DecryptSecureString(_configParameters[ConstDefinition.CFG_JWT_KEY_PASSWORD].SecureValue));
            utils.Logger(TraceEventType.Verbose, ConstDefinition.ID0450_VERBOSE_OPENIMPORTCONNECTION, _types.ObjectTypeAttributeName);
#endif
            try
            {
                // Get importObjectTypes
                targetObjectTypes = new List<string>();
                _fimSchemas = new Dictionary<string, KeyedCollection<string, SchemaAttribute>>();
                foreach (Microsoft.MetadirectoryServices.SchemaType _objectType in _types.Types)
                {
                    _fimSchemas.Add(_objectType.Name, _objectType.Attributes);
#if DEBUG
                    foreach (var attr in _objectType.Attributes)
                    {
                        utils.Logger(TraceEventType.Verbose, ConstDefinition.ID0450_VERBOSE_OPENIMPORTCONNECTION, _objectType.Name + ":" + attr.Name);
                    }
#endif
                }
                // Get OperationType
                operationType = _importRunStep.ImportType;
#if DEBUG
                switch (operationType)
                {
                    case OperationType.Full:
                        utils.Logger(TraceEventType.Verbose, ConstDefinition.ID0450_VERBOSE_OPENIMPORTCONNECTION, "OperationType : Full");
                        break;
                    case OperationType.Delta:
                        utils.Logger(TraceEventType.Verbose, ConstDefinition.ID0450_VERBOSE_OPENIMPORTCONNECTION, "OperationType : Delta");
                        break;
                    case OperationType.FullObject:
                        utils.Logger(TraceEventType.Verbose, ConstDefinition.ID0450_VERBOSE_OPENIMPORTCONNECTION, "OperationType : FullObject");
                        break;
                    default:
                        utils.Logger(TraceEventType.Verbose, ConstDefinition.ID0450_VERBOSE_OPENIMPORTCONNECTION, "OperationType : Other");
                        break;
                }
#endif
                // Get Domain Parameter from Sub
                domain = _configParameters[ConstDefinition.CFG_JWT_SUB].Value.ToString().Split('@')[1];
                // Get plugin library instance
                if (pluginInstance == null)
                {
                    utils.Logger(TraceEventType.Verbose,
                                 ConstDefinition.ID0450_VERBOSE_OPENIMPORTCONNECTION,
                                 ConstDefinition.MSG0450_VERBOSE_OPENIMPORTCONNECTION + "Getting plugin library instance");
                    pluginInstance = utils.GetPluginLibraryInstance(_configParameters);
                }
                // Get Token End Point
                string _tokenEndPoint = pluginInstance.GetEndPoints("Token");
                // Initialize PagingToken
                pagingToken = null;
                importPagesize = _importRunStep.PageSize;

                // Obtain Access Token
                proxyServerType = utils.GetProxyUsageType(_configParameters);
                webProxy = utils.GetProxy(_configParameters);
                accessToken = utils.GetAccessToken(pluginInstance, _configParameters, webProxy);
                utils.Logger(TraceEventType.Verbose,
                             ConstDefinition.ID0450_VERBOSE_OPENIMPORTCONNECTION,
                             ConstDefinition.MSG0450_VERBOSE_OPENIMPORTCONNECTION + "obtain access token : " + accessToken);
                return new OpenImportConnectionResults();
            }
            catch (Exception ex)
            {
                utils.Logger(TraceEventType.Error,
                             ConstDefinition.ID0499_ERROR_OPENIMPORTCONNECTION,
                             ex.Message);
                throw new ExtensibleExtensionException(ConstDefinition.MSG0499_ERROR_OPENIMPORTCONNECTION + ex.Message);
            }
        }
        // GetImportEntries
        public GetImportEntriesResults GetImportEntries(GetImportEntriesRunStep _importRunStep)
        {
            utils.Logger(TraceEventType.Information,
                         ConstDefinition.ID0500_START_GETIMPORTENTRIES,
                         ConstDefinition.MSG0500_START_GETIMPORTENTRIES);
            try
            {
                string _pagingTokenParam = pluginInstance.PagingTokenParameter;
                string _pagingTokenAttribute = pluginInstance.PagingTokenAttribute;
                var _csentries = new List<CSEntryChange>();

                foreach (KeyValuePair<string, KeyedCollection<string, SchemaAttribute>> _objectTypeInfo in _fimSchemas)
                {
                    string _endPoint = pluginInstance.GetEndPoints(_objectTypeInfo.Key, domain);
                    string _importEntriesJSON = null;
                    if(pagingToken != null){
                        _importEntriesJSON = utils.GetContentsWithAccessToken(_endPoint + importPagesize + "&" + _pagingTokenParam + "=" + pagingToken, accessToken, webProxy);
                    }else{
                        _importEntriesJSON = utils.GetContentsWithAccessToken(_endPoint + importPagesize, accessToken, webProxy);
                    }
                    // obtain pagingToken
                    var _getPagingTokenResultJson = JToken.Parse(_importEntriesJSON);
                    if (_getPagingTokenResultJson[_pagingTokenAttribute] != null)
                    {
                        // store pagingToken for next page
                        pagingToken = _getPagingTokenResultJson[_pagingTokenAttribute].ToString();
                        utils.Logger(TraceEventType.Verbose,
                                     ConstDefinition.ID0550_VERBOSE_GETIMPORTENTRIES,
                                     ConstDefinition.MSG0550_VERBOSE_GETIMPORTENTRIES + "pagingToken : " + pagingToken);
                    }
                    else
                    {
                        pagingToken = null;
                    }
                    // retrieve entries
                    var _getImportEntriesByObjectTypeResult = JObject.Parse(_importEntriesJSON).SelectToken(_objectTypeInfo.Key).ToString();
                    var _importObjectJSONArray = JArray.Parse(_getImportEntriesByObjectTypeResult);
                    foreach (var _importObjectJSON in _importObjectJSONArray)
                    {
                        var _csentryChange = CSEntryChange.Create();
                        _csentryChange.ObjectModificationType = ObjectModificationType.Add;
                        _csentryChange.ObjectType = _objectTypeInfo.Key;
#if DEBUG
                        utils.Logger(TraceEventType.Verbose,
                                     ConstDefinition.ID0550_VERBOSE_GETIMPORTENTRIES,
                                     ConstDefinition.MSG0550_VERBOSE_GETIMPORTENTRIES + "objectType : " + _objectTypeInfo.Key);
#endif
                        foreach (var _attribute in _objectTypeInfo.Value)
                        {
                            if (_attribute.AllowedAttributeOperation != AttributeOperation.ExportOnly)
                            {
                                // check attribute has a parent atttribute
                                if (!_attribute.Name.Contains("__"))
                                {
                                    // no parent attribute
                                    // check target attribute in JSON
                                    if (_importObjectJSON[_attribute.Name] != null)
                                    {
                                        if (_attribute.IsMultiValued)
                                        {
                                            // multi value attribute
                                            bool _hasValue = false;
                                            var _valueJSON = JArray.Parse(_importObjectJSON[_attribute.Name].ToString());
                                            var _valueList = new List<object>();

                                            foreach (var _value in _valueJSON)
                                            {
                                                _valueList.Add(_value.ToString());
                                                _hasValue = true;
                                            }
                                            if (_hasValue)
                                            {
                                                utils.AddCSEntryAttribute(_attribute.DataType, _csentryChange, _attribute.Name, _valueList);
                                            }
                                        }
                                        else
                                        {
                                            // single value attribute
                                            utils.AddCSEntryAttribute(_attribute.DataType, _csentryChange, _attribute.Name, _importObjectJSON[_attribute.Name]);
                                        }
                                    }
                                }
                                else
                                {
                                    // have parent attribute
                                    // get parent attribute name
                                    var _parentAttribute = _attribute.Name.Substring(0, _attribute.Name.IndexOf("__"));
                                    // check for parent attrbibute in JSON
                                    if (_importObjectJSON[_parentAttribute] != null)
                                    {
                                        // get child atttribute name
                                        var _childAttrbuteName = _attribute.Name.Substring(_attribute.Name.IndexOf("__") + 2);
                                        if (_attribute.IsMultiValued)
                                        {
                                            // multi value attribute
                                            bool _hasValue = false;
                                            var _childObjArr = JArray.Parse(_importObjectJSON[_parentAttribute].ToString());
                                            var _valueList = new List<object>();

                                            foreach (var _childObj in _childObjArr)
                                            {
                                                if (_childObj[_childAttrbuteName] != null)
                                                {
                                                    _valueList.Add(_childObj[_childAttrbuteName].ToString());
                                                    _hasValue = true;
                                                }
                                            }
                                            if (_hasValue)
                                            {
                                                utils.AddCSEntryAttribute(_attribute.DataType, _csentryChange, _attribute.Name, _valueList);
                                            }
                                        }
                                        else
                                        {
                                            // single value attribute
                                            var _childObj = JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JContainer>(_importObjectJSON[_parentAttribute].ToString());
                                            // check for child attrbibute in JSON
                                            if (_childObj[_childAttrbuteName] != null)
                                            {
                                                utils.AddCSEntryAttribute(_attribute.DataType, _csentryChange, _attribute.Name, _childObj[_childAttrbuteName]);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        _csentries.Add(_csentryChange);
                    }
                }
                var _importReturnInfo = new GetImportEntriesResults();
                if (pagingToken != null)
                {
                    _importReturnInfo.MoreToImport = true;
                    utils.Logger(TraceEventType.Verbose,
                                 ConstDefinition.ID0550_VERBOSE_GETIMPORTENTRIES,
                                 ConstDefinition.MSG0550_VERBOSE_GETIMPORTENTRIES + "More objects to import");
                }
                else
                {
                    _importReturnInfo.MoreToImport = false;
                    utils.Logger(TraceEventType.Verbose,
                                 ConstDefinition.ID0550_VERBOSE_GETIMPORTENTRIES,
                                 ConstDefinition.MSG0550_VERBOSE_GETIMPORTENTRIES + "No more objects to import");
                }
                _importReturnInfo.CSEntries = _csentries;
                return _importReturnInfo;
            }
            catch (Exception ex)
            {
                utils.Logger(TraceEventType.Error,
                             ConstDefinition.ID0599_ERROR_GETIMPORTENTRIES,
                             ConstDefinition.MSG0599_ERROR_GETIMPORTENTRIES + ex.Message);
                throw new ExtensibleExtensionException(ConstDefinition.MSG0599_ERROR_GETIMPORTENTRIES + ex.Message);
            }
        }
        // CloseImportConnection
        public CloseImportConnectionResults CloseImportConnection(CloseImportConnectionRunStep _importRunStepInfo)
        {
            utils.Logger(TraceEventType.Information,
                         ConstDefinition.ID0600_START_CLOSEIMPORTCONNECTION,
                         ConstDefinition.MSG0600_START_CLOSEIMPORTCONNECTION);
            return new CloseImportConnectionResults();
        }
        // other
        public int ImportMaxPageSize
        {
            get
            {
                return ConstDefinition.CFG_IMPORT_MAX_PAGE_SIZE;
            }
        }
        public int ImportDefaultPageSize
        {
            get
            {
                return ConstDefinition.CFG_IMPORT_DEFAULT_PAGE_SIZE;
            }
        }
        //
        // Export
        //
        // OpenExportConnection
        public void OpenExportConnection(KeyedCollection<string, ConfigParameter> _configParameters,
                                         Schema _types,
                                         OpenExportConnectionRunStep _exportRunStep)
        {
            utils.Logger(TraceEventType.Information,
                         ConstDefinition.ID0700_START_OPENEXPORTCONNECTION,
                         ConstDefinition.MSG0700_START_OPENEXPORTCONNECTION);
#if DEBUG
            utils.Logger(TraceEventType.Verbose, ConstDefinition.ID0750_VERBOSE_OPENEXPORTCONNECTION, _configParameters[ConstDefinition.CFG_JWT_SUB].Value.ToString());
            utils.Logger(TraceEventType.Verbose, ConstDefinition.ID0750_VERBOSE_OPENEXPORTCONNECTION, _configParameters[ConstDefinition.CFG_JWT_ISS].Value.ToString());
            utils.Logger(TraceEventType.Verbose, ConstDefinition.ID0750_VERBOSE_OPENEXPORTCONNECTION, _configParameters[ConstDefinition.CFG_JWT_KEY_FILENAME].Value.ToString());
            utils.Logger(TraceEventType.Verbose, ConstDefinition.ID0750_VERBOSE_OPENEXPORTCONNECTION, utils.DecryptSecureString(_configParameters[ConstDefinition.CFG_JWT_KEY_PASSWORD].SecureValue));
            utils.Logger(TraceEventType.Verbose, ConstDefinition.ID0750_VERBOSE_OPENEXPORTCONNECTION, _types.ObjectTypeAttributeName);
#endif
            try
            {
                // Get ExportObjectTypes
                targetObjectTypes = new List<string>();
                _fimSchemas = new Dictionary<string, KeyedCollection<string, SchemaAttribute>>();
                foreach (Microsoft.MetadirectoryServices.SchemaType _objectType in _types.Types)
                {
                    _fimSchemas.Add(_objectType.Name, _objectType.Attributes);
#if DEBUG
                    foreach (var _attr in _objectType.Attributes)
                    {
                        utils.Logger(TraceEventType.Verbose, ConstDefinition.ID0450_VERBOSE_OPENIMPORTCONNECTION, _objectType.Name + ":" + _attr.Name);
                    }
#endif
                }
                // Get plugin library instance
                if (pluginInstance == null)
                {
                    pluginInstance = utils.GetPluginLibraryInstance(_configParameters);
                    utils.Logger(TraceEventType.Verbose,
                                 ConstDefinition.ID0750_VERBOSE_OPENEXPORTCONNECTION,
                                 ConstDefinition.MSG0750_VERBOSE_OPENEXPORTCONNECTION + "Getting plugin library instance");
                }
                // Get Token End Point
                string _tokenEndPoint = pluginInstance.GetEndPoints(pluginInstance.ENDPOINTTYPE_TOKEN);

                // Obtain Access Token
                proxyServerType = utils.GetProxyUsageType(_configParameters);
                webProxy = utils.GetProxy(_configParameters);
                accessToken = utils.GetAccessToken(pluginInstance, _configParameters, webProxy);
                utils.Logger(TraceEventType.Verbose,
                             ConstDefinition.ID0750_VERBOSE_OPENEXPORTCONNECTION,
                             ConstDefinition.MSG0750_VERBOSE_OPENEXPORTCONNECTION + "obtain access token : " + accessToken);
            }
            catch (Exception ex)
            {
                utils.Logger(TraceEventType.Error,
                             ConstDefinition.ID0799_ERROR_OPENEXPORTCONNECTION,
                             ConstDefinition.MSG0799_ERROR_OPENEXPORTCONNECTION + ex.Message);
                throw new ExtensibleExtensionException(ConstDefinition.MSG0799_ERROR_OPENEXPORTCONNECTION + ex.Message);
            }
        }
        // PutExportEntries
        public PutExportEntriesResults PutExportEntries(IList<CSEntryChange> _csentries)
        {
            utils.Logger(TraceEventType.Information,
                         ConstDefinition.ID0800_START_PUTEXPORTENTRIES,
                         ConstDefinition.MSG0800_START_PUTEXPORTENTRIES);
            PutExportEntriesResults _exportEntriesResults = new PutExportEntriesResults();
            try
            {
                foreach (CSEntryChange _csentryChange in _csentries)
                {
#if DEBUG
                    utils.Logger(TraceEventType.Verbose, ConstDefinition.ID0850_VERBOSE_PUTEXPORTENTRIES, _csentryChange.DN.ToString());
#endif
                    foreach (KeyValuePair<string, KeyedCollection<string, SchemaAttribute>> _objectTypeInfo in _fimSchemas)
                    {
                        // get endpoint for each object types
                        string _endPoint = pluginInstance.GetEndPoints(_objectTypeInfo.Key);
                        // build json to POST
                        var _attr = new Dictionary<string, string>();
                        // anchor attribute
                        _attr.Add(pluginInstance.GetAnchor(_objectTypeInfo.Key), _csentryChange.DN.ToString());
                        switch (_csentryChange.ObjectModificationType)
                        {
                            case ObjectModificationType.Add:
                            case ObjectModificationType.Update:
#if DEBUG
                                utils.Logger(TraceEventType.Verbose, ConstDefinition.ID0850_VERBOSE_PUTEXPORTENTRIES, "ObjectModificationType : Add / Update");
#endif
                                foreach (string _attribName in _csentryChange.ChangedAttributeNames)
                                {
                                    var _attributeChange = _csentryChange.AttributeChanges[_attribName];
                                    var _valueChanges = _attributeChange.ValueChanges;
                                    if (_valueChanges != null)
                                    {
                                        foreach (var _valueChange in _valueChanges)
                                        {
                                            if (_valueChange.ModificationType == ValueModificationType.Add)
                                            {
                                                // new value
#if DEBUG
                                                utils.Logger(TraceEventType.Verbose, ConstDefinition.ID0850_VERBOSE_PUTEXPORTENTRIES, _attribName + " : " + _valueChange.Value.ToString());
#endif
                                                _attr.Add(_attribName, _valueChange.Value.ToString());
                                                break;
                                            }
                                        }
                                    }
                                }
                                // build json
                                string _exportDataJSON = pluginInstance.GetJSONObject(_objectTypeInfo.Key, _attr);
#if DEBUG
                                utils.Logger(TraceEventType.Verbose, ConstDefinition.ID0850_VERBOSE_PUTEXPORTENTRIES, _exportDataJSON);
#endif
                                string _exportResult = null;
                                switch (_csentryChange.ObjectModificationType)
                                {
                                    case ObjectModificationType.Add:
                                        // POST
                                        // catch exception per entry and go on exporting
                                        try
                                        {
                                            _exportResult = utils.PostContentsWithAccessToken(_endPoint, accessToken, _exportDataJSON, webProxy);
#if DEBUG
                                            utils.Logger(TraceEventType.Verbose, ConstDefinition.ID0850_VERBOSE_PUTEXPORTENTRIES, _exportResult);
#endif
                                            // success
                                            _exportEntriesResults.CSEntryChangeResults.Add(
                                                CSEntryChangeResult.Create(_csentryChange.Identifier,
                                                                           _csentryChange.AttributeChanges,
                                                                           MAExportError.Success));
                                        }
                                        catch (Exception ex)
                                        {
                                            // error
                                            utils.Logger(TraceEventType.Error,
                                                         ConstDefinition.ID0899_ERROR_PUTEXPORTENTRIES,
                                                         ConstDefinition.MSG0899_ERROR_PUTEXPORTENTRIES + ex.Message);
                                            // To do : change MAExportError value per ex.HResult
                                            _exportEntriesResults.CSEntryChangeResults.Add(
                                                CSEntryChangeResult.Create(_csentryChange.Identifier,
                                                                           _csentryChange.AttributeChanges,
                                                                           MAExportError.ExportErrorDuplicateAnchor,
                                                                           ex.Message,
                                                                           ex.InnerException.Message
                                                                           ));
                                        }
                                        break;
                                    case ObjectModificationType.Update:
                                        // PUT endpoint + "/" + dn
                                        // catch exception per entry and go on exporting
                                        try
                                        {
                                            // success
                                            _exportResult = utils.PutContentsWithAccessToken(_endPoint + "/" + _csentryChange.DN.ToString(), accessToken, _exportDataJSON, webProxy);
#if DEBUG
                                            utils.Logger(TraceEventType.Verbose, ConstDefinition.ID0850_VERBOSE_PUTEXPORTENTRIES, _exportResult);
#endif
                                            _exportEntriesResults.CSEntryChangeResults.Add(
                                                CSEntryChangeResult.Create(_csentryChange.Identifier,
                                                                           _csentryChange.AttributeChanges,
                                                                           MAExportError.Success));
                                        }
                                        catch (Exception ex)
                                        {
                                            // error
                                            utils.Logger(TraceEventType.Error,
                                                         ConstDefinition.ID0899_ERROR_PUTEXPORTENTRIES,
                                                         ConstDefinition.MSG0899_ERROR_PUTEXPORTENTRIES + ex.Message);
                                            // To do : change MAExportError value per ex.HResult
                                            _exportEntriesResults.CSEntryChangeResults.Add(
                                                CSEntryChangeResult.Create(_csentryChange.Identifier,
                                                                           _csentryChange.AttributeChanges,
                                                                           MAExportError.ExportErrorDuplicateAnchor,
                                                                           ex.Message,
                                                                           ex.InnerException.Message
                                                                           ));
                                        }
                                        break;
                                }
                                break;
                            case ObjectModificationType.Delete:
                                // DELETE endpoint + "/" + dn
                                // catch exception per entry and go on exporting
                                try
                                {
                                    _exportResult = utils.DeleteContentsWithAccessToken(_endPoint + "/" + _csentryChange.DN.ToString(), accessToken, webProxy);
#if DEBUG
                                            utils.Logger(TraceEventType.Verbose, ConstDefinition.ID0850_VERBOSE_PUTEXPORTENTRIES, _res);
#endif
                                    // success
                                    _exportEntriesResults.CSEntryChangeResults.Add(
                                        CSEntryChangeResult.Create(_csentryChange.Identifier,
                                                                   _csentryChange.AttributeChanges,
                                                                   MAExportError.Success));
                                }
                                catch (Exception ex)
                                {
                                    // error
                                    utils.Logger(TraceEventType.Error,
                                                 ConstDefinition.ID0899_ERROR_PUTEXPORTENTRIES,
                                                 ConstDefinition.MSG0899_ERROR_PUTEXPORTENTRIES + ex.Message);
                                    // To do : change MAExportError value per ex.HResult
                                    _exportEntriesResults.CSEntryChangeResults.Add(
                                        CSEntryChangeResult.Create(_csentryChange.Identifier,
                                                                   _csentryChange.AttributeChanges,
                                                                   MAExportError.ExportErrorConnectedDirectoryError,
                                                                   ex.Message,
                                                                   ex.InnerException.Message
                                                                   ));
                                }
                                break;
                            default:
                                // error
                                utils.Logger(TraceEventType.Error,
                                             ConstDefinition.ID0899_ERROR_PUTEXPORTENTRIES,
                                             ConstDefinition.MSG0899_ERROR_PUTEXPORTENTRIES + "Unknown Operation Type : " + _csentryChange.ObjectModificationType);
                                _exportEntriesResults.CSEntryChangeResults.Add(
                                    CSEntryChangeResult.Create(_csentryChange.Identifier,
                                                                _csentryChange.AttributeChanges,
                                                                MAExportError.ExportErrorConnectedDirectoryError,
                                                                "Operation Error",
                                                                "Unknown Operation Type : " + _csentryChange.ObjectModificationType
                                                                ));
                                break;
                        }
                    }
        		}
                return _exportEntriesResults;
            }
            catch (Exception ex)
            {
                utils.Logger(TraceEventType.Error,
                             ConstDefinition.ID0899_ERROR_PUTEXPORTENTRIES,
                             ConstDefinition.MSG0899_ERROR_PUTEXPORTENTRIES + ex.Message);
                throw new ExtensibleExtensionException(ex.Message);
            }
        }
        // CloseExportConnection
        public void CloseExportConnection(CloseExportConnectionRunStep _exportRunStep)
        {
            utils.Logger(TraceEventType.Information,
                         ConstDefinition.ID0900_START_CLOSEEXPORTCONNECTION,
                         ConstDefinition.MSG0900_START_CLOSEEXPORTCONNECTION);
        }
        public int ExportDefaultPageSize
        {
            get
            {
                return ConstDefinition.CFG_EXPORT_DEFAULT_PAGE_SIZE;
            }
        }
        public int ExportMaxPageSize
        {
            get
            {
                return ConstDefinition.CFG_EXPORT_MAX_PAGE_SIZE;
            }
        }
        // Password Management
        public void OpenPasswordConnection(KeyedCollection<string, ConfigParameter> _configParameters, Partition _partition)
        {
            utils.Logger(TraceEventType.Information,
                         ConstDefinition.ID1000_START_OPENPASSWORDCONNECTION,
                         ConstDefinition.MSG1000_START_OPENPASSWORDCONNECTION);
            try
            {
                // Get plugin library instance
                if (pluginInstance == null)
                {
                    pluginInstance = utils.GetPluginLibraryInstance(_configParameters);
                }
                // Get Token End Point
                string _tokenEndPoint = pluginInstance.GetEndPoints(pluginInstance.ENDPOINTTYPE_TOKEN);
                // Get Proxy Server Settings
                webProxy = utils.GetProxy(_configParameters);
                // Obtain Access Token
                proxyServerType = utils.GetProxyUsageType(_configParameters);
                accessToken = utils.GetAccessToken(pluginInstance, _configParameters, webProxy);
                utils.Logger(TraceEventType.Verbose, 0, "obtain access token : " + accessToken);
            }
            catch (Exception ex)
            {
                utils.Logger(TraceEventType.Error,
                             ConstDefinition.ID1099_ERROR_OPENPASSWORDCONNECTION,
                             ConstDefinition.MSG1099_ERROR_OPENPASSWORDCONNECTION + ex.Message);
                throw new ExtensibleExtensionException(ConstDefinition.MSG1099_ERROR_OPENPASSWORDCONNECTION + ex.Message);
            }
        }
        public void ClosePasswordConnection()
        {
            utils.Logger(TraceEventType.Information,
                         ConstDefinition.ID1100_START_CLOSEPASSWORDCONNECTION,
                         ConstDefinition.MSG1100_START_CLOSEPASSWORDCONNECTION);
        }
        public ConnectionSecurityLevel GetConnectionSecurityLevel()
        {
            return ConnectionSecurityLevel.Secure;
        }
        private void changePassword(CSEntry _csentry, System.Security.SecureString _newPassword)
        {
            string _changeResult = null;
            try
            {
                // get endpoint for each object types
                string _endPoint = pluginInstance.GetEndPoints(pluginInstance.OBJECTTYPE_USER);
                // build json to POST
                var _attr = new Dictionary<string, string>();
                _attr.Add(pluginInstance.PasswordAttribute, utils.DecryptSecureString(_newPassword));
                // build json
                string _putDataJSON = pluginInstance.GetJSONObject(pluginInstance.OBJECTTYPE_USER, _attr);
                _changeResult = utils.PutContentsWithAccessToken(_endPoint + "/" + _csentry.DN.ToString(), accessToken, _putDataJSON, webProxy);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public void SetPassword(CSEntry _csentry, System.Security.SecureString _newPassword, PasswordOptions _options)
        {
            utils.Logger(TraceEventType.Information,
                         ConstDefinition.ID1200_START_SETPASSWORD,
                         ConstDefinition.MSG1200_START_SETPASSWORD);
            try
            {
                changePassword(_csentry, _newPassword);
            }
            catch (Exception ex)
            {
                utils.Logger(TraceEventType.Error,
                             ConstDefinition.ID1299_ERROR_SETPASSWORD,
                             ConstDefinition.MSG1299_ERROR_SETPASSWORD + ex.Message);
                throw new ExtensibleExtensionException(ConstDefinition.MSG1299_ERROR_SETPASSWORD + ex.Message);
            }
        }
        public void ChangePassword(CSEntry _csentry, System.Security.SecureString _oldPassword, System.Security.SecureString _newPassword)
        {
            utils.Logger(TraceEventType.Information,
                         ConstDefinition.ID1300_START_CHANGEPASSWORD,
                         ConstDefinition.MSG1300_START_CHANGEPASSWORD);
            try
            {
                changePassword(_csentry, _newPassword);
            }
            catch (Exception ex)
            {
                utils.Logger(TraceEventType.Error,
                             ConstDefinition.ID1399_ERROR_CHANGEPASSWORD,
                             ConstDefinition.MSG1399_ERROR_CHANGEPASSWORD + ex.Message);
                throw new ExtensibleExtensionException(ConstDefinition.MSG1399_ERROR_CHANGEPASSWORD + ex.Message);
            }
        }
        public void RequireChangePasswordOnNextLogin(CSEntry _csentry,bool _fRequireChangePasswordOnNextLogin)
        {
            utils.Logger(TraceEventType.Information,
                         ConstDefinition.ID1400_START_REQUIRECHANGEPASSWORDONNEXTLOGIN,
                         ConstDefinition.MSG1400_START_REQUIRECHANGEPASSWORDONNEXTLOGIN);
            // To do : implement
            throw new ExtensibleExtensionException(ConstDefinition.MSG1499_ERROR_REQUIRECHANGEPASSWORDONNEXTLOGIN + "Not Implemented");
        }
    };
}
