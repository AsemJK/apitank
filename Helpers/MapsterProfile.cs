using Mapster;

namespace apitank.Helpers
{
    public class MapsterProfile : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<DTOs.ProductDto, Models.Product>();
        }
    }
}
