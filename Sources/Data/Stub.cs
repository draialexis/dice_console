using Data;
using Model;

namespace Stub
{
    public class Stub : ILoader
    {

        // when the other classes are ready
        // the Stub should just make and return a GameRunner, and the GameRunner should have
        // a PlayerManager, a collection of Games, a FavGroupManager, etc. (see diagram)

        public GameRunner LoadApp()
        {
            // this doesn't do much for now, because the class isn't coded
            return new GameRunner();
        }

        public static List<Player> LoadPlayers()
        {
            List<Player> list = new()
            {
                new Player("name 1"),
                new Player("name 2"),
                new Player("name 3"),
                new Player("name 4"),
                new Player("name 5"),
                new Player("name 6")
            };
            return list;
        }

        public static List<Die> LoadDices()
        {
            List<Die> list = new()
            {
                new Die("Dice 1"),
                new Die("Dice 1"),
                new Die("Dice 1"),
                new Die("Dice 1"),
                new Die("Dice 1"),
                new Die("Dice 1")
            };

            return list;
        }

        public static List<NumberDieFace> LoadNumFaces()
        {
            List<NumberDieFace> list = new()
            {
                new NumberDieFace(1),
                new NumberDieFace(2),
                new NumberDieFace(3),
                new NumberDieFace(4),
                new NumberDieFace(5),
                new NumberDieFace(6),
                new NumberDieFace(7)
            };

            return list;
        }

        public static List<ColorDieFace> LoadClrFaces()
        {
            List<ColorDieFace> list = new()
            {
                new ColorDieFace("ffffff"),
                new ColorDieFace("ffff66"),
                new ColorDieFace("ffff11"),
                new ColorDieFace("ffff22"),
                new ColorDieFace("ffff33"),
                new ColorDieFace("ffff44"),
                new ColorDieFace("ffff55")
            };

            return list;
        }

        public static List<ImageDieFace> LoadImgFaces()
        {
            string urlBase = "http://baseUrl/img/";
            List<ImageDieFace> list = new()
            {
                new ImageDieFace( urlBase + 1),
                new ImageDieFace( urlBase + 2),
                new ImageDieFace( urlBase + 3),
                new ImageDieFace( urlBase + 4),
                new ImageDieFace( urlBase + 5),
                new ImageDieFace( urlBase + 6),
                new ImageDieFace( urlBase + 7),
            };

            return list;
        }
    }
}