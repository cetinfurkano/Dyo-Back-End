using Dyo.Core.Extensions;
using Dyo.Core.Utilities.Security.Encryption;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Dyo.Core.Entities;

namespace Dyo.Core.Utilities.Security.JWT
{
    public class JwtHelper:ITokenHelper
    {
        public IConfiguration Configuration { get; }
        private TokenOptions _tokenOptions;
        private DateTime _accessTokenExpiration;

        public JwtHelper(IConfiguration configuration)
        {
            Configuration = configuration;
            _tokenOptions = Configuration.GetSection("TokenOptions").Get<TokenOptions>();
        }

        public AccessToken CreateToken<T>(T entity, string email, List<string> operationClaims)where T:
            class, IEntity, new()
        {
            
            _accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOptions.AccessTokenExpiration);
            var securityKey = SecurityKeyHelper.CreateSecurityKey(_tokenOptions.SecurityKey);
            var signingCredentials = SigningCredentialsHelper.CreateSigningCredentials(securityKey);
            var jwt = CreateJwtSecurityToken(_tokenOptions, entity, email, signingCredentials, operationClaims);
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var token = jwtSecurityTokenHandler.WriteToken(jwt);

            return new AccessToken
            {
                Expiration = _accessTokenExpiration,
                Token = token
            };
        }

        private JwtSecurityToken CreateJwtSecurityToken<T>(TokenOptions tokenOptions, T entity, string email, Microsoft.IdentityModel.Tokens.SigningCredentials signingCredentials, List<string> operationClaims) where T: class, IEntity, new()
        {
            var jwt = new JwtSecurityToken(
                     issuer: tokenOptions.Issuer,
                     audience: tokenOptions.Audience,
                     expires: _accessTokenExpiration,
                     notBefore: DateTime.Now,
                     claims: SetClaims(entity, email, operationClaims),
                     signingCredentials: signingCredentials
                     );

            return jwt;
        }

        private IEnumerable<Claim> SetClaims<T>(T entity, string email, List<string> operationClaims) 
            where T:class, IEntity, new()
        {
            var claims = new List<Claim>();
            claims.AddNameIdentifier(entity.Id.ToString());
            claims.AddEmail(email);
            claims.AddRoles(operationClaims);

            return claims;
        }
    }
}
