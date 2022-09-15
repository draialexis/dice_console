using Model;
using System.Diagnostics;

namespace App
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            Debug.WriteLine("Hello Debug!");
            Player player = new Player("John");
            Die die = new Die("I'm a die");
            Debug.WriteLine(player.Name);
            Debug.WriteLine(die.Name);
        }
    }
}
