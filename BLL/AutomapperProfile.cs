using AutoMapper;
using BLL.Models;
using DAL.Entities;
using System.Linq;
using System;

namespace BLL
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Card, CardModel>()
                .ForMember(p => p.Id, c => c.MapFrom(x => x.Id))
                .ForMember(p => p.CategoryId, c => c.MapFrom(card => card.Category.Id))
                .ForMember(p => p.CardScoresIds, c => c.MapFrom(card => card.Assessment.Select(x => x.Id)))
                .ReverseMap();

            CreateMap<Category, CategoryModel>()
                .ForMember(p => p.Id, c => c.MapFrom(x => x.Id))
                .ForMember(p => p.CardsId, c => c.MapFrom(card => card.Cards.Select(x => x.Id)))
                .ReverseMap();

            CreateMap<User, UserModel>()
                .ForMember(p => p.Id, c => c.MapFrom(user => user.Id))
                .ReverseMap();

            CreateMap<CardScore, CardScoreModel>()
               .ForMember(p => p.Id, c => c.MapFrom(card => card.Id))
               .ReverseMap();

            CreateMap<Role, UserRoleModel>()
               .ForMember(p => p.Id, c => c.MapFrom(role => role.Id))
               .ForMember(p => p.Role, c => c.MapFrom(role => role.Name))
               .ReverseMap();

        }
    }
}
