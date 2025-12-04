using System.Security.Cryptography;
using System.Text;

namespace FFAppMiddleware.API.Security
{
    public class CustomAuthenticationManager : ICustomAuthenticationManager
    {
        private readonly AuthenticationParams allUsers = new(CustomUserRoleEnum.RoleAleator);

        private readonly AuthenticationParams shopifyUser = new(CustomUserRoleEnum.ShopifyUser);

        private readonly string _allUsersToken;

        private readonly string _shopifyToken;

        private readonly Dictionary<string, AuthenticationParams> _staticTokenMap = new Dictionary<string, AuthenticationParams>();

        public CustomAuthenticationManager()
        {
            _allUsersToken = GenerateStaticToken(allUsers);
            _shopifyToken = GenerateStaticToken(shopifyUser);

            _staticTokenMap.Add(_allUsersToken, allUsers);
            _staticTokenMap.Add(_shopifyToken, shopifyUser);
        }

        public string Authenticate(string username, string password)
        {
            if (username == allUsers.Username && password == allUsers.Password)
            {
                return _allUsersToken; 
            }

            if(username == shopifyUser.Username && password == shopifyUser.Password)
            {
                return _shopifyToken;
            }

            return null;
        }

        public bool ValidateToken(string token)
        {
            return !string.IsNullOrEmpty(token) && _staticTokenMap.ContainsKey(token);
        }

        private string GenerateStaticToken(AuthenticationParams auth)
        {
            using var sha = SHA256.Create();
            string raw = $"{auth.Username}:{auth.Password}:static:{auth.Id}:{auth.Description} //";
            byte[] bytes = Encoding.UTF8.GetBytes(raw);

            byte[] hash = sha.ComputeHash(bytes);
            return Convert.ToHexString(hash);
        }

        public AuthenticationParams GetAuthenticationParamsByToken(string token)
        {
            AuthenticationParams authentication = null;

            if (ValidateToken(token))
            {
                _staticTokenMap.TryGetValue(token, out AuthenticationParams userParams);
                return authentication = userParams;
            }

            return authentication;
        }
    }
}
