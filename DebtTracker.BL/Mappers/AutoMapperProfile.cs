using AutoMapper;
using DebtTracker.BL.Models;
using DebtTracker.DAL.Entities;

namespace DebtTracker.BL.Mappers;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<DebtEntity, DebtDetailModel>().ReverseMap();
        CreateMap<DebtEntity, DebtListModel>();

        CreateMap<GroupEntity, GroupDetailModel>().ReverseMap();
        CreateMap<GroupEntity, GroupListModel>();

        CreateMap<RegisteredGroupEntity, RegisteredGroupModel>().ReverseMap();

        CreateMap<UserEntity, UserDetailModel>().ReverseMap();
        CreateMap<UserEntity, UserListModel>();
    }
}