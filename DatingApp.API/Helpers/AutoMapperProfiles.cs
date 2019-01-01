using System.Linq;
using AutoMapper;
using DatingApp.API.Dtos;
using DatingApp.API.Models;

namespace DatingApp.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserForListDto>()
            .ForMember(p => p.PhotoUrl, opt =>
            {
                opt.MapFrom(u => u.Photos.FirstOrDefault(x => x.IsMain).Url);
            })
            .ForMember(u => u.Age, opt =>
            {
                opt.ResolveUsing(d => d.DateOfBirth.CalculateAge());
            });

            CreateMap<User, UserForDetailedDto>()
            .ForMember(p => p.PhotoUrl, opt =>
            {
                opt.MapFrom(u => u.Photos.FirstOrDefault(x => x.IsMain).Url);
            })
            .ForMember(u => u.Age, opt =>
            {
                opt.ResolveUsing(d => d.DateOfBirth.CalculateAge());
            });

            CreateMap<Photo, PhotosForDetailedDto>();
        }
    }
}