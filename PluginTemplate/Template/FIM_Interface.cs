using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

//
// To do
// - support for multi value attribute
namespace Plugin
{
    public class FIM_Interface
    {
        // constants
        private const string TOKEN_ENDPOINT = "https://www.googleapis.com/oauth2/v3/token";
        private const string USER_ENDPOINT = "https://www.googleapis.com/admin/directory/v1/users";
        private const string GROUP_ENDPOINT = "https://www.googleapis.com/admin/directory/v1/groups";
        private const string SCOPE = "https://www.googleapis.com/auth/admin.directory.user https://www.googleapis.com/auth/admin.directory.group";
        public string OBJECTTYPE_USER = "users";
        public string OBJECTTYPE_GROUP = "groups";
        public string ENDPOINTTYPE_TOKEN = "Token";
        public string PasswordAttribute = "password";
        public string PagingTokenParameter = "pageToken";
        public string PagingTokenAttribute = "nextPageToken";

        // get anchor attribute name for object type
        private string getAnchor(PropertyInfo[] _propertyInfoArray)
        {
            string _getAnchor = null;
            foreach (var propertyInfo in _propertyInfoArray)
            {
                var attribute = propertyInfo.GetCustomAttribute(typeof(AnchorAttribute));
                if (attribute != null)
                {
                    if (((AnchorAttribute)attribute).isAnchor == true)
                    {
                        _getAnchor = propertyInfo.Name;
                        break;
                    }
                }
            }
            return _getAnchor;
        }
        public string GetAnchor(string objectType)
        {
            string _getAnchor = null;
            PropertyInfo[] _propertyInfoArray = null;

            switch (objectType)
            {
                case "users":
                    // User
                    var _users = new users();
                    _propertyInfoArray = _users.GetType().GetProperties();
                    _getAnchor = getAnchor(_propertyInfoArray);
                    break;
                case "groups":
                    // Group
                    var _groups = new groups();
                    _propertyInfoArray = _groups.GetType().GetProperties();
                    _getAnchor = getAnchor(_propertyInfoArray);
                    break;
                default:
                    break;
            }
            return _getAnchor;
        }
        // build JSON Object
        private string getJSONObject(PropertyInfo[] propertyInfoArray, Dictionary<string, string> attributes)
        {
            string _getJSONObject = null;
            var _obj = new Dictionary<string, object>();

            foreach (var _propertyInfo in propertyInfoArray)
            {
                foreach (KeyValuePair<string, string> _kvp in attributes)
                {
                    if (_propertyInfo.Name == _kvp.Key)
                    {
                        if(_kvp.Key.Contains("__"))
                        {
                            // nested attribute
                            var _parentAttributeName = _kvp.Key.Substring(0, _kvp.Key.IndexOf("__"));
                            var _childAttributeName = _kvp.Key.Substring(_kvp.Key.IndexOf("__") + 2);
                            if (_obj.ContainsKey(_parentAttributeName))
                            {
                                // already have parent attribute as key -> replace value
                                var _child = (Dictionary<string, object>)_obj[_parentAttributeName];
                                _child.Add(_childAttributeName, _kvp.Value);
                                _obj[_parentAttributeName] = _child;
                            }
                            else
                            {
                                var _child = new Dictionary<string, object>();
                                _child.Add(_childAttributeName, _kvp.Value);
                                _obj.Add(_parentAttributeName, _child);
                            }
                        }                        
                        else
                        {
                            // build jsonObject from string
                            // do not have parent
                            _obj.Add(_kvp.Key, _kvp.Value);
                        }
                        break;
                    }
                }
            }
            _getJSONObject = JsonConvert.SerializeObject(_obj);

            return _getJSONObject;
        }
        //
        public string GetJSONObject(string objectType, Dictionary<string, string> attributes)
        {
            string _jsonString = null ;
            PropertyInfo[] _propertyInfoArray = null;

            // map avp to objectClass
            switch (objectType)
            {
                case "users":
                    users _users = new users();
                    _propertyInfoArray = _users.GetType().GetProperties();
                    break;
                case "groups":
                    groups _groups = new groups();
                    _propertyInfoArray = _groups.GetType().GetProperties();
                    break;
                default:
                    break;
            }
            if (_propertyInfoArray != null)
            {
                _jsonString = getJSONObject(_propertyInfoArray, attributes);
            }
            return _jsonString;
        }
        // build OAuth2.0 JWT request payload
        public JObject GetOAuth2JWTPayload(string subject, string issuer, string endPoint, long exp, long iat)
        {
            var _payload = JObject.FromObject(new
            {
                sub = subject,
                iss = issuer,
                scope = SCOPE,
                aud = endPoint,
                exp = exp,
                iat = iat
            });
            return _payload;
        }
        //
        // Get EndPoints
        //
        public string GetEndPoints(string endPointType)
        {
            return GetEndPoints(endPointType, null);
        }
        public string GetEndPoints(string endPointType, string option)
        {
            string _getEndPoints = null;
            switch (endPointType)
            {
                case "users":
                    // must be same as json
                    if (option != null)
                    {
                        _getEndPoints = USER_ENDPOINT + "?domain=" + option + "&maxResults=";
                    }
                    else
                    {
                        _getEndPoints = USER_ENDPOINT;
                    }
                    break;
                case "groups":
                    if (option != null)
                    {
                        _getEndPoints = GROUP_ENDPOINT + "?domain=" + option + "&customer=my_customer&maxResults=";
                    }
                    else
                    {
                        _getEndPoints = GROUP_ENDPOINT;
                    }
                    break;
                case "Token":
                    _getEndPoints = TOKEN_ENDPOINT;
                    break;
                default:
                    break;
            }
            return _getEndPoints;
        }
        //
        //  Get Schema
        //  return value
        //      Dictionary<
        //          Key:Tuple<
        //              string  :   objectType
        //              string  :   attributeName
        //          >
        //          Value:Tuple<
        //              Type    :   attributeType
        //              bool    :   canRead
        //              bool    :   canWrite
        //              bool    :   isAnchor
        //          >
        //      >
        //
        private Dictionary<Tuple<string, string>, Tuple<Type, bool, bool, bool, bool>> getSchema(string objectType, PropertyInfo[] _propertyInfoArray)
        {
            var _getSchema = new Dictionary<Tuple<string, string>, Tuple<Type, bool, bool, bool, bool>>();
            foreach (var propertyInfo in _propertyInfoArray)
            {
                bool isAnchor = false;
                bool isMultiValue = false;
                var attribute = propertyInfo.GetCustomAttribute(typeof(AnchorAttribute));
                if (attribute != null)
                {
                    if (((AnchorAttribute)attribute).isAnchor == true)
                    {
                        isAnchor = true;
                    }
                }
                else
                {
                    attribute = propertyInfo.GetCustomAttribute(typeof(MultiValueAttribute));
                    if (attribute != null)
                    {
                        if(((MultiValueAttribute)attribute).isMultiValue == true)
                        {
                            isMultiValue = true;
                        }
                    }
                }
                
                Tuple<string, string> _key =
                    Tuple.Create(objectType, propertyInfo.Name);
                Tuple<Type, bool, bool, bool, bool> _prop =
                    Tuple.Create(propertyInfo.PropertyType,
                                 propertyInfo.CanRead,
                                 propertyInfo.CanWrite,
                                 isAnchor,
                                 isMultiValue);
                _getSchema.Add(_key, _prop);
            }

            return _getSchema;
        }
        //
        public Dictionary<Tuple<string, string>, Tuple<Type, bool, bool, bool, bool>> GetSchema()
        {
            var _getSchema = new Dictionary<Tuple<string, string>, Tuple<Type, bool, bool, bool, bool>>();
            PropertyInfo[] _propertyInfoArray = null;

            // User
            var _users = new users();
            _propertyInfoArray = _users.GetType().GetProperties();
            foreach (KeyValuePair<Tuple<string, string>, Tuple<Type, bool, bool, bool, bool>> kvp in getSchema(_users.GetType().Name, _propertyInfoArray))
            {
                _getSchema.Add(kvp.Key, kvp.Value);
            }
            // Group
            var _group = new groups();
            _propertyInfoArray = _group.GetType().GetProperties();
            foreach (KeyValuePair<Tuple<string, string>, Tuple<Type, bool, bool, bool, bool>> kvp in getSchema(_group.GetType().Name, _propertyInfoArray))
            {
                _getSchema.Add(kvp.Key, kvp.Value);
            }

            return _getSchema;
        }
    }
}
