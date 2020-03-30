using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace DarrenTestProject.Handlers
{
    /* Problem:
     You’ve learned everything you’ll need to complete this exercise, and it has a side benefit of potentially being useful code you can use for certain scenarios you might run into in the future.

    In this exercise, I want you to create a new DelegatingHandler that implements the X-HTTP-Method-Override behavior, with an extra twist.    

    First, here’s a description of X-HTTP-Method-Override.  In some cases, usually involving browsers, network appliances or firewall rules, certain HTTP methods are blocked, making it difficult to create a REST API based on the full list of HTTP verbs. For example the firewall might allow GET and POST, but not allow PUT, PATCH, or DELETE HTTP methods.  

    The X-HTTP-Method-Override HTTP header was invented as a workaround.  When a server receives a request that uses the POST method call that includes the X-HTTP-Method-Override header set to another HTTP method, the server is supposed to treat the request as if the method was not POST but rather the method listed in the header.  

    So in your delegating handler, you will implement the following behavior:  

    1) Check the HTTP method. If not POST, bail early from the handler.  This is not an error condition; the request could be a valid request for one of the other verbs, so don't return an error in this case.

    2) If POST, look for the X-HTTP-Method-Override.  If present, and if the value is in the list PUT, PATCH, DELETE and let’s add HEAD and VIEW as extra twists on the usual version, override the HTTP method in the Request to the value from the header.  If not present, you should not return an error response as this is a valid, normal POST request with no override involved.    If the header is present but not in the list, use an error response.

    You’ll have to use PostMan to test, since it allows you to easily create the X-HTTP-Method-Override and play with different values.  Use the standard ValuesController to test that you successfully get routed to the Put, Patch and Delete methods.  Add the HEAD and VIEW verbs to one of the GET methods and verify you get routed to the method. Remember in PostMan your request must be a POST verb, but try another to ensure your handler is properly triggering its logic only on POST.  

    To make it easier, consider starting with the DelegatingHandlerTemplate class from the HandlerTemplates resource project attached to this section of the course. 
     */

    /*
        Below is the Instructors solution:
        Note the _header and _validMethods fields are readonly/const so there are no threading
        issues to be worried about for those values. Handlers and filters should avoid any fields
        that get written to while running the handler, since there is only one instance of
        the handler class being used to serve many concurrent requests.
     */
    public class MethodOverrideWithViewHandler : DelegatingHandler
    {
        /// <summary>
        /// Names of http methods that are valid to substitute
        /// </summary>
        readonly string[] _validMethods = {
            HttpMethod.Put.Method,
            HttpMethod.Delete.Method,
            HttpMethod.Head.Method,
            "VIEW", "PATCH"
        };

        /// <summary>
        /// HTTP header name to process
        /// </summary>
        const string _header = "X-HTTP-Method-Override";

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // STEP 1: Global message-level logic that must be executed BEFORE the request
            //          is sent on to the action method

            // X-HTTP-Method-Override header requires POST verb
            if (request.Method.Equals(HttpMethod.Post) && request.Headers.Contains(_header))
            {
                // Check for a valid new method from the list
                var method = request.Headers.GetValues(_header).FirstOrDefault();
                if (_validMethods.Contains(method, StringComparer.InvariantCultureIgnoreCase))
                {
                    // Change the request method to the override one
                    request.Method = new HttpMethod(method.ToUpperInvariant());
                }
            }
            // Collapse STEPs 2-4 since this handler doesn't do any response processing.
            return base.SendAsync(request, cancellationToken);
        }
    }
}