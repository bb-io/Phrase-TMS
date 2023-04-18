using Apps.PhraseTMS.Authorization.OAuth2;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication.OAuth2;


namespace Apps.PhraseTMS
{
    public class PhraseTMSApplication : IApplication
    {
        private string _name;
        private readonly Dictionary<Type, object> _typesInstances;

        public PhraseTMSApplication()
        {
            _name = "PhraseTMS";
            _typesInstances = CreateTypesInstances();
        }

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public T GetInstance<T>()
        {
            if (!_typesInstances.TryGetValue(typeof(T), out var value))
            {
                throw new InvalidOperationException($"Instance of type '{typeof(T)}' not found");
            }
            return (T)value;
        }

        private Dictionary<Type, object> CreateTypesInstances()
        {
            return new Dictionary<Type, object>
            {
                { typeof(IOAuth2AuthorizeService), new OAuth2AuthorizeService() },
                { typeof(IOAuth2TokenService), new OAuth2TokenService() }
            };
        }
    }
}
