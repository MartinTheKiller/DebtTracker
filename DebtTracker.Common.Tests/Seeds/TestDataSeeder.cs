using DebtTracker.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DebtTracker.Common.Tests.Seeds;

public static class TestDataSeeder
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        DebtSeeds.Seed(modelBuilder);
        GroupSeeds.Seed(modelBuilder);
        UserSeeds.Seed(modelBuilder);
        RegisteredGroupSeeds.Seed(modelBuilder);

        FillGroupEntityCollections();
        FillUserEntityCollections();
    }

    private static void FillGroupEntityCollections()
    {
        GroupSeeds.Group1Entity.Users.Add(RegisteredGroupSeeds.UserCreditorGroup1RegistrationEntity);
        GroupSeeds.Group1Entity.Users.Add(RegisteredGroupSeeds.UserDebtorGroup1RegistrationEntity);
        GroupSeeds.Group1Entity.Users.Add(RegisteredGroupSeeds.UserToDeleteCascadeGroup1RegistrationToCascadeDeleteEntity);
        GroupSeeds.Group1Entity.Debts.Add(DebtSeeds.UserCreditorUserDebtorDebt1Entity);
        GroupSeeds.Group1Entity.Debts.Add(DebtSeeds.UserCreditorUserDebtorDebt2Entity);

        GroupSeeds.GroupToDeleteCascadeEntity.Users.Add(RegisteredGroupSeeds.UserCreditorGroupToDeleteCascadeRegistrationToCascadeDeleteEntity);
        GroupSeeds.GroupToDeleteCascadeEntity.Users.Add(RegisteredGroupSeeds.UserDebtorGroupToDeleteCascadeRegistrationToCascadeDeleteEntity);
        GroupSeeds.GroupToDeleteCascadeEntity.Debts.Add(DebtSeeds.UserCreditorUserDebtorDebtToCascadeDeleteEntity);
    }

    private static void FillUserEntityCollections()
    {
        UserSeeds.UserDebtorEntity.Groups.Add(RegisteredGroupSeeds.UserDebtorGroup1RegistrationEntity);
        UserSeeds.UserCreditorEntity.Groups.Add(RegisteredGroupSeeds.UserCreditorGroup1RegistrationEntity);
        UserSeeds.UserToDeleteCascadeEntity.Groups.Add(RegisteredGroupSeeds.UserToDeleteCascadeGroup1RegistrationToCascadeDeleteEntity);
        
        UserSeeds.UserDebtorEntity.OwesDebts.Add(DebtSeeds.UserCreditorUserDebtorDebt1Entity);
        UserSeeds.UserCreditorEntity.LentDebts.Add(DebtSeeds.UserCreditorUserDebtorDebt1Entity);
        
        UserSeeds.UserDebtorEntity.OwesDebts.Add(DebtSeeds.UserCreditorUserDebtorDebt2Entity);
        UserSeeds.UserCreditorEntity.LentDebts.Add(DebtSeeds.UserCreditorUserDebtorDebt2Entity);

        UserSeeds.UserToDeleteCascadeEntity.OwesDebts.Add(DebtSeeds.UserCreditorUserToDeleteCascadeDebtToCascadeDeleteEntity);

        UserSeeds.UserToDeleteCascadeEntity.LentDebts.Add(DebtSeeds.UserToDeleteCascadeUserDebtorDebtToCascadeDeleteEntity);
    }
}