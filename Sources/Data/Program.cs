
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
            using (PlayerDBManager playerDBManager = new())
            {
                PrintTable(playerDBManager.GetAll(), "Before");
                try
                {
                    playerDBManager.Add(PlayerExtensions.ToEntity(new Player("Ernesto")));

                }
                catch (ArgumentException ex)
                {
                    Debug.WriteLine($"{ex.Message}\n... Never mind");
                }
                PrintTable(playerDBManager.GetAll(), "After");
            }
        }

        static void PrintTable(IEnumerable table, string description)
        {
            Debug.WriteLine(description);
            if (table is not null)
            {
                foreach (var entity in table)
                {
                    Debug.WriteLine(entity);
                }
            }
        }
    }
}