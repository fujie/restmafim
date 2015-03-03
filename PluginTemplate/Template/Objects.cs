using System;

// **** Remarks ****
// - Do not define multi-valued attributes as bool ( FIM do not support multi value for bool typed attributes)
//   You must define multi-valued attributes as string.
//
// To do
// - support for DateTime objectTypes
namespace Plugin
{
    // class name must be same as JSON Array in response
    public class users
    {
        private string _primaryEmail = null;
        // name
        private string _name__givenName = null;
        private string _name__familyName = null;
        private string _name__fullName = null;

        private bool _isAdmin = false;
        private bool _isDelegatedAdmin = false;
        //private DateTime _lastLoginTime;
        //private DateTime _creationTime;
        private bool _agreedToTerms = false;
        private string _password = null;
        private string _hashFunction = null;
        private bool _suspended = false;
        private string _suspensionReason = null;
        private bool _changePasswordAtNextLogin;
        private bool _ipWhitelisted;
        
        // ims
        private string _ims__type = null;
        private string _ims__customType = null;
        private string _ims__protocol = null;
        private string _ims__customProtocol = null;
        private string _ims__im = null;
        private string _ims__primary = null;

        // emails
        private string _emails__address = null;
        private string _emails__type = null;
        private string _emails__customType =null;
        private string _emails__primary = null;

        // externalIds
        private string _externalIds__value = null;
        private string _externalIds__type = null;
        private string _externalIds__customType = null;

        // relations
        private string _relations__value = null;
        private string _relations__type = null;
        private string _relations__customType = null;

        // addresses
        private string _addresses__type = null;
        private string _addresses__customType = null;
        private string _addresses__sourceIsStructured = null;
        private string _addresses__formatted = null;
        private string _addresses__poBox = null;
        private string _addresses__extendedAddress = null;
        private string _addresses__streetAddress = null;
        private string _addresses__locality = null;
        private string _addresses__region = null;
        private string _addresses__postalCode = null;
        private string _addresses__country = null;
        private string _addresses__primary = null;
        private string _addresses__countryCode = null;

        // organizations
        private string _organizations__name = null;
        private string _organizations__title = null;
        private string _organizations__primary = null;
        private string _organizations__type = null;
        private string _organizations__customType = null;
        private string _organizations__symbol = null;
        private string _organizations__location = null;
        private string _organizations__description = null;
        private string _organizations__domain = null;
        private string _organizations__costCenter = null;

        // phones
        private string _phones__value = null;
        private string _phones__primary = null;
        private string _phones__type = null;
        private string _phones__customType = null;

        private string _nonEditableAliases = null;
        private string _customerId = null;
        private string _orgUnitPath;
        private bool _isMailboxSetup = false;
        private bool _includeInGlobalAddressList;
        private string _thumbnailPhotoUrl = null;
        // not support for DateTime type
        //private DateTime _deletionTime;
        // not support for eTag type
        //private string _etag;

        // notes
        private string _notes__value = null;
        private string _notes__contentType = null;

        // websites
        private string _websites__value = null;
        private string _websites__primary = null;
        private string _websites__type = null;
        private string _websites__customType = null;


        [Anchor(true)]
        public string primaryEmail
        {
            get { return _primaryEmail; }
            set { _primaryEmail = value; }
        }

        // name
        public string name__givenName
        {
            get { return _name__givenName; }
            set { _name__givenName = value; }
        }
        public string name__familyName
        {
            get { return _name__familyName; }
            set { _name__familyName = value; }
        }
        public string name__fullName
        {
            get { return _name__fullName; }
        }

