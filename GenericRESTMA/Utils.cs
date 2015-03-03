#define TRACE
using Microsoft.MetadirectoryServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;


namespace FimSync_Ezma
{
    class Utils
    {
        TraceSource traceSource = new TraceSource("Generic REST MA", SourceLevels.All);

        // Proxy Server Type
        public enum ProxyServerType
        {
            NoProxy,
            Proxy,
            ProxyWithAuthN
        }

        //
        // common utilities
        //
        // logging
        public void Logger(TraceEventType _traceEventType, int _id, string _message)
        {
            traceSource.TraceEvent(_traceEventType, _id, _message);
            traceSource.Flush();
        }
        // Base64URL Encode
        private string base64UrlEncode(byte[] _input)
        {
            var _output = Convert.ToBase64String(_input);
            _output = _output.Split('=')[0]; // Remove any trailing '='s
            _output = _output.Replace('+', '-'); // 62nd char of encoding
            _output = _output.Replace('/', '_'); // 63rd char of encoding
            return _output;
        }
        // descrypt secure string
        public string DecryptSecureString(SecureString _input)
        {
            IntPtr _pointer = Marshal.SecureStringToBSTR(_input);
            string _output = Marshal.PtrToStringUni(_pointer);
            return _output;
        }

        //
        // utilities for HTTP connection
        //
        // Create HTTP Client
        private HttpClient createHttpClient(WebProxy _webProxy)
        {
            HttpClient _httpClient;
            try
            {
                if (_webProxy != null)
                {
                    var _httpClientHandler = new HttpClientHandler();
                    _httpClientHandler.Proxy = _webProxy;
                    _httpClientHandler.UseProxy = true;
                    _httpClient = new HttpClient(_httpClientHandler);
                }
                else
                {
                    _httpClient = new HttpClient();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            return _httpClient;
        }
        // HTTP Async POST
        private async Task<string> postContents(string _url, Dictionary<string, string> _postData, WebProxy _webProxy)
        {
            try
            {
                var _httpClient = createHttpClient(_webProxy);
                var _content = new FormUrlEncodedContent(_postData);
                var _response = await _httpClient.PostAsync(_url, _content);
                return await _response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        // HTTP Async POST with Access Token
        private async Task<string> postContentsWithAccessToken(string _url, string _accessToken, string _postData, WebProxy _webProxy)
        {
            var _httpClient = createHttpClient(_webProxy);
            try
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _accessToken);
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var _stringContent = new StringContent(_postData, System.Text.Encoding.UTF8, "application/json");
                var _response = await _httpClient.PostAsync(_url, _stringContent);
                return await _response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        // HTTP Async PUT with Access Token
        private async Task<string> putContentsWithAccessToken(string _url, string _accessToken, string _putData, WebProxy _webProxy)
        {
            var _httpClient = createHttpClient(_webProxy);
            try
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _accessToken);
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var _stringContent = new StringContent(_putData, System.Text.Encoding.UTF8, "application/json");
                var _response = await _httpClient.PutAsync(_url, _stringContent);
                return await _response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        // HTTP Async DELETE with AccessToken
        private async Task<string> deleteContentsWithAccessToken(string _uri, string _accessToken, WebProxy _webProxy)
        {
            try
            {
                var _httpClient = createHttpClient(_webProxy);
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _accessToken);
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var _response = await _httpClient.DeleteAsync(_uri);
                return await _response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        // HTTP Async GET with Access Token
        private async Task<string> getContentsWithAccessToken(string _url, string _accessToken, WebProxy _webProxy)
        {
            try
            {
                var _httpClient = createHttpClient(_webProxy);
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _accessToken);
                return await _httpClient.GetStringAsync(_url);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        // HTTP POST with Access Token
        public string PostContentsWithAccessToken(string _url, string _accessToken, string _postData, WebProxy _webProxy)
        {
            string _result = null;
            try
            {
                Task _task = Task.Factory.StartNew(async () =>
                {
                    _result = await postContentsWithAccessToken(_url, _accessToken, _postData, _webProxy);
                }).Unwrap();
                _task.Wait();
                return _result;
            }
            catch (Exception ex)
            {

                throw new Exception("Exception in PostContentsWithAccessToken", ex);
            }
        }
        // HTTP PUT with Access Token
        public string PutContentsWithAccessToken(string _url, string _accessToken, string _putData, WebProxy _webProxy)
        {
            string _result = null;
            try
            {
                Task _task = Task.Factory.StartNew(async () =>
                {
                    _result = await putContentsWithAccessToken(_url, _accessToken, _putData, _webProxy);
                }).Unwrap();
                _task.Wait();
                return _result;
            }
            catch (Exception ex)
            {
                throw new Exception("Exception in PutContentsWithAccessToken", ex);
            }
        }
        // HTTP DELETE with Access Token
        public string DeleteContentsWithAccessToken(string _url, string _accessToken, WebProxy _webProxy)
        {
            string _result = null;
            try
            {
                Task _task = Task.Factory.StartNew(async () =>
                {
                    _result = await deleteContentsWithAccessToken(_url, _accessToken, _webProxy);
                }).Unwrap();
                _task.Wait();
                return _result;
            }
            catch (Exception ex)
            {
                throw new Exception("Exception in DeleteContentsWithAccessToken", ex);
            }
        }
        // HTTP GET with Access Token
        public string GetContentsWithAccessToken(string _url, string _accessToken, WebProxy _webProxy)
        {
            string _result = null;
            try
            {
                Task _task = Task.Factory.StartNew(async () =>
                {
                    _result = await getContentsWithAccessToken(_url, _accessToken, _webProxy);
                }).Unwrap();
                _task.Wait();
                return _result;
            }
            catch (Exception ex)
            {
                throw new Exception("Exception in GetContentsWithAccessToken", ex);
            }
        }

        //
        // utilities for OAuth2.0 AuthZ
        //
        // Get Access Token
        public string GetAccessToken(dynamic _pluginInstance, KeyedCollection<string, ConfigParameter> _fimConfigParameters, WebProxy _webProxy)
        {
            string _accessToken = null;

            try
            {
                string _tokenEndPoint = _pluginInstance.GetEndPoints("Token");
                var _subject = _fimConfigParameters[ConstDefinition.CFG_JWT_SUB].Value.ToString();
                var _issuer = _fimConfigParameters[ConstDefinition.CFG_JWT_ISS].Value.ToString();
                var _jwtKeyFileName = _fimConfigParameters[ConstDefinition.CFG_JWT_KEY_FILENAME].Value.ToString();
                var _jwtKeyPassword = DecryptSecureString(_fimConfigParameters[ConstDefinition.CFG_JWT_KEY_PASSWORD].SecureValue);

                // signing certificate
                var _certificate = new X509Certificate2(
                    _jwtKeyFileName,
                    _jwtKeyPassword,
                    X509KeyStorageFlags.Exportable);

                // token lifetime
                var _utc0 = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                var _issueTime = DateTime.Now;
                var _iat = (long)_issueTime.ToUniversalTime().Subtract(_utc0).TotalSeconds;
                var _exp = (long)_issueTime.ToUniversalTime().AddMinutes(55).Subtract(_utc0).TotalSeconds;

                // header
                var _header = new { alg = "RS256", typ = "JWT", x5t = _certificate.Thumbprint };
                var _headerSerialized = JsonConvert.SerializeObject(_header, Formatting.None);
                var _headerBytes = Encoding.UTF8.GetBytes(_headerSerialized);
                var _headerEncoded = base64UrlEncode(_headerBytes);

                // payload
                JObject _payload = _pluginInstance.GetOAuth2JWTPayload(
                    _subject,
                    _issuer,
                    _tokenEndPoint,
                    _exp,
                    _iat
                    );
                var _payloadSerialized = JsonConvert.SerializeObject(_payload, Formatting.None);
                var _payloadBytes = Encoding.UTF8.GetBytes(_payloadSerialized);
                var _payloadEncoded = base64UrlEncode(_payloadBytes);

                // signature
                var _rsaParameters = ((RSACryptoServiceProvider)(_certificate.PrivateKey)).ExportParameters(true);
                var _rsa = new RSACryptoServiceProvider(_certificate.PrivateKey.KeySize);
                _rsa.ImportParameters(_rsaParameters);
                Byte[] _target = Encoding.UTF8.GetBytes(_headerEncoded + "." + _payloadEncoded);
                byte[] _signBytes = _rsa.SignData(_target, new SHA256Managed());
                var _signEncoded = base64UrlEncode(_signBytes);

                // build JWT request
                string _req_jwt = _headerEncoded + "." + _payloadEncoded + "." + _signEncoded;

                // request access_token
                string _result = null;
                Task _task = Task.Factory.StartNew(async () =>
                {
                    _result = await postContents(
                        _tokenEndPoint,
                        new Dictionary<string, string>() { 
                        { "assertion" , _req_jwt },
                        { "grant_type", "urn:ietf:params:oauth:grant-type:jwt-bearer" }},
                            _webProxy);
                }).Unwrap();
                _task.Wait();

                // get access_token from result
                var _res = JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JContainer>(_result);
                _accessToken = _res["access_token"].ToString();

                return _accessToken;
            }
            catch (Exception ex)
            {
                throw new Exception("Exception in GetAccessToken", ex);
            }
        }

        //
        // utilities for WebProxy handling
        //
        // Get Proxy
        public WebProxy GetProxy(KeyedCollection<string, ConfigParameter> _fimConfigParameters)
        {
            WebProxy _webProxy = null;
            ProxyServerType _proxyServerType = ProxyServerType.NoProxy;

            try
            {
                _proxyServerType = GetProxyUsageType(_fimConfigParameters);
                switch (_proxyServerType)
                {
                    case ProxyServerType.NoProxy:
                        break;
                    case ProxyServerType.Proxy:
                        _webProxy = new WebProxy(_fimConfigParameters[ConstDefinition.CFG_PROXY_SERVER].Value.ToString());
                        break;
                    case ProxyServerType.ProxyWithAuthN:
                        _webProxy = new WebProxy(_fimConfigParameters[ConstDefinition.CFG_PROXY_SERVER].Value.ToString());
                        _webProxy.Credentials = new NetworkCredential(_fimConfigParameters[ConstDefinition.CFG_PROXY_USERNAME].Value.ToString(),
                                                                      DecryptSecureString(_fimConfigParameters[ConstDefinition.CFG_PROXY_PASSWORD].SecureValue));
                        break;
                    default:
                        throw new Exception("Unexcepted ProxyServerType Error in GetProxy : " + _proxyServerType.ToString());
                }
                return _webProxy;
            }
            catch (Exception ex)
            {
                throw new Exception("Exception in GetProxy", ex);
            }

        }
        // Check Proxy Usage Type
        public ProxyServerType GetProxyUsageType(KeyedCollection<string, ConfigParameter> _fimConfigParameters)
        {
            var _getProxyUsageType = ProxyServerType.NoProxy;

            try
            {
                // check proxy server settings
                if (_fimConfigParameters[ConstDefinition.CFG_PROXY_SERVER].Value.Length == 0)
                {
                    _getProxyUsageType = ProxyServerType.NoProxy;
                }
                else if (_fimConfigParameters[ConstDefinition.CFG_PROXY_USERNAME].Value.Length == 0)
                {
                    _getProxyUsageType = ProxyServerType.Proxy;
                }
                else
                {
                    _getProxyUsageType = ProxyServerType.ProxyWithAuthN;
                }
                return _getProxyUsageType;
            }
            catch (Exception ex)
            {
                throw new Exception("Exception in GetProxyUsageType", ex);
            }
        }

        //
        // utilities for FIM Interface
        //
        // create plugin library instance
        public dynamic GetPluginLibraryInstance(KeyedCollection<string, ConfigParameter> _fimConfigParameters)
        {
            try
            {
                var _asm = Assembly.LoadFrom(_fimConfigParameters[ConstDefinition.CFG_PLUGIN_FILENAME].Value.ToString());
                var _FIM_Interface = _asm.GetType("Plugin.FIM_Interface");
                return Activator.CreateInstance(_FIM_Interface);
            }
            catch (Exception ex)
            {
                throw new ExtensionException("Exception in GetPluginLibraryInstance", ex);
            }
        }
        // Add attribute to CS entry
        // for multi value
        public void AddCSEntryAttribute(AttributeType _attributeType, CSEntryChange _csentry, string _attributeName, List<object> _attributeValueList)
        {
            try
            {
                switch (_attributeType)
                {
                    case AttributeType.String:
                        _csentry.AttributeChanges.Add(AttributeChange.CreateAttributeAdd(_attributeName, _attributeValueList));
                        break;
                    default:
                        throw new ExtensionException("Unsupported attribute type for multiple value : " + _attributeType.GetTypeCode().ToString());
                }
            }
            catch (Exception ex)
            {
                throw new ExtensibleExtensionException("Exception in AddCSEntryAttribute", ex);
            }
        }
        // for single value
        public void AddCSEntryAttribute(AttributeType _attributeType, CSEntryChange _csentry, string _attributeName, object _attributeValue)
        {
            try
            {
                switch (_attributeType)
                {
                    case AttributeType.String:
                        _csentry.AttributeChanges.Add(AttributeChange.CreateAttributeAdd(_attributeName, _attributeValue.ToString()));
                        break;
                    case AttributeType.Boolean:
                        _csentry.AttributeChanges.Add(AttributeChange.CreateAttributeAdd(_attributeName, System.Convert.ToBoolean(_attributeValue.ToString())));
                        break;
                    case AttributeType.Integer:
                        _csentry.AttributeChanges.Add(AttributeChange.CreateAttributeAdd(_attributeName, System.Convert.ToInt64(_attributeValue.ToString())));
                        break;
                    default:
                        throw new ExtensionException("Unsupported attribute type : " + _attributeType.GetTypeCode().ToString());
                }
            }
            catch (Exception ex)
            {
                throw new ExtensibleExtensionException("Exception in AddCSEntryAttribute", ex);
            }
        }
    }
}
