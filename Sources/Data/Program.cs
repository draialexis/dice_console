
using Data.EF;
using Data.EF.Players;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Model.Players;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Intrinsics.Arm;

namespace Data
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            using (DiceAppDbContext db = new DiceAppDbContextWithStub()) // we will remove the "WithStub" bit when we release
            {
                if (db.Players is not null)
                {
                    foreach (PlayerEntity entity in db.Players)
                    {
                        Debug.WriteLine($"{entity.ID} -- {entity.Name}");
                    }
                }
            }
        }
    }
}