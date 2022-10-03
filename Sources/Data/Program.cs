
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
            using PlayerDBManager playerDBManager = new();
            try { playerDBManager.Add(new Player("Ernesto").ToEntity()); }
            catch (ArgumentException ex) { Debug.WriteLine($"{ex.Message}\n... Never mind"); }
            foreach (PlayerEntity entity in playerDBManager.GetAll()) { Debug.WriteLine(entity); }
        }
    }
}