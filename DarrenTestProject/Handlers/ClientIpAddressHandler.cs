using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace DarrenTestProject.Handlers
{
    /* 
    In this handler, though, you will process the X-Forwarded-For header, or the for= portion of the Forwarded header.  This will let you work with either the RFC standard or the de-facto legacy headers.   

    One important thing to note about these headers when more than one proxying device is in the networking chain.  With X-Forwarded-For, there can be a comma-delimited list of IP addresses.  The FIRST one in the list is the original caller IP, the rest are the chain of proxies.  With the Forwarded header, there can be more than one "for=", but the FIRST one in the string is the original caller.

    Note for later use that if you plan to use OWIN or self-hosting models instead of IIS for your service, check out this code in the WebApiContrib project to get the IP for any hosting model:
    https://github.com/WebApiContrib/WebAPIContrib/blob/master/src/WebApiContrib/Http/HttpRequestMessageExtensions.cs
    … but for this exercise, the IIS version above is sufficient.   

    You’ll have to use PostMan to test, since it allows you to easily create the X-Forward-For or Forwarded headers.  Use the standard ValuesController Get method to return the IP address as one of the strings. The Section 4 code so far is attached to this assignment so you can use it as a starting point for adding your handler.

    In PostMan, you can use the following examples to verify your handler is working properly.  In each case, the proper client IP your handler should calculate is 192.168.1.1.  If you don't include either header type, you should get your own local machine's IP address.

    Forwarded =>           by=10.0.0.1;for=192.168.1.1;host=mycompany.com
    X-Forwarded-For =>     192.168.1.1
    X-Forwarded-For =>     192.168.1.1,10.0.0.1
    */

    /// <summary>
    /// Message handler to retrieve any incoming client IP address, taking into account 
    /// any load balancers or other proxy servers.
    public class ClientIpAddressHandler : DelegatingHandler
    {
        const string _fwdHeader = "Forwarded";
        public const string _fwdForHeader = "X-Forwarded-For";

        /// <summary>
        /// Message handler to retrieve the incoming client ip address,
        /// accounting for any load balancer in use.
        /// </summary>
        protected override Task<HttpResponseMessage> SendAsync(
              HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string ip = null;
            // X-Forwarded-For header - the first one in the comma-separated string is the original client if more than one
            if (request.Headers.Contains(_fwdForHeader))
            {
                ip = request.Headers.GetValues(_fwdForHeader).FirstOrDefault(s => !String.IsNullOrEmpty(s));
                if (!String.IsNullOrEmpty(ip) && ip.Contains(","))
                    ip = ip.Substring(0, ip.IndexOf(','));
            }
            if (String.IsNullOrEmpty(ip) && request.Headers.Contains(_fwdHeader))
            {
                var fwd = request.Headers.GetValues(_fwdHeader)
                    .FirstOrDefault(s => !String.IsNullOrEmpty(s))
                    .Split(';')
                    .Select(s => s.Trim());
                // syntax for the Forwarded header: Forwarded: by=<identifier>; for=<identifier>; host=<host>; proto=<http|https>
                ip = fwd.FirstOrDefault(s => s.ToLowerInvariant().StartsWith("for="));
                if (!String.IsNullOrEmpty(ip))
                    ip = ip.Substring(4);
            }
            // try the http context to see what it has, if none of the standard headers have worked out
            if (String.IsNullOrEmpty(ip))
            {
                if (request.Properties.ContainsKey("MS_HttpContext"))
                {
                    var ctx = request.Properties["MS_HttpContext"] as HttpContextBase;
                    if (ctx != null)
                        ip = ctx.Request.UserHostAddress;
                }
            }

            // store it off if we found it
            if (!String.IsNullOrEmpty(ip))
                request.Properties.Add(_fwdForHeader, ip);

            // continue the handler chain
            return base.SendAsync(request, cancellationToken);
        }
    }

    /// <summary>
    /// Note:
    /// Here is an extension to get the IP address from the Request.Properties collection;
    /// as with the other example I used the X-Forwarded-For header name as the storage key for the collection,
    /// but you can use any key you like.
    /// 
    /// http request extension for retrieving client ip
    /// </summary>
    public static class HttpRequestMessageClientIpAddressExtension
    {
        public static string GetClientIpAddress(this HttpRequestMessage request)
        {
            if (request == null)
                return null;
            if (request.Properties.TryGetValue(ClientIpAddressHandler._fwdForHeader, out object ip))
            {
                return (string)ip;
            }
            return null;
        }
    }
}