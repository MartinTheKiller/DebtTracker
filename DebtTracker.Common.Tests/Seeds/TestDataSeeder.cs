using System.Reflection;
using DebtTracker.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DebtTracker.Common.Tests.Seeds;

public static class TestDataSeeder
{
    private static readonly List<object?> Users = typeof(UserSeeds)
                                                    .GetFields(BindingFlags.Public | BindingFlags.Static)
                                                    .Where(i => i.FieldType == typeof(UserEntity))
                                                    .Select(i => i.GetValue(null))
                                                    .ToList();

    private static readonly List<object?> Groups = typeof(GroupSeeds)
                                                     .GetFields(BindingFlags.Public | BindingFlags.Static)
                                                     .Where(i => i.FieldType == typeof(GroupEntity))
                                                     .Select(i => i.GetValue(null))
                                                     .ToList();

    private static readonly List<object?> RegisteredGroups = typeof(RegisteredGroupSeeds)
                                                                .GetFields(BindingFlags.Public | BindingFlags.Static)
                                                                .Where(i => i.FieldType == typeof(RegisteredGroupEntity))
                                                                .Select(i => i.GetValue(null))
                                                                .ToList();

    private static readonly List<object?> Debts = typeof(DebtSeeds)
                                                        .GetFields(BindingFlags.Public | BindingFlags.Static)
                                                        .Where(i => i.FieldType == typeof(DebtEntity))
                                                        .Select(i => i.GetValue(null))
                                                        .ToList();

    private static bool _collectionsFilled = false;

    public static void Seed(ModelBuilder modelBuilder)
    {
        DebtSeeds.Seed(modelBuilder);
        GroupSeeds.Seed(modelBuilder);
        UserSeeds.Seed(modelBuilder);
        RegisteredGroupSeeds.Seed(modelBuilder);

        if (!_collectionsFilled)
        {
            FillGroupEntityCollections();
            FillUserEntityCollections();
            _collectionsFilled = true;
        }
    }

    private static void FillGroupEntityCollections()
    {
        foreach (GroupEntity? group in Groups)
        {
            if (group is null) continue;

            foreach (RegisteredGroupEntity? registeredGroup in RegisteredGroups)
            {
                if (registeredGroup is null) continue;

                if (registeredGroup.GroupId == group.Id)
                {
                    group.Users.Add(registeredGroup);
                }
            }

            foreach (DebtEntity? debt in Debts)
            {
                if (debt is null) continue;

                if (debt.GroupId == group.Id)
                {
                    group.Debts.Add(debt);
                }
            }
        }
    }

    private static void FillUserEntityCollections()
    {
        foreach (UserEntity? user in Users)
        {
            if (user is null) continue;

            foreach (DebtEntity? debt in Debts)
            {
                if (debt is null) continue;

                if (debt.CreditorId == user.Id)
                {
                    user.LentDebts.Add(debt);
                }
                else if (debt.DebtorId == user.Id)
                {
                    user.OwesDebts.Add(debt);
                }
            }

            foreach (RegisteredGroupEntity? registeredGroup in RegisteredGroups)
            {
                if (registeredGroup is null) continue;

                if (registeredGroup.UserId == user.Id)
                {
                    user.Groups.Add(registeredGroup);
                }
            }
        }
    }
}