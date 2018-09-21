// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace StsServerIdentity
{
    public class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email()
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>();
        }

        // clients want to access resources (aka scopes)
        public static IEnumerable<Client> GetClients(IConfigurationSection stsConfig)
        {
            var demoDeploymentUrl = stsConfig["DemoDeploymentUrl"];
            // TODO use configs in app

            // client credentials client
            return new List<Client>
            {
                 new Client
                {
                    ClientName = "hybridclient",
                    ClientId = "hybridclient",
                    ClientSecrets = {new Secret("hybrid_flow_secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    AllowOfflineAccess = true,
                    RedirectUris = {
                        "https://localhost:44389/signin-oidc",
                        $"{demoDeploymentUrl}/signin-oidc"
                    },
                    PostLogoutRedirectUris = {
                        "https://localhost:44389/signout-callback-oidc",
                        $"{demoDeploymentUrl}/signout-callback-oidc"
                    },
                    AllowedCorsOrigins = new List<string>
                    {
                        "https://localhost:44389/",
                        $"{demoDeploymentUrl}/"
                    },
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "scope_used_for_hybrid_flow",
                        "role"
                    }
                },
            };
        }
    }
}