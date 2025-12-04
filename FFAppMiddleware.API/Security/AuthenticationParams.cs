namespace FFAppMiddleware.API.Security
{
    public class AuthenticationParams
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string Id { get; set; }

        public string Description { get; set; }

        public string Role { get; set; }

        public AuthenticationParams()
        {

        }

        public AuthenticationParams(CustomUserRoleEnum roles)
        {
            switch (roles)
            {
                case CustomUserRoleEnum.RoleAleator:

                    Username = "admin";
                    Password = "password123";
                    Id = "a3f9c2d1-4b7e-4f1a-9d8c-2e5b6a7c8d9f";
                    Description = "Dita EstFarm";
                    Role = nameof(CustomUserRoleEnum.RoleAleator);
              
                    break;

              case CustomUserRoleEnum.ShopifyUser:

                    Username = "ShopifyUserName";
                    Password = "gR8!Vz@1pK#m47Qs*Ld9$Bf2Pn&x";
                    Id = "f3c9a6e5-1d89-4b7e-a82b-3f3e66fbc3e1";
                    Description = "Utilizator Shopify";
                    Role = nameof(CustomUserRoleEnum.ShopifyUser);

                    break;


                default:
                    throw new ArgumentOutOfRangeException(nameof(roles));
            }

        }
    }
}
