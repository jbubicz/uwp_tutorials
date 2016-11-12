﻿// <copyright file="ContosoCallbackUriMapper.cs" company="Microsoft Corporation.">
//     Copyright (c) 2013 Microsoft Corporation. All rights reserved.
// </copyright>
namespace ContosoSocial
{
    using System;
    using System.Net;
    using System.Windows;
    using System.Windows.Navigation;
    using Facebook.Client;

    /// <summary>
    /// Implements the URI mapper for this application
    /// </summary>
    public class CallbackUriMapper : UriMapperBase
    {
        /// <summary>
        /// The facebook login flow has been handled in the current app invocation
        /// </summary>
        private bool facebookLoginHandled;

        /// <summary>
        /// Maps a deep link Uri to a navigation within this application
        /// </summary>
        /// <param name="uri">Deep link Uri to map</param>
        /// <returns>Navigation Uri within this app</returns>
        public override Uri MapUri(Uri uri)
        {
            // if URI is a facebook login response, handle the deep link (once per invocation)
            if (AppAuthenticationHelper.IsFacebookLoginResponse(uri))
            {
                FacebookSession session = new FacebookSession();

                try
                {
                    session.ParseQueryString(HttpUtility.UrlDecode(uri.ToString()));

                    // Handle success case

                    // do something with the custom state parameter
                    if (session.State != "custom_state_string")
                    {
                        MessageBox.Show("Unexpected state: " + session.State);
                    }
                    else
                    {
                        // save the token and continue (token is retrieved and used when the app
                        // is launched)
                        SessionStorage.Save(session);
                    }
                }
                catch (Facebook.FacebookOAuthException exc)
                {
                    if (!this.facebookLoginHandled)
                    {
                        // Handle error case
                        MessageBox.Show("Not signed in: " + exc.Message);

                        this.facebookLoginHandled = true;
                    }
                }

                return new Uri("/MainPage.xaml", UriKind.Relative);
            }

            // by default, navigate to the requested uri
            return uri;
        }
    }
}
