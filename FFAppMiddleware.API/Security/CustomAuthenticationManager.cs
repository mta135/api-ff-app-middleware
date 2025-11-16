using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;

namespace FFAppMiddleware.API.Security
{
    public class CustomAuthenticationManager : ICustomAuthenticationManager
    {
        UserAuthenticationModel multipleUser = new UserAuthenticationModel
        {
            USERNAME = "admin",
            PASSWORD = "password123",
            ID = "a3f9c2d1-4b7e-4f1a-9d8c-2e5b6a7c8d9f",
            DESCRIPTION = "Dita EstFarm"
        };

        UserAuthenticationModel shopifyUser = new UserAuthenticationModel
        {
            USERNAME = "shopify_user",
            PASSWORD = "shopify_pass",
            ID = "b4g0d3e2-5c8f-5g2b-0h9d-3f6c7b8d9e0g",
            DESCRIPTION = "Shopify Integration User"
        };


        private readonly string _multipleUsersToken;
        private readonly string _shopifyUser;


        private static  Dictionary<string, AuthenticationParams> _staticTokenMap;

        public CustomAuthenticationManager()
        {
            _multipleUsersToken = GenerateStaticToken(multipleUser);
            _shopifyUser = GenerateStaticToken(shopifyUser);

            _staticTokenMap = new Dictionary<string, AuthenticationParams>
            {
                {
                    _multipleUsersToken,
                    new AuthenticationParams
                    {
                        Username = multipleUser.USERNAME,
                        Password = multipleUser.PASSWORD,
                        Id = multipleUser.ID,
                        Description = multipleUser.DESCRIPTION
                    }
                },
                {
                    _shopifyUser,
                    new AuthenticationParams
                    {
                        Username = shopifyUser.USERNAME,
                        Password = shopifyUser.PASSWORD,
                        Id = shopifyUser.ID,
                        Description = shopifyUser.DESCRIPTION
                    }
                }
            };
        }

        public string Authenticate(string username, string password)
        {
            if (username == multipleUser.USERNAME && password == multipleUser.PASSWORD)
            {
                return _multipleUsersToken; 
            }

            if(username == shopifyUser.USERNAME && password == shopifyUser.PASSWORD)
            {
                return _shopifyUser;
            }

            return null;
        }

        public bool ValidateToken(string token)
        {
            return !string.IsNullOrEmpty(token) && _staticTokenMap.ContainsKey(token);
        }

        public string GetUsernameFromToken(string token)
        {
            return ValidateToken(token) ? multipleUser.USERNAME : null;
        }



        private string GenerateStaticToken(UserAuthenticationModel authenticationModel)
        {
            using var sha = SHA256.Create();

            string raw = $"{authenticationModel.USERNAME}:{authenticationModel.PASSWORD}:static:{authenticationModel.ID}:{authenticationModel.DESCRIPTION} //";
            byte[] bytes = Encoding.UTF8.GetBytes(raw);

            byte[] hash = sha.ComputeHash(bytes);
            return Convert.ToHexString(hash);
        }


        public AuthenticationParams GetAuthenticationParamsByToken(string token)
        {
            return ValidateToken(token) ? new AuthenticationParams()
            {
                //Username = USERNAME,
                //Password = PASSWORD,
                //Description = DESCRIPTION,
                //Id = ID,

            } : null;
        }
    }

    public class AuthenticationParams
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string Id { get; set; }

        public string Description { get; set; }

        public string Role { get; set; } = "RoleAleator";
    }
}
