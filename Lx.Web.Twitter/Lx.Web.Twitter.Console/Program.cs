using System.Reflection;
using Autofac;

namespace Lx.Web.Twitter.Console
{
    class Program
    {
        static Program()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<TwitterMain>();
            builder.RegisterAssemblyTypes(typeof(Program).Assembly)
                .AsImplementedInterfaces();
            Container = builder.Build();
        }

        public static IContainer Container { get; set; }

        public static void Main(string[] args)
        {
            ContainerConfigure(Container);
            var program = Container.Resolve<TwitterMain>();
            program.Run(args);
        }

        public static void ContainerConfigure(IContainer container)
        {

        }
    }
}
