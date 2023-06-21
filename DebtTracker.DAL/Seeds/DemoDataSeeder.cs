using Microsoft.EntityFrameworkCore;

namespace DebtTracker.DAL.Seeds;

public static class DemoDataSeeder
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
        GroupSeeds.Group1Entity.Debts.Add(DebtSeeds.UserCreditorUserDebtorDebt1Entity);
        GroupSeeds.Group1Entity.Debts.Add(DebtSeeds.UserCreditorUserDebtorDebt2Entity);
    }

    private static void FillUserEntityCollections()
    {
        UserSeeds.UserDebtorEntity.Groups.Add(RegisteredGroupSeeds.UserDebtorGroup1RegistrationEntity);
        UserSeeds.UserCreditorEntity.Groups.Add(RegisteredGroupSeeds.UserCreditorGroup1RegistrationEntity);
        
        UserSeeds.UserDebtorEntity.OwesDebts.Add(DebtSeeds.UserCreditorUserDebtorDebt1Entity);
        UserSeeds.UserCreditorEntity.LentDebts.Add(DebtSeeds.UserCreditorUserDebtorDebt1Entity);
        
        UserSeeds.UserDebtorEntity.OwesDebts.Add(DebtSeeds.UserCreditorUserDebtorDebt2Entity);
        UserSeeds.UserCreditorEntity.LentDebts.Add(DebtSeeds.UserCreditorUserDebtorDebt2Entity);
    }
}