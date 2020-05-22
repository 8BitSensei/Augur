using Autofac;
using LiteDB;
using LiteDbTest.Commands;
using LiteDbTest.Interfaces;
using LiteDbTest.Services;

namespace LiteDbTest
{
    public class Container
    {
        private ContainerBuilder _builder;
        public IContainer container;
        private LiteDatabase _db;

        public Container(LiteDatabase db)
        {
            _builder = new ContainerBuilder();
            container = ConfigureServices(_builder);
            _db = db;
        }

        private IContainer ConfigureServices(ContainerBuilder builder)
        {
            builder.RegisterType<Delete>().AsImplementedInterfaces();
            builder.RegisterType<Export>().AsImplementedInterfaces();
            builder.RegisterType<Reset>().AsImplementedInterfaces();
            builder.RegisterType<Run>().AsImplementedInterfaces();

            builder.Register(x =>
            {
                return new ConfigService(_db);
            }).AsImplementedInterfaces();
            builder.Register(x =>
            {
                return new ScraperService(_db);
            }).AsImplementedInterfaces();
            builder.Register(x =>
            {
                return new DataService(_db);
            }).AsSelf().AsImplementedInterfaces().SingleInstance();
            builder.Register(ctx =>
            {
                return new PrinterService(_db, ctx.Resolve<IDataService>());
            }).AsImplementedInterfaces();
            return builder.Build();
        }


    }
}