        public bool isAdmin
        {
            get { return _isAdmin; }
        }
        public bool isDelegatedAdmin
        {
            get { return _isDelegatedAdmin; }
        }
        // not support for DateTime type
        //public DateTime lastLoginTime
        //{
        //    get { return _lastLoginTime; }
        //}
        //public DateTime creationTime
        //{
        //    get { return _creationTime; }
        //}
        public bool agreedToTerms
        {
            get { return _agreedToTerms; }
        }
        public string password
        {
            set { _password = value; }
        }
        public string hashFunction
        {
            get { return _hashFunction; }
            set { _hashFunction = value; }
        }
        public bool suspended
        {
            get { return _suspended; }
            set { _suspended = value; }
        }
        public string suspensionReason
        {
            get { return _suspensionReason; }
        }
        public bool changePasswordAtNextLogin
        {
            get { return _changePasswordAtNextLogin; }
            set { _changePasswordAtNextLogin = value; }
        }
        public bool ipWhitelisted
        {
            get { return _ipWhitelisted; }
            set { _ipWhitelisted = value; }
        }
        [MultiValue(true)]
        public string ims__type
        {
            get { return _ims__type; }
            set { _ims__type = value; }
        }
        [MultiValue(true)]
        public string ims__customType
        {
            get { return _ims__customType; }
            set { _ims__customType = value; }
        }
        [MultiValue(true)]
        public string ims__protocol
        {
            get { return _ims__protocol; }
            set { _ims__protocol = value; }
        }
        [MultiValue(true)]
        public string ims__customProtocol
        {
            get { return _ims__customProtocol; }
            set { _ims__customProtocol = value; }
        }
        [MultiValue(true)]
        public string ims__im
        {
            get { return _ims__im; }
            set { _ims__im = value; }
        }
        [MultiValue(true)]
        public string ims__primary
        {
            get { return _ims__primary; }
            set { _ims__primary = value; }
        }
        [MultiValue(true)]
        public string emails__address
        {
            get { return _emails__address; }
            set { _emails__address = value; }
        }
        [MultiValue(true)]
        public string emails__type
        {
            get { return _emails__type; }
            set { _emails__type = value; }
        }
        [MultiValue(true)]
        public string emails__customType
        {
            get { return _emails__customType; }
            set { _emails__customType = value; }
        }
        [MultiValue(true)]
        public string emails__primary
        {
            get { return _emails__primary; }
            set { _emails__primary = value; }
        }
        [MultiValue(true)]
        public string externalIds__value
        {
            get { return _externalIds__value; }
            set { _externalIds__value = value; }
        }
        [MultiValue(true)]
        public string externalIds__type
        {
            get { return _externalIds__type; }
            set { _externalIds__type = value; }
        }
        [MultiValue(true)]
        public string externalIds__customType
        {
            get { return _externalIds__customType; }
            set { _externalIds__customType = value; }
        }
        [MultiValue(true)]
        public string relations__value
        {
            get { return _relations__value; }
            set { _relations__value = value; }
        }
        [MultiValue(true)]
        public string relations__type
        {
            get { return _relations__type; }
            set { _relations__type = value; }
        }
        [MultiValue(true)]
        public string relations__customType
        {
            get { return _relations__customType; }
            set { _relations__customType = value; }
        }
        [MultiValue(true)]
        public string addresses__type
        {
            get { return _addresses__type; }
            set { _addresses__type = value; }
        }
        [MultiValue(true)]
        public string addresses__customType
        {
            get { return _addresses__customType; }
            set { _addresses__customType = value; }
        }
        [MultiValue(true)]
        public string addresses__sourceIsStructured
        {
            get { return _addresses__sourceIsStructured; }
            set { _addresses__sourceIsStructured = value; }
        }
        [MultiValue(true)]
        public string addresses__formatted
        {
            get { return _addresses__formatted; }
            set { _addresses__formatted = value; }
        }
        [MultiValue(true)]
        public string addresses__poBox
        {
            get { return _addresses__poBox; }
            set { _addresses__poBox = value; }
        }
        [MultiValue(true)]
        public string addresses__extendedAddress
        {
            get { return _addresses__extendedAddress; }
            set { _addresses__extendedAddress = value; }
        }
        [MultiValue(true)]
        public string addresses__streetAddress
        {
            get { return _addresses__streetAddress; }
            set { _addresses__streetAddress = value; }
        }
        [MultiValue(true)]
        public string addresses__locality
        {
            get { return _addresses__locality; }
            set { _addresses__locality = value; }
        }
        [MultiValue(true)]
        public string addresses__region
        {
            get { return _addresses__region; }
            set { _addresses__region = value; }
        }
        [MultiValue(true)]
        public string addresses__postalCode
        {
            get { return _addresses__postalCode; }
            set { _addresses__postalCode = value; }
        }
        [MultiValue(true)]
        public string addresses__country
        {
            get { return _addresses__country; }
            set { _addresses__country = value; }
        }
        [MultiValue(true)]
        public string addresses__primary
        {
            get { return _addresses__primary; }
            set { _addresses__primary = value; }
        }
        [MultiValue(true)]
        public string addresses__countryCode
        {
            get { return _addresses__countryCode; }
            set { _addresses__countryCode = value; }
        }

