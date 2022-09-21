using Data;
using Model;

namespace App
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            ILoader loader = new Stub();
            GameRunner gameRunner = loader.LoadApp();
            // use gameRunner to play
        }
    }
}
