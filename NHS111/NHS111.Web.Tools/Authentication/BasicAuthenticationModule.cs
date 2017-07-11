using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace NHS111.Web.Tools.Authentication
{
    public class BasicAuthenticationModule : Devbridge.BasicAuthentication.BasicAuthenticationModule
    {

        protected override bool ValidateCredentials(string userName, string password)
        {
            var configUsername = ConfigurationManager.AppSettings["login_tools_credential_user"];
            var configPassword = ConfigurationManager.AppSettings["login_tools_credential_password"];
            if (configUsername != null && configUsername == userName &&
              configPassword != null && configPassword == password)
            {
                return true;
            }

            return false;
        }
    }
}