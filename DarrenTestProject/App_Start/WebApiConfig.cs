using DeLoachAero.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Routing;
using DarrenTestProject.Models;
using DarrenTestProject.Handlers;
using HandlerTemplates.Filters;

namespace DarrenTestProject
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // see AuthenticationFilterTemplate for notes about this setting
            config.SuppressHostPrincipal();

            // Web API configuration and services
            // register our Delegating Handlers
            config.MessageHandlers.Add(new FullPipelineTimerHandler());
            config.MessageHandlers.Add(new ApiKeyHeaderHandler());
            // We don't need this, cause all the headers we want to remove, can be accomplished with
            // settings changes inside the web.config
            // config.MessageHandlers.Add(new RemoveBadHeadersHandler());
            config.MessageHandlers.Add(new MethodOverrideWithViewHandler());
            config.MessageHandlers.Add(new ForwardedHeadersHandler());

            config.Filters.Add(new BasicAuthFilterAttribute());
            // config.Filters.Add(new JwtAuthenticationFilterAttribute());
            config.Filters.Add(new AuthorizeAttribute());

            config.MessageHandlers.Add(new ClientIpAddressHandler());

            // Global Filter
            config.Filters.Add(new RouteTimerFilterAttribute("Global"));


            // Web API routes
            var constraintResolver = new DefaultInlineConstraintResolver();
            constraintResolver.ConstraintMap.Add("enum", typeof(EnumerationConstraint));
            constraintResolver.ConstraintMap.Add("base64", typeof(Base64Constraint));
            config.MapHttpAttributeRoutes(constraintResolver);

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);
        }
    }
}
