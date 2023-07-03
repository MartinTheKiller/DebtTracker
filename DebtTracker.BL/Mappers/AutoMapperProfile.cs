using AutoMapper;
using DebtTracker.BL.Models;
using DebtTracker.DAL.Entities;

namespace DebtTracker.BL.Mappers;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<DebtEntity, DebtDetailModel>()
            .ForMember(m => m.Creditor, expr => expr.Ignore())
            .ForMember(m => m.Debtor, expr => expr.Ignore())
            .ForMember(m => m.Group, expr => expr.Ignore())
            .ReverseMap();
        CreateMap<DebtEntity, DebtListModel>();

        CreateMap<GroupEntity, GroupDetailModel>()
            .ForMember(m => m.Debts, expr => expr.Ignore())
            .ForMember(m => m.Users, expr => expr.Ignore())
            .ReverseMap();
        CreateMap<GroupEntity, GroupListModel>();

        CreateMap<RegisteredGroupEntity, RegisteredGroupModel>()
            .ForMember(m => m.Group, expr => expr.Ignore())
            .ForMember(m => m.User, expr => expr.Ignore())
            .ReverseMap();

        CreateMap<UserEntity, UserDetailModel>()
            .ForMember(m => m.LentDebts, expr => expr.Ignore())
            .ForMember(m => m.OwesDebts, expr => expr.Ignore())
            .ForMember(m => m.Groups, expr => expr.Ignore())
            .ReverseMap();
        CreateMap<UserEntity, UserListModel>();
        CreateMap<UserEntity, UserLoginModel>();
    }
}