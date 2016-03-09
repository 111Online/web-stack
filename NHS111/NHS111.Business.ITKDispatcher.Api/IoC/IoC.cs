using StructureMap;

namespace NHS111.Business.ITKDispatcher.Api.IoC
{
    public static class IoC
    {
        public static IContainer Initialize()
        {
            var cont = new Container(c => c.AddRegistry<ItkDispatcherApiRegistry>());
            cont.AssertConfigurationIsValid();
            return cont;
        }
    }
}