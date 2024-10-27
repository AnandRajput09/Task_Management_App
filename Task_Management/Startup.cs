using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.IdentityModel.Tokens; // Ensure you have this using for TokenValidationParameters
using System.Text;
using Microsoft.AspNet.SignalR;

[assembly: OwinStartup(typeof(Task_Management.Startup))]
namespace Task_Management
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Configure SignalR
            app.MapSignalR();

            // Configure JWT Authentication
            ConfigureAuth(app);
        }

        public void ConfigureAuth(IAppBuilder app)
        {
            var issuer = "Anand"; // Replace with your JWT issuer
            // "my_secret_key_123"  ->> convert -->>   bXlfc2VjcmV0X2tleV8xMjM=
            var secret = TextEncodings.Base64Url.Decode("TXlWZXJ5U2VjcmV0S2V5VGhhdElzTG9uZ0Vub3VnaDEyMyE="); // Replace with a strong base64 encoded secret key

            app.UseJwtBearerAuthentication(new JwtBearerAuthenticationOptions
            {
                AuthenticationMode = AuthenticationMode.Active,
                TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = issuer,
                    ValidAudience = issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(secret)
                }
            });
        }
    }
}