        // organizations
        [MultiValue(true)]
        public string organizations__name
        {
            get { return _organizations__name; }
            set { _organizations__name = value; }
        }
        [MultiValue(true)]
        public string organizations__title
        {
            get { return _organizations__title; }
            set { _organizations__title = value; }
        }
        [MultiValue(true)]
        public string organizations__primary
        {
            get { return _organizations__primary; }
            set { _organizations__primary = value; }
        }
        [MultiValue(true)]
        public string organizations__type
        {
            get { return _organizations__type; }
            set { _organizations__type = value; }
        }
        [MultiValue(true)]
        public string organizations__customType
        {
            get { return _organizations__customType; }
            set { _organizations__customType = value; }
        }
        [MultiValue(true)]
        public string organizations__symbol
        {
            get { return _organizations__symbol; }
            set { _organizations__symbol = value; }
        }
        [MultiValue(true)]
        public string organizations__location
        {
            get { return _organizations__location; }
            set { _organizations__location = value; }
        }
        [MultiValue(true)]
        public string organizations__description
        {
            get { return _organizations__description; }
            set { _organizations__description = value; }
        }
        [MultiValue(true)]
        public string organizations__domain
        {
            get { return _organizations__domain; }
            set { _organizations__domain = value; }
        }
        [MultiValue(true)]
        public string organizations__costCenter
        {
            get { return _organizations__costCenter; }
            set { _organizations__costCenter = value; }
        }
        
        // phones
        [MultiValue(true)]
        public string phones__value
        {
            get { return _phones__value; }
            set { _phones__value = value; }
        }
        [MultiValue(true)]
        public string phones__primary
        {
            get { return _phones__primary; }
            set { _phones__primary = value; }
        }
        [MultiValue(true)]
        public string phones__type
        {
            get { return _phones__type; }
            set { _phones__type = value; }
        }
        [MultiValue(true)]
        public string phones__customType
        {
            get { return _phones__customType; }
            set { _phones__customType = value; }
        }
        [MultiValue(true)]
        public string nonEditableAliases
        {
            get { return _nonEditableAliases; }
        }
        public string customerId
        {
            get { return _customerId; }
        }
        public string orgUnitPath
        {
            get { return _orgUnitPath; }
            set { _orgUnitPath = value; }
        }
        public bool isMailboxSetup
        {
            get { return _isMailboxSetup; }
        }
        public bool includeInGlobalAddressList
        {
            get { return _includeInGlobalAddressList; }
            set { _includeInGlobalAddressList = value; }
        }
        public string thumbnailPhotoUrl
        {
            get { return _thumbnailPhotoUrl; }
        }
        // not support for DateTime type
        //public DateTime deletionTime
        //{
        //    get { return _deletionTime; }
        //}
        // not support for eTag type
        //public string eTag
        //{
        //    get { return _eTag; }
        //}

        // notes
        [MultiValue(true)]
        public string notes__value
        {
            get { return _notes__value; }
            set { _notes__value = value; }
        }
        [MultiValue(true)]
        public string notes__contentType
        {
            get { return _notes__contentType; }
            set { _notes__contentType = value; }
        }

        // websites
        [MultiValue(true)]
        public string websites__value
        {
            get { return _websites__value; }
            set { _websites__value = value; }
        }
        [MultiValue(true)]
        public string websites__primary
        {
            get { return _websites__primary; }
            set { _websites__primary = value; }
        }
        [MultiValue(true)]
        public string websites__type
        {
            get { return _websites__type; }
            set { _websites__type = value; }
        }
        [MultiValue(true)]
        public string websites__customType
        {
            get { return _websites__customType; }
            set { _websites__customType = value; }
        }
    
    }

    // groups
    public class groups
    {
        private string _email;
        private string _name;
        private string _description;

        [Anchor(true)]
        public string email
        {
            get { return _email; }
            set { _email = value; }
        }
        public string name
        {
            get { return _name; }
            set { _name = value; }
        }
        public string description
        {
            get { return _description; }
            set { _description = value; }
        }
    }
    //
    // define custom attribute
    //
    public class AnchorAttribute : Attribute
    {
        private bool _isAnchor;
        public AnchorAttribute(bool isAnchor) { this._isAnchor = isAnchor; }
        public bool isAnchor { get { return this._isAnchor; } }
    }
    public class MultiValueAttribute : Attribute
    {
        private bool _isMultiValue;
        public MultiValueAttribute(bool isMultiValue) { this._isMultiValue = isMultiValue; }
        public bool isMultiValue { get { return this._isMultiValue; } }
    }
}
