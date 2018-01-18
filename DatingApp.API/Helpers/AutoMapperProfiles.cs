using System.Linq;
using AutoMapper;
using DatingApp.API.Models.Data;
using DatingApp.API.ViewModel;

namespace DatingApp.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User,UserDetailViewModel>()
                .ForMember(dest=> dest.PhotoUrl, 
                            ops=> ops.MapFrom(src=> src.Photos.FirstOrDefault(p=> p.IsMain).Url)
                          )
                .ForMember(dest=> dest.Age, 
                            ops=> ops.ResolveUsing(d=> d.DateOfBirth.CalculateAge())
                          );
            CreateMap<User,UserListViewModel>()
                    .ForMember(dest=> dest.PhotoUrl, 
                            ops=> ops.MapFrom(src=> src.Photos.FirstOrDefault(p=> p.IsMain).Url)
                          )
                    .ForMember(dest=> dest.Age, 
                            ops=> ops.ResolveUsing(d=> d.DateOfBirth.CalculateAge())
                          );
            CreateMap<Photo,PhotoesForDetailViewModel>();

            CreateMap<UserForupdateViewModel,User>();
        }
    }
}