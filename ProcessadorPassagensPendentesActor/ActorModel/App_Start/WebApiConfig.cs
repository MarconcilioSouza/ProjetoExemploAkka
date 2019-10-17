using ConectCar.Framework.Infrastructure.Ioc;
using ConectCar.Framework.Infrastructure.Ioc.Validation;
using ProcessadorPassagensActors.CommandQuery.Handlers;
using System.Web.Http;
using ConectCar.Transacoes.Domain.Model;
using ProcessadorPassagensActors.CommandQuery.Handlers.Artesp;

namespace ProcessadorPassagensActors
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Ioc Container            
            IocContainer.Container.RegisterValidators(typeof(ValidadorPassagemPendenteArtespHandler).Assembly);
            IocContainer.Container.RegisterValidators(typeof(PassagemPendenteEDI).Assembly);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
