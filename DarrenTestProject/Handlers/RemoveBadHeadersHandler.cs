using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace DarrenTestProject.Handlers
{
    // REMEMBER this handler doesn't work - we used it to show the limits of handlers
    public class RemoveBadHeadersHandler : DelegatingHandler
    {
        /// <summary>
        /// Names of headers to remove
        /// </summary>
        readonly string[] _badHeaders = { "X-Powered-By", "X-AspNet-Version", "Server" };

        protected async override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // STEP 2: Call the rest of the pipeline, all the way to a response message
            var response = await base.SendAsync(request, cancellationToken);

            // STEP 3: Any global message-level logic that must be executed AFTER the request
            //          has executed, before the final HTTP response message
            // remove all the bad headers from the collection
            foreach (var h in _badHeaders)
            {
                response.Headers.Remove(h);
                // Overwrite the header to something else, something that won't help an attacker.
                // response.Headers.Add(h, "NA");
            }

            // STEP 4:  Return the final HTTP response
            return response;
        }

    }
}