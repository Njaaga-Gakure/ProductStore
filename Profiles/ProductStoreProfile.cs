using AutoMapper;
using ProductStore.Model;
using ProductStore.Model.DTOs;

namespace ProductStore.Profiles
{
    public class ProductStoreProfile: Profile
    {
        public ProductStoreProfile()
        {
            CreateMap<RegisterUserDTO, User>().ReverseMap();    
            CreateMap<AddProductDTO, Product>().ReverseMap();   
            CreateMap<AddNewOrderDTO, Order>().ReverseMap();
            CreateMap<Order, OrderDTO>()
               .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
               .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Product.Id))
               .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
               .ForMember(dest => dest.ProductDescription, opt => opt.MapFrom(src => src.Product.Description))
               .ForMember(dest => dest.ProductPrice, opt => opt.MapFrom(src => src.Product.Price))
               .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.Id))
               .ForMember(dest => dest.OrderCreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
               .ForMember(dest => dest.OrderUpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt));
        }
    }
}
