using Gacha.LogIn;
using Nancy;
using Nancy.Extensions;

namespace Gacha
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            // Gacha
            Post["/Gacha"] = parameters =>
            {
                var jsonFromClient = this.Request.Body.AsString();
                
                var controller = new GachaController();
                var ret = controller.Perform(jsonFromClient);

                return ret;
            };

            // Sign In
            Post["/SignIn"] = parameters =>
            {
                var jsonFromClient = this.Request.Body.AsString();

                var controller = new SignInController();
                var ret = controller.Perform(jsonFromClient);

                return ret;
            };

            // Log In
            Post["/LogIn"] = parameters =>
            {
                var jsonFromClient = this.Request.Body.AsString();
                var controller = new LogInController();
                var ret = controller.Perform(jsonFromClient);

                return ret;
            };
        }
    }
}