using AutoMapper;
using SPMS.Dtos;
using SPMS.Models;

namespace SPMS.Profiles
{
 public class MappingProfile : Profile
 {
 public MappingProfile()
 {
 CreateMap<BookingCreateDto, Booking>()
 .ForMember(dest => dest.BookingType, opt => opt.MapFrom(src => (BookingType)src.BookingType));
 CreateMap<Booking, BookingResponseDto>()
 .ForMember(dest => dest.BookingType, opt => opt.MapFrom(src => (int)src.BookingType))
 .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (int)src.Status));

 CreateMap<PaymentInitiateDto, Payment>();
 CreateMap<Payment, PaymentResponseDto>();

 CreateMap<VehicleCreateDto, Vehicle>();
 CreateMap<Vehicle, VehicleResponseDto>();

 CreateMap<ParkingSpaceCreateDto, ParkingSpace>();
 CreateMap<ParkingSpace, ParkingSpaceResponseDto>();

 CreateMap<ParkingSlotCreateDto, ParkingSlot>();
 CreateMap<ParkingSlot, ParkingSlotResponseDto>();

 CreateMap<RegisterDto, User>();
 CreateMap<User, UserResponseDto>();
 }
 }
}