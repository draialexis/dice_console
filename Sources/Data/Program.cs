using Data.EF.Players;
using Model.Players;
using System.Diagnostics;

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